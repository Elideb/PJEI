using UnityEngine;

public class Rotate : MonoBehaviour {

    [SerializeField]
    private Vector3 rotation = new Vector3(15f, 15f, 15f);

    // Update is called once per frame
    void Update() {
        this.transform.Rotate(this.rotation * Time.deltaTime);
    }
}
