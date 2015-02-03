using UnityEngine;
using System.Collections;

public class SimpleMovement : MonoBehaviour {

    void Update() {
        if (Input.GetKey(KeyCode.DownArrow))
            transform.position += new Vector3(0, 0, -5 * Time.deltaTime);
        else if (Input.GetKey(KeyCode.UpArrow))
            transform.position += new Vector3(0, 0, 5 * Time.deltaTime);
        else if (Input.GetKey(KeyCode.LeftArrow))
            transform.position += new Vector3(-5 * Time.deltaTime, 0, 0);
        else if (Input.GetKey(KeyCode.RightArrow))
            transform.position += new Vector3(5 * Time.deltaTime, 0, 0);
        else if (Input.GetKey(KeyCode.LeftControl))
            transform.position += new Vector3(0, -5 * Time.deltaTime, 0);
        else if (Input.GetKey(KeyCode.LeftShift))
            transform.position += new Vector3(0, 5 * Time.deltaTime, 0);
    }
}
