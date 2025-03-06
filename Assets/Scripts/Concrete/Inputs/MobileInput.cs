using Assets.Scripts.Abstracts.Inputs;
using UnityEngine;

namespace Assets.Scripts.Concrete.Inputs
{
    internal class MobileInput : IInput
    {
        Touch touch;
        public bool GetButtonDown0()
        {
            if (Input.touchCount == 1)
            {
                touch = Input.GetTouch(0);
                return touch.phase == TouchPhase.Began;
            }
            else
                return false;
        }
        public bool GetButtonUp0()
        {
            if (Input.touchCount == 1)
            {
                touch = Input.GetTouch(0);
                return touch.phase == TouchPhase.Ended;
            }
            else
                return false;
        }
        public bool GetButton0()
        {
            if (Input.touchCount == 1)
            {
                touch = Input.GetTouch(0);
                return true;
            }
            else
                return false;
        }
        public bool Pinch(out Touch touch0, out Touch touch1)
        {
            if (Input.touchCount == 2)
            {
                touch0 = Input.GetTouch(0);
                touch1 = Input.GetTouch(1);
                return true;
            }
            touch0 = default;  // Dokunma olmadığı için varsayılan değer
            touch1 = default;
            return false;
        }
        public bool DragMove(out Touch touch0, int fingerNumber = 0)
        {
            if (Input.touchCount == fingerNumber)
            {
                touch0 = Input.GetTouch(0);
                return true;
            }
            touch0 = default;
            return false;
        }
    }
}
