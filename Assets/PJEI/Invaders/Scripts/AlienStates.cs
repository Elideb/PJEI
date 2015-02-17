using StateMachine;

namespace PJEI.Invaders {

    /// <summary>
    /// Reference alien state to use as a base to create the actual alien states.
    /// </summary>
    ////class SampleAlienState : State<Alien> {

    ////    #region Singleton

    ////    private static SampleAlienState instance = null;
    ////    public static SampleAlienState Instance {
    ////        get {
    ////            if (instance == null)
    ////                instance = new SampleAlienState();

    ////            return instance;
    ////        }
    ////    }

    ////    #endregion

    ////    private SampleAlienState() {
    ////        // Assign the functions to use for each case.
    ////        // OnEnter, OnUpdate and OnExit are variables which point to functions
    ////        // and can later call them, passing the proper parameters.
    ////        // If null is assigned to any of them, no function is called.
    ////        OnEnter = EnterState;
    ////        OnUpdate = UpdateState;
    ////        OnExit = ExitState;
    ////    }

    ////    private void EnterState(Alien alien) {

    ////    }

    ////    private void UpdateState(Alien alien) {

    ////    }

    ////    private void ExitState(Alien alien) {

    ////    }

    ////}

    // TODO : Add the actual states, using SampleAlienState as reference.
    // Possible states: StartMoveLeft, MoveDown, MoveLeft, MoveRight
    // Simplified states: Startup, MoveDown, MoveSideways

    class StartMovement : State<Alien> {

        #region Singleton

        private static StartMovement instance = null;
        public static StartMovement Instance {
            get {
                if (instance == null)
                    instance = new StartMovement();

                return instance;
            }
        }

        #endregion

        private StartMovement() {
            // Assign the functions to use for each case.
            // OnEnter, OnUpdate and OnExit are variables which point to functions
            // and can later call them, passing the proper parameters.
            // If null is assigned to any of them, no function is called.
            OnEnter = EnterState;
            OnUpdate = UpdateState;
            OnExit = ExitState;
        }

        private void EnterState(Alien alien) {
            alien.MovesRemaining = 5;
        }

        private void UpdateState(Alien alien) {
            alien.Move(new Vector2D(-1, 0));
            alien.MovesRemaining -= 1;
        }

        private void ExitState(Alien alien) {

        }

    }

    class MoveLeft : State<Alien> {

        #region Singleton

        private static MoveLeft instance = null;
        public static MoveLeft Instance {
            get {
                if (instance == null)
                    instance = new MoveLeft();

                return instance;
            }
        }

        #endregion

        private MoveLeft() {
            // Assign the functions to use for each case.
            // OnEnter, OnUpdate and OnExit are variables which point to functions
            // and can later call them, passing the proper parameters.
            // If null is assigned to any of them, no function is called.
            OnEnter = EnterState;
            OnUpdate = UpdateState;
            OnExit = ExitState;
        }

        private void EnterState(Alien alien) {
            alien.MovesRemaining = 10;
            alien.ShiftMoveDirection();
        }

        private void UpdateState(Alien alien) {
            alien.Move(new Vector2D(-1, 0));
            alien.MovesRemaining -= 1;
        }

        private void ExitState(Alien alien) {

        }

    }

    class MoveRight : State<Alien> {

        #region Singleton

        private static MoveRight instance = null;
        public static MoveRight Instance {
            get {
                if (instance == null)
                    instance = new MoveRight();

                return instance;
            }
        }

        #endregion

        private MoveRight() {
            // Assign the functions to use for each case.
            // OnEnter, OnUpdate and OnExit are variables which point to functions
            // and can later call them, passing the proper parameters.
            // If null is assigned to any of them, no function is called.
            OnEnter = EnterState;
            OnUpdate = UpdateState;
            OnExit = ExitState;
        }

        private void EnterState(Alien alien) {
            alien.MovesRemaining = 10;
            alien.ShiftMoveDirection();
        }

        private void UpdateState(Alien alien) {
            alien.Move(new Vector2D(1, 0));
            alien.MovesRemaining -= 1;
        }

        private void ExitState(Alien alien) {

        }

    }

    class MoveDown : State<Alien> {

        #region Singleton

        private static MoveDown instance = null;
        public static MoveDown Instance {
            get {
                if (instance == null)
                    instance = new MoveDown();

                return instance;
            }
        }

        #endregion

        private MoveDown() {
            // Assign the functions to use for each case.
            // OnEnter, OnUpdate and OnExit are variables which point to functions
            // and can later call them, passing the proper parameters.
            // If null is assigned to any of them, no function is called.
            OnEnter = EnterState;
            OnUpdate = UpdateState;
            OnExit = ExitState;
        }

        private void EnterState(Alien alien) {
            alien.Move(new Vector2D(0, -1));
        }

        private void UpdateState(Alien alien) {

        }

        private void ExitState(Alien alien) {

        }

    }
}
