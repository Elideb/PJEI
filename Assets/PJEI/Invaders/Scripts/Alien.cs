using UnityEngine;
using System.Collections;

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

        public void Initialize(Vector2D initialPosition, int firstSprite, int pixelsPerMove) {
            this.pixelsPerMove = pixelsPerMove;
            spriteRenderer = GetComponent<SpriteRenderer>();

            SetPosition(initialPosition);
            SetSprite(firstSprite);
        }

        public void StartExecution() {

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
    }
}
