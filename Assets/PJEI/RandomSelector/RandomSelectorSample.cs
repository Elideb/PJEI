using System.Collections.Generic;
using UnityEngine;

namespace PJEI.RandomSelector {
    public class RandomSelectorSample {

        public class Attack {

            public virtual float EvaluateProbability() {
                return Random.Range(0f, 1f);
            }

        }

        public class ImpossibleAttack : Attack {
            public override float EvaluateProbability() {
                return 0;
            }
        }

        private RandomSelector<Attack, float> selector;

        /// <summary>
        /// Must be called before attempting to call SelectAttack.
        /// </summary>
        public void Initialize() {
            // Create a group of attacks
            List<Attack> attacks = new List<Attack>();
            for(int i = 0; i < 4; ++i)
                attacks.Add(new Attack());

            attacks.Add(new ImpossibleAttack());

            // Create the random selector.
            // We pass as parameters the attacks to select from
            // and a function which receives an Attack (attack)
            // and returns a float probability value from it.
            selector = RandomSelector.CreateFloatSelector(attacks, this.GetAttackProbability);
        }

        private float GetAttackProbability(Attack attack) {
            return attack.EvaluateProbability();
        }

        /// <summary>
        /// Select one of the available attacks at random.
        /// </summary>
        /// <returns></returns>
        public Attack SelectAttack() {
            return selector.Select();
        }

    }
}
