using Assets.Scripts.Abstracts.Inputs;
using UnityEngine;

namespace Assets.Scripts.Concrete.Inputs
{
    internal class PcInput : IInput
    {
        public bool GetButtonDown0()
        {
            return Input.GetMouseButtonDown(0);
        }
        public bool GetButtonUp0 ()
        {
            return Input.GetMouseButtonUp(0); ;
        }
        public bool GetButton0 ()
        {
            return Input.GetMouseButton(0);
        }

        public float Scroll()
        {
            return Input.GetAxis("Mouse ScrollWheel");
        }
    }
}
