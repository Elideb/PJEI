using StateMachine;
using UnityEngine;

namespace PJEI.Invaders {

    [RequireComponent(typeof(SpriteRenderer))]
    public class Alien : MonoBehaviour {

        public Sprite[] sprites;
        private int currentSprite = 0;

        private int pixelsPerMove = 0;

        private SpriteRenderer spriteRenderer;
        private Vector2D currentPos;

        #region State information

        private int movesRemaining = 0;
        public int MovesRemaining {
            get { return movesRemaining; }
            set { movesRemaining = Mathf.Max(0, value); }
        }

        public bool HasFinishedMoving() { return MovesRemaining == 0; }

        private bool movingLeft = true;
        public void StartMovingLeft() { movingLeft = true; }
        public void StartMovingRight() { movingLeft = false; }
        public bool IsMovingLeft() { return movingLeft; }
        public bool IsMovingRight() { return movingLeft == false; }
        public void ShiftMoveDirection() { movingLeft = !movingLeft; }

        #endregion

        private StateMachine<Alien> stateMachine;

        public void Initialize(Vector2D initialPosition, int firstSprite, int pixelsPerMove) {
            this.pixelsPerMove = pixelsPerMove;
            spriteRenderer = GetComponent<SpriteRenderer>();

            SetPosition(initialPosition);
            SetSprite(firstSprite);

            // TODO : Build the alien state machine
            stateMachine = new StateMachine<Alien>(this, SampleAlienState.Instance)
                .AddTransitions(
                    Transition.From(SampleAlienState.Instance)
                              .To(SampleAlienState.Instance)
                              .When(HasFinishedMoving),
                    Transition.FromAny<Alien>()
                              .Except(SampleAlienState.Instance)
                              .To(SampleAlienState.Instance)
                              .When(IsMovingLeft),
                    Transition.From(SampleAlienState.Instance)
                              .ToPrev()
                              .When(IsMovingRight),
                    Transition.From(SampleAlienState.Instance)
                              .To(SampleAlienState.Instance)
                              .Always() );
        }

        public void StartExecution() {
            StartCoroutine(StateMachineUpdate());
        }

        public void Move(Vector2D distance) {
            SetPosition(new Vector2D(currentPos.x + distance.x, currentPos.y + distance.y));
            SetSprite(currentSprite + 1);
        }

        private void SetPosition(Vector2D position) {
            currentPos = position;
            this.transform.position = new Vector3(pixelsPerMove * position.x, (pixelsPerMove / 2) * position.y, 0);
        }

        private void SetSprite(int spriteCount) {
            currentSprite = spriteCount % sprites.Length;
            spriteRenderer.sprite = sprites[currentSprite];
        }

        private System.Collections.IEnumerator StateMachineUpdate() {
            while (true) {
                // TODO : As the number of aliens decrease, this time should decrease too.
                yield return new WaitForSeconds(1f);
                stateMachine.Update();
            }
        }
    }
}
