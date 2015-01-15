using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PJEI {
    public class DelayedDisposer : MonoBehaviour {

        Dictionary<IDisposable, float> gameTimeDisposers = new Dictionary<IDisposable, float>();
        Dictionary<IDisposable, float> realTimeDisposers = new Dictionary<IDisposable, float>();

        void OnEnable() {
            var delayedDisposers = FindObjectsOfType<DelayedDisposer>();
            if (delayedDisposers.Length > 1) {
                Debug.LogWarning("Found more than one delayed disposer!");
                Destroy(this);
            }
        }

        void Update() {
            if (this.gameTimeDisposers.Any()) {
                var disposed = new HashSet<IDisposable>();
                foreach (var kvp in this.gameTimeDisposers) {
                    if (kvp.Value < Time.time) {
                        kvp.Key.Dispose();
                        disposed.Add(kvp.Key);
                    }
                }

                foreach (var disp in disposed)
                    this.gameTimeDisposers.Remove(disp);
            }

            if (this.realTimeDisposers.Any()) {
                var disposed = new HashSet<IDisposable>();
                foreach (var kvp in this.realTimeDisposers) {
                    if (kvp.Value < Time.realtimeSinceStartup) {
                        kvp.Key.Dispose();
                        disposed.Add(kvp.Key);
                    }
                }

                foreach (var disp in disposed)
                    this.realTimeDisposers.Remove(disp);
            }
        }

        public void AddGameTimeDisposer(IDisposable disposable, float time) {
            this.gameTimeDisposers.Add(disposable, Time.time + time);
        }

        public void AddRealTimeDisposer(IDisposable disposable, float time) {
            this.realTimeDisposers.Add(disposable, Time.realtimeSinceStartup + time);
        }
    }
}
