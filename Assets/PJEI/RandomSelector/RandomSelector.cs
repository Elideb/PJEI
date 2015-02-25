using System;
using System.Collections.Generic;

namespace PJEI.RandomSelector {

    public static class SelectionOperator {
        private static SelectionOperator<float> floatOp = new SelectionOperator<float>(0f,
                                                                                       (v1, v2) => v1 + v2,
                                                                                       (v1, v2) => v1 - v2,
                                                                                       (v1, v2) => UnityEngine.Random.Range(v1, v2),
                                                                                       (v1, v2) => v1 > v2);
        public static SelectionOperator<float> FloatOp { get { return SelectionOperator.floatOp; } }

        private static SelectionOperator<int> intOp = new SelectionOperator<int>(0,
                                                                                 (v1, v2) => v1 + v2,
                                                                                 (v1, v2) => v1 - v2,
                                                                                 (v1, v2) => UnityEngine.Random.Range(v1, v2),
                                                                                 (v1, v2) => v1 > v2);
        public static SelectionOperator<int> IntOp { get { return SelectionOperator.intOp; } }
    }

    public class SelectionOperator<T> {

        private T zeroSum = default(T);
        public T Zero { get { return this.zeroSum; } }
        private Func<T, T, T> sum;
        private Func<T, T, T> subtract;
        private Func<T, T, T> randomVal;
        private Func<T, T, bool> greaterThan;

        public SelectionOperator(T zeroSum, Func<T, T, T> sum, Func<T, T, T> subtract, Func<T, T, T> randomVal, Func<T, T, bool> greaterThan) {
            this.zeroSum = zeroSum;
            this.sum = sum;
            this.subtract = subtract;
            this.randomVal = randomVal;
            this.greaterThan = greaterThan;
        }

        public T Sum(T v1, T v2) { return this.sum(v1, v2); }
        public T Subtract(T v1, T v2) { return this.subtract(v1, v2); }
        public T Random(T min, T max) { return this.randomVal(min, max); }
        public bool GreaterThan(T v1, T v2) { return this.greaterThan(v1, v2); }
    }

    public static class RandomSelector {

        /// <summary>
        /// Create a new selector for elements associated to float non-normalized probabilities.
        /// </summary>
        /// <typeparam name="T">Type of the elements the selector works with.</typeparam>
        /// <param name="container">Collection of elements to include in the selector created.</param>
        /// <param name="probCalculation">A function to calculate the probability associated to each of the elements received.</param>
        /// <returns>The created selector, with all the corresponding elements added.</returns>
        public static RandomSelector<T, float> CreateFloatSelector<T>(IEnumerable<T> container, System.Func<T, float> probCalculation) {
            var selector = new RandomSelector<T, float>(SelectionOperator.FloatOp, probCalculation);
            foreach (var value in container)
                selector.Add(value);

            return selector;
        }

        /// <summary>
        /// Create a new selector for elements associated to int non-normalized probabilities.
        /// </summary>
        /// <typeparam name="T">Type of the elements the selector works with.</typeparam>
        /// <param name="container">Collection of elements to include in the selector created.</param>
        /// <param name="probCalculation">A function to calculate the probability associated to each of the elements received.</param>
        /// <returns>The created selector, with all the corresponding elements added.</returns>
        public static RandomSelector<T, int> CreateIntSelector<T>(IEnumerable<T> container, System.Func<T, int> probCalculation) {
            var selector = new RandomSelector<T, int>(SelectionOperator.IntOp, probCalculation);
            foreach (var value in container)
                selector.Add(value);

            return selector;
        }
    }

    /// <summary>
    /// Class used to easily select random elements with non-normalized probabilities.
    /// It is recommended to not create elements of this type directly, but using the static helpers in <see cref="RandomSelector"/>.
    /// </summary>
    /// <typeparam name="TContained">Type of the elements to accumulate and select.</typeparam>
    /// <typeparam name="TProb">Type used for the probabilities. It must have an implementation of <see cref="SelectionOperator<TProb>"/></typeparam>
    public class RandomSelector<TContained, TProb> {

        private HashSet<TContained> contents = new HashSet<TContained>();
        private SelectionOperator<TProb> selectorOps;
        private Func<TContained, TProb> probCalculation;

        public bool IsEmpty { get { return this.contents.Count == 0; } }

        public RandomSelector(SelectionOperator<TProb> selectorOps, Func<TContained, TProb> func) {
            this.selectorOps = selectorOps;
            this.probCalculation = func;
        }

        /// <summary>
        /// Add a new element wit a given probability to the selector.
        /// </summary>
        /// <param name="probability">Probability associated with the element. Does not need to be in the range [0..1].</param>
        /// <param name="value">Value to add.</param>
        public void Add(TContained value) {
            if (!this.contents.Contains(value)) {
                this.contents.Add(value);
            }
        }

        /// <summary>
        /// Remove an element from the collection, if it is present.
        /// </summary>
        /// <param name="value">Element to remove.</param>
        public void Remove(TContained value) {
            if (this.contents.Contains(value)) {
                this.contents.Remove(value);
            }
        }

        /// <summary>
        /// Select a random element from the collection and remove it for later queries.
        /// </summary>
        /// <returns>The element selected.</returns>
        /// <exception cref="System.InvalidOperationException">If the selector is empty.</exception>
        public TContained SelectAndRemove() {
            var selected = this.Select();
            this.Remove(selected);
            return selected;
        }

        /// <summary>
        /// Select a random element from the collection.
        /// </summary>
        /// <returns>The element selected.</returns>
        /// <exception cref="System.InvalidOperationException">If the selector is empty.</exception>
        public TContained Select() {
            if (this.IsEmpty)
                throw new System.InvalidOperationException("Cannot select from an empty collection.");

            TProb totalProbability = this.selectorOps.Zero;
            Dictionary<TContained, TProb> values = new Dictionary<TContained, TProb>(this.contents.Count);
            foreach (var value in this.contents) {
                var prob = this.probCalculation(value);
                values.Add(value, prob);
                totalProbability = this.selectorOps.Sum(totalProbability, prob);
            }

            TProb selectedValue = this.selectorOps.Random(this.selectorOps.Zero, totalProbability);
            TProb acc = this.selectorOps.Zero;

            foreach (var entry in values) {
                acc = this.selectorOps.Sum(acc, entry.Value);
                if (this.selectorOps.GreaterThan(acc, entry.Value))
                    return entry.Key;
            }

            // Return a random entry. We selected nothing if all probs were zero.
            int val = UnityEngine.Random.Range(0, this.contents.Count);
            TContained selected = default(TContained);
            foreach (var entry in this.contents) {
                if (val == 0) {
                    selected = entry;
                    break;
                }

                --val;
            }

            return selected;
        }
    }
}
