using UnityEngine;
using System.Collections;

namespace PJEI.Lanes {
    public class LaneCreator : MonoBehaviour {

        /// <summary>
        /// Position used as reference to create the others from.
        /// </summary>
        public Transform originLane;

        /// <summary>
        /// Distance between lanes.
        /// </summary>
        public Vector2 laneGaps = new Vector2(1, 1.94f);

        /// <summary>
        /// Number of lanes per row.
        /// </summary>
        public int lanesX = 3;

        /// <summary>
        /// Number of lanes per column.
        /// </summary>
        public int lanesY = 2;

        /// <summary>
        /// Lanes generated.
        /// </summary>
        private Transform[][] lanes = null;

        /// <summary>
        /// Current column.
        /// </summary>
        private int currentX = 0;

        /// <summary>
        /// Current row.
        /// </summary>
        private int currentY = 0;

        void Start() {
            // Create the new lanes, adding to the origin position
            lanes = new Transform[lanesX][];
            for (int x = 0; x < lanesX; x++) {
                lanes[x] = new Transform[lanesY];
                for (int y = 0; y < lanesY; y++) {
                    // Create a new object to use as a lane
                    Transform newLane = new GameObject("Lane" + x + "" + y).transform;
                    newLane.position = originLane.position + new Vector3(laneGaps.x * x, laneGaps.y * y, 0);

                    // Making them the origin's children keeps the scene much tidier
                    newLane.transform.parent = originLane.transform;

                    // Add the new lane to the matrix
                    lanes[x][y] = newLane;
                }
            }

            currentX = lanesX / 2;
            currentY = lanesY / 2;
        }

        // Update is called once per frame
        void Update() {
            // Move around the matrix, but never going out of its bounds
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

        /// <summary>
        /// Visual aid to debug and verify the lanes are where we want them.
        /// The current one is painted solid, instead of in wireframe.
        /// </summary>
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
