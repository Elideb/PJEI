using UnityEngine;
using System.Collections;

namespace PJEI.Lanes {
    public class LaneCreator : MonoBehaviour {

        public Transform originLane;
        public Vector2 laneGaps = new Vector2(1, 1.94f);
        public int lanesX = 3;
        public int lanesY = 2;

        private Transform[][] lanes = null;
        private int currentX = 0;
        private int currentY = 0;

        // Use this for initialization
        void Start() {
            lanes = new Transform[lanesX][];
            for (int x = 0; x < lanesX; x++) {
                lanes[x] = new Transform[lanesY];
                for (int y = 0; y < lanesY; y++) {
                    Transform newLane = new GameObject("Lane" + x + "" + y).transform;
                    newLane.position = originLane.position + new Vector3(laneGaps.x * x, laneGaps.y * y, 0);
                    newLane.transform.parent = originLane.transform;
                    lanes[x][y] = newLane;
                }
            }

            currentX = lanesX / 2;
            currentY = lanesY / 2;
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetButtonDown("Derecha")) {
                currentX = Mathf.Min(currentX + 1, lanesX - 1);
                animation.Play("right");
            } else if (Input.GetButtonDown("Izquierda")) {
                currentX = Mathf.Max(currentX - 1, 0);
                animation.Play("left");
            } else if (Input.GetButtonDown("Arriba")) {
                currentY = Mathf.Min(currentY + 1, lanesY - 1);
                animation.Play("up");
            } else if (Input.GetButtonDown("Abajo")) {
                currentY = Mathf.Max(currentY - 1, 0);
                animation.Play("down");
            }
        }

        private Vector3 CurrentPosition() {
            return lanes[currentX][currentY].position;
        }

        void OnDrawGizmosSelected() {
            if (lanes == null)
                return;

            Color color = Gizmos.color;
            Gizmos.color = Color.magenta;

            for (int x = 0; x < lanesX; x++) {
                for (int y = 0; y < lanesY; y++) {
                    Transform lane = lanes[x][y];
                    Gizmos.DrawRay(lane.position, lane.forward);

                    if (x == currentX && y == currentY)
                        Gizmos.DrawSphere(lane.position, .5f);
                    else
                        Gizmos.DrawWireSphere(lane.position, .5f);
                }
            }

            Gizmos.color = color;
        }
    }
}
