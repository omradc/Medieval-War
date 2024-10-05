using Assets.Scripts.Abstracts.Inputs;
using UnityEngine;

namespace Assets.Scripts.Concrete.Inputs
{
    internal class PcInput : IInput
    {
        public bool GetButtonDown0 => Input.GetMouseButtonDown(0);
        public bool GetButtonUp0 => Input.GetMouseButtonUp(0);
        public bool GetButton0 => Input.GetMouseButton(0);
        public bool GetButtonDown1 => Input.GetMouseButtonDown(1);
        public bool GetButtonUp1 => Input.GetMouseButtonUp(1);
        public bool GetButton1 => Input.GetMouseButton(1);
        public bool GetKeyDownEscape => Input.GetKeyDown(KeyCode.Escape);
    }
}
