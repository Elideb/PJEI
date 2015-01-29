using System;

namespace PJEI.Invaders {

    [Serializable]
    public struct Vector2D {
        public int x;
        public int y;

        public Vector2D(int x, int y)
            : this() {
                this.x = x;
                this.y = y;
        }
    }
}
