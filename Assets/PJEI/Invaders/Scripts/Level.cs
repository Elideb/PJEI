using UnityEngine;

namespace PJEI.Invaders {

    public class Level : MonoBehaviour {
        public Alien[] lines;
        public int width = 11;
        public int pixelsBetweenAliens = 48;

        private Alien[] aliens;

        void Start() {
            StartCoroutine(InitGame());
        }

        private System.Collections.IEnumerator InitGame() {
            aliens = new Alien[lines.Length * width];
            for (int i = lines.Length - 1; i >= 0; --i) {
                for (int j = 0; j < width; ++j) {
                    var alien = (Alien)Instantiate(lines[i]);
                    alien.Initialize(new Vector2D(j - width / 2, -i + lines.Length / 2), i, pixelsBetweenAliens);

                    aliens[i * width + j] = alien;

                    yield return new WaitForSeconds(.05f);
                }
            }

            foreach (var alien in aliens)
                alien.Run();
        }
    }

}
