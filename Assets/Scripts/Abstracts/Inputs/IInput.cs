using UnityEngine;

namespace Assets.Scripts.Abstracts.Inputs
{
    internal interface IInput
    {
        /// <summary>
        /// Dokunma başladı
        /// </summary>
        bool GetButtonDown0();
        /// <summary>
        /// Dokunma bitti
        /// </summary>
        bool GetButtonUp0();
        /// <summary>
        /// Dokunma devam ediyor
        /// </summary>
        bool GetButton0();

    }
}
