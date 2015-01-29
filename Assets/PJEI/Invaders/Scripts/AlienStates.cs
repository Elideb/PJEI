using StateMachine;

namespace PJEI.Invaders {

    /// <summary>
    /// Reference alien state to use as a base to create the actual alien states.
    /// </summary>
    class SampleAlienState : State<Alien> {

        #region Singleton

        private static SampleAlienState instance = null;
        public static SampleAlienState Instance {
            get {
                if (instance == null)
                    instance = new SampleAlienState();

                return instance;
            }
        }

        #endregion

        private SampleAlienState() {
            // Assign the functions to use for each case.
            // OnEnter, OnUpdate and OnExit are variables which point to functions
            // and can later call them, passing the proper parameters.
            // If null is assigned to any of them, no function is called.
            OnEnter = EnterState;
            OnUpdate = UpdateState;
            OnExit = ExitState;
        }

        private void EnterState(Alien alien) {

        }

        private void UpdateState(Alien alien) {

        }

        private void ExitState(Alien alien) {

        }

    }

    // TODO : Add the actual states, using SampleAlienState as reference.
    // Possible states: StartMoveLeft, MoveDown, MoveLeft, MoveRight
    // Simplified states: Startup, MoveDown, MoveSideways
}
