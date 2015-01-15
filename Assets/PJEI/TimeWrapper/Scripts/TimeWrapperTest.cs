using PJEI;
using UnityEngine;

public class TimeWrapperTest : MonoBehaviour {

    [SerializeField]
    private Rect instructionsRect = new Rect(50, 100, 500, 100);

    [SerializeField]
    private Rect textRect = new Rect(50, 50, 300, 20);

    void OnGUI() {
        GUI.Label(this.instructionsRect, "Press P to pause the game for one second.\nPress S to slow down by half for one game second.\nPress F to fast forward the game by double for one game second");

        if (TimeWrapper.Paused)
            GUI.Label(this.textRect, "PAUSED");
        else
            GUI.Label(this.textRect, "Time scale: " + Time.timeScale + " Fixed delta time: " + Time.fixedDeltaTime);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P))
            TimeWrapper.Pause(1f);

        if (Input.GetKeyDown(KeyCode.S))
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                TimeWrapper.OverrideTimeForRealSeconds(.5f, 1f);
            else
                TimeWrapper.WarpTimeForGameSeconds(.5f, 1f);

        if (Input.GetKeyDown(KeyCode.F))
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                TimeWrapper.OverrideTimeForRealSeconds(2f, 1f);
            else
                TimeWrapper.WarpTimeForGameSeconds(2f, 1f);
    }

}
