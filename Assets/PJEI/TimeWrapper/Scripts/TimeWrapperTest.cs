using UnityEngine;
using System.Collections;
using PJEI;

public class TimeWrapperTest : MonoBehaviour {

    void Start() {
        StartCoroutine(this.RunTimeWarps());
    }

    [SerializeField]
    private Rect textRect = new Rect(50, 50, 300, 20);

    void OnGUI() {
        if (TimeWrapper.Paused)
            GUI.Label(this.textRect, "PAUSED");
        else
            GUI.Label(this.textRect, "Time scale: " + Time.timeScale + " Fixed delta time: " + Time.fixedDeltaTime);
    }

    private System.Collections.IEnumerator RunTimeWarps() {
        while (true) {
            int? factor = null;
            while (factor == null) {
                factor = GetNumberPressed();
                yield return null;
            }

            int? length = null;
            while (length == null) {
                length = GetNumberPressed();
                yield return null;
            }

            float actualFactor = (float)factor.Value * .1f;
            float actualLength = (float)length.Value / 2f;

            if (factor.Value == 0) {
                Debug.Log("Pausing for " + actualLength);
                TimeWrapper.Pause(actualLength);
            } else {
                Debug.Log("Warp to " + actualFactor + " for " + actualLength);
                TimeWrapper.WarpTimeForGameSeconds(actualFactor, actualLength);
            }
        }
    }

    private int? GetNumberPressed() {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            return 1;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            return 2;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            return 3;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            return 4;
        if (Input.GetKeyDown(KeyCode.Alpha5))
            return 5;
        if (Input.GetKeyDown(KeyCode.Alpha6))
            return 6;
        if (Input.GetKeyDown(KeyCode.Alpha7))
            return 7;
        if (Input.GetKeyDown(KeyCode.Alpha8))
            return 8;
        if (Input.GetKeyDown(KeyCode.Alpha9))
            return 9;
        if (Input.GetKeyDown(KeyCode.Alpha0))
            return 0;

        return null;
    }
}
