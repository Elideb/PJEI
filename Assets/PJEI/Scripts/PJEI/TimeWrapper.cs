using System;
using System.Collections.Generic;
using System.Linq;
using PJEI.Libraries;
using UnityEngine;

namespace PJEI {
    public static class TimeWrapper {

        private static int activePauses = 0;

        public static bool Paused { get { return activePauses > 0; } }

        private static int nextDisposerID = 0;
        private static Dictionary<int, IDisposable> activeDisposers = new Dictionary<int, IDisposable>();

        private static float baseFixedTime = 0;

        /// <summary>
        /// The actual time scale used by the game. Can be 0 if the game is paused.
        /// </summary>
        public static float CurrentTimeScale {
            get {
                return Paused ? 0 : ModifiedTimeScale;
            }
        }

        private static List<float> activeWarps = new List<float>();

        /// <summary>
        /// Time warps which override the active ones. Only the last one is active at any time.
        /// Use Nullable as reference type, so we can keep a reference and remove them at will.
        /// </summary>
        private static List<float?> overrides = new List<float?>();

        /// <summary>
        /// The time scale used by the game when not in pause. Cannot be 0.
        /// </summary>
        public static float ModifiedTimeScale {
            get {
                if (overrides.Any()) {
                    return overrides.Last().Value;
                }

                float agg = 1;
                for (int i = 0; i < activeWarps.Count; ++i) {
                    agg *= activeWarps[i];
                }

                return agg;
            }
        }

        static TimeWrapper() {
            baseFixedTime = Time.fixedDeltaTime;
        }

        public static void ResetTime() {
            // Invalidate all disposers
            while (activeDisposers.Any()) {
                var disposer = activeDisposers.Values.First();
                disposer.Dispose();
            }

            Time.timeScale = 1;
        }

        /// <summary>
        /// Pause the game, obtaining a disposer to undo the pause when desired.
        /// </summary>
        /// <returns>An IDisposable to call when the pause ends.</returns>
        public static IDisposable Pause() {
            ++activePauses;
            if (activePauses == 1) {
                Time.timeScale = 0;
            }

            return CreateDisposableAction(Resume);
        }

        private static void Resume() {
            --activePauses;
            Time.timeScale = CurrentTimeScale;

            if (activePauses < 0) {
                Debug.LogError("Disposed of time wrapper pause " + -activePauses + " too many.");
                activePauses = 0;
            }
        }

        /// <summary>
        /// Multiply time scale by a factor.
        /// </summary>
        /// <param name="factor">A positive value to multiply time scale by.
        /// If 0, you should use <see cref="TimeWrapper.Pause()"/> instead.</param>
        /// <returns>A disposable to call when the time warp must be undone.</returns>
        public static IDisposable WarpTime(float factor) {
            if (factor < .0001f)
                throw new ArgumentOutOfRangeException("factor",
                    "Must be a positive value, but was " + factor + ". If 0, consider pausing. If lower, don't event think about that.");

            activeWarps.Add(factor);
            Time.timeScale = CurrentTimeScale;
            Time.fixedDeltaTime = baseFixedTime * ModifiedTimeScale;

            return CreateDisposableAction(() => UnwarpTime(factor));
        }

        /// <summary>
        /// Apply a forced warp, disregarding all other warps.
        /// Overrides do not stack, but are recovered after the active one is disposed.
        /// </summary>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static IDisposable OverrideTime(float factor) {
            if (factor < .0001f)
                throw new ArgumentOutOfRangeException("factor",
                    "Must be a positive value, but was " + factor + ". If 0, consider pausing. If lower, don't event think about that.");

            float? refFactor = factor;
            overrides.Add(refFactor);
            Time.timeScale = CurrentTimeScale;
            Time.fixedDeltaTime = baseFixedTime * ModifiedTimeScale;

            return CreateDisposableAction(() => UndoOverride(refFactor));
        }

        private static void UndoOverride(float? refFactor) {
            if (overrides.Remove(refFactor)) {
                Time.timeScale = CurrentTimeScale;
                Time.fixedDeltaTime = baseFixedTime * ModifiedTimeScale;
            }
        }

        private static void UnwarpTime(float factor) {
            if (activeWarps.Remove(factor)) {
                Time.timeScale = CurrentTimeScale;
                Time.fixedDeltaTime = baseFixedTime * ModifiedTimeScale;
            }
        }

        private static IDisposable CreateDisposableAction(Action action) {
            var disposer = WrapActionInDisposable(nextDisposerID, action);
            activeDisposers.Add(nextDisposerID, disposer);
            ++nextDisposerID;
            return disposer;
        }

        private static IDisposable WrapActionInDisposable(int disposerID, Action action) {
            return Disposable.Create(() => {
                if (activeDisposers.Remove(disposerID)) {
                    action();
                }
            });
        }

        private static DelayedDisposer delayedDisposer;
        private static DelayedDisposer DelayedDisposer {
            get {
                if (delayedDisposer == null)
                    delayedDisposer = GameObject.FindObjectOfType<DelayedDisposer>();
                if (delayedDisposer == null)
                    delayedDisposer = new GameObject("DelayedDisposer").AddComponent<DelayedDisposer>();

                return delayedDisposer;
            }
        }

        /// <summary>
        /// Multiply time scale by a factor for the given game time seconds.
        /// </summary>
        /// <param name="factor">A positive value to multiply time scale by.
        /// If 0, you should use <see cref="TimeWrapper.Pause()"/> instead.</param>
        /// <param name="gameSeconds">How long the warp lasts, in game time (affected by the warping).</param>
        public static void WarpTimeForGameSeconds(float factor, float gameSeconds) {
            var disp = TimeWrapper.WarpTime(factor);
            DelayedDisposer.AddGameTimeDisposer(disp, gameSeconds);
        }

        /// <summary>
        /// Multiply time scale by a factor for the given real time seconds.
        /// </summary>
        /// <param name="factor">A positive value to multiply time scale by.
        /// If 0, you should use <see cref="TimeWrapper.Pause()"/> instead.</param>
        /// <param name="realSeconds">How long the warp lasts, in real time (unaffected by the warping).</param>
        public static void WarpTimeForRealSeconds(float factor, float realSeconds) {
            var disp = TimeWrapper.WarpTime(factor);
            DelayedDisposer.AddRealTimeDisposer(disp, realSeconds);
        }

        /// <summary>
        /// Override time scale to a factor for the given game time seconds.
        /// </summary>
        /// <param name="factor">A positive value to set time scale to.
        /// If 0, you should use <see cref="TimeWrapper.Pause()"/> instead.</param>
        /// <param name="gameSeconds">How long the warp lasts, in game time (affected by the warping).</param>
        public static void OverrideTimeForGameSeconds(float factor, float gameSeconds) {
            var disp = TimeWrapper.OverrideTime(factor);
            DelayedDisposer.AddGameTimeDisposer(disp, gameSeconds);
        }

        /// <summary>
        /// Override time scale to a factor for the given game time seconds.
        /// </summary>
        /// <param name="factor">A positive value to set time scale to.
        /// If 0, you should use <see cref="TimeWrapper.Pause()"/> instead.</param>
        /// <param name="realSeconds">How long the warp lasts, in game time (affected by the warping).</param>
        public static void OverrideTimeForRealSeconds(float factor, float realSeconds) {
            var disp = TimeWrapper.OverrideTime(factor);
            DelayedDisposer.AddRealTimeDisposer(disp, realSeconds);
        }

        /// <summary>
        /// Pause the game for the given real time seconds.
        /// </summary>
        /// <param name="realSeconds">Real time to maintain the game paused.</param>
        public static void Pause(float realSeconds) {
            var disp = TimeWrapper.Pause();
            DelayedDisposer.AddRealTimeDisposer(disp, realSeconds);
        }
    }
}
