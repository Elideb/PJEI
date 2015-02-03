using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Agent : MonoBehaviour {

    private NavMeshAgent agent = null;

    private int layerToPass;

    private GameObject origin;
    private GameObject destiny;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        layerToPass = LayerMask.NameToLayer("Igorne Raycase");
    }

    // Update is called once per frame
    void Update() {
        if (!Input.GetMouseButtonDown(0))
            return;

        NavMeshHit rayHit;
        bool touched = NavMesh.Raycast(Camera.main.transform.position, Camera.main.transform.forward * 1000, out rayHit, layerToPass);
        if (touched) {
            if (agent.SetDestination(rayHit.position)) {
                Debug.Log("Moving from " + transform.position + " to " + rayHit.position);
                if (origin == null)
                    origin = GameObject.CreatePrimitive(PrimitiveType.Cube);
                origin.transform.position = transform.position;

                if (destiny == null)
                    destiny = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                destiny.transform.position = agent.destination;
            } else {
                Debug.LogWarning("No path found");
            }
        } else {
            Debug.LogWarning("Touched out of navmesh");
        }
    }
}
