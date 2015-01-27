using UnityEngine;
using System.Collections;

namespace PJEI.Invaders {

    [RequireComponent(typeof(SpriteRenderer))]
    public class AlienRenderer : MonoBehaviour {

        public Sprite[] sprites;
        private int currentSprite = 0;

        private int pixelsPerMove = 1;

        private SpriteRenderer spriteRenderer;
        private Vector2D currentPos;

        public void Intialize(Vector2D initialPosition, int firstSprite, int pixelsPerMove) {
            this.pixelsPerMove = pixelsPerMove;
            SetPosition(initialPosition);
            SetSprite(firstSprite);
        }

        public void Move(Vector2D distance) {
            SetPosition(new Vector2D(currentPos.x + distance.x, currentPos.y + distance.y));
            SetSprite(currentSprite + 1);
        }

        private void SetPosition(Vector2D position) {
            this.transform.position = pixelsPerMove * new Vector3(position.x, position.y, 0);
        }

        private void SetSprite(int spriteCount) {
            currentSprite = spriteCount % sprites.Length;
            spriteRenderer.sprite = sprites[currentSprite];
        }
    }
}
