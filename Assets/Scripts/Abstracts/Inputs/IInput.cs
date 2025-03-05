using UnityEngine;

namespace Assets.Scripts.Abstracts.Inputs
{
    internal interface IInput
    {
        bool GetButtonDown0(int fingerNumber = 1);
        bool GetButtonUp0(int fingerNumber = 1);
        bool GetButton0(int fingerNumber = 1);
    }
}
