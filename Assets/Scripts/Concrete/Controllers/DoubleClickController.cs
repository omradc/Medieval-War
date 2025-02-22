using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Concrete.Controllers
{
    public class DoubleClickController : MonoBehaviour
    {
        float lastClickTime = 0f; // Son tıklama zamanı
        float doubleClickThreshold = 0.3f; // Çift tıklama süresi

        void Update()
        {
            if (Input.GetMouseButtonDown(0)) // Sol tıklama
            {
                if (Time.time - lastClickTime <= doubleClickThreshold)
                {
                    Debug.Log("Çift tıklama algılandı!");
                }

                lastClickTime = Time.time; // Tıklama zamanını güncelle
            }
        }
    }
}