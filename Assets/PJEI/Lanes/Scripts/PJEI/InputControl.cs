using UnityEngine;

namespace PJEI.Lanes {
    public class InputControl : MonoBehaviour {

        [SerializeField]
        private string upAxis;
        [SerializeField]
        private bool upPositive = true;
        [SerializeField]
        private string downAxis;
        [SerializeField]
        private bool downPositive = true;
        [SerializeField]
        private string leftAxis;
        [SerializeField]
        private bool leftPositive = true;
        [SerializeField]
        private string rightAxis;
        [SerializeField]
        private bool rightPositive = true;

        public bool GetUpJustPressed() { return Input.GetButtonDown(upAxis); }
        public bool GetDownJustPressed() { return Input.GetButtonDown(downAxis); }
        public bool GetLeftJustPressed() { return Input.GetButtonDown(leftAxis); }
        public bool GetRightJustPressed() { return Input.GetButtonDown(rightAxis); }

        public bool GetUpJustReleased() { return Input.GetButtonUp(upAxis); }
        public bool GetDownJustReleased() { return Input.GetButtonUp(downAxis); }
        public bool GetLeftJustReleased() { return Input.GetButtonUp(leftAxis); }
        public bool GetRightJustReleased() { return Input.GetButtonUp(rightAxis); }

        public bool GetUp() { return Input.GetButton(upAxis); }
        public bool GetDown() { return Input.GetButton(downAxis); }
        public bool GetLeft() { return Input.GetButton(leftAxis); }
        public bool GetRight() { return Input.GetButton(rightAxis); }
    }
}
