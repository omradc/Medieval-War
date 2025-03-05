using Assets.Scripts.Abstracts.Inputs;
using UnityEngine;

namespace Assets.Scripts.Concrete.Inputs
{
    internal class PcInput : IInput
    {
        public bool GetButtonDown0(int fingerNumber = 1)
        {
            return Input.GetMouseButtonDown(0);
        }
        public bool GetButtonUp0 (int fingerNumber = 1)
        {
            return Input.GetMouseButtonUp(0); ;
        }
        public bool GetButton0 (int fingerNumber = 1)
        {
            return Input.GetMouseButton(0);
        }
    }
}
