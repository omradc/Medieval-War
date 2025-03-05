using Assets.Scripts.Abstracts.Inputs;
using UnityEngine;

namespace Assets.Scripts.Concrete.Inputs
{
    internal class MobileInput : IInput
    {
        Touch touch;
        /// <summary>
        /// Dokunma başladı
        /// </summary>
        public bool GetButtonDown0(int fingerNumber = 1)
        {
            if (Input.touchCount == 1)
            {
                touch = Input.GetTouch(0);
                return touch.phase == TouchPhase.Began;
            }
            else
                return false;
        }
        /// <summary>
        /// Dokunma bitti
        /// </summary>
        public bool GetButtonUp0(int fingerNumber = 1)
        {
            if (Input.touchCount == 1)
            {
                touch = Input.GetTouch(0);
                return touch.phase == TouchPhase.Ended;
            }
            else
                return false;
        }
        /// <summary>
        /// Dokunma devam ediyor
        /// </summary>
        public bool GetButton0(int fingerNumber = 1)
        {
            if (Input.touchCount == 1)
            {
                touch = Input.GetTouch(0);
                return Input.touchCount > 0;
            }
            else
                return false;
        }
    }
}
