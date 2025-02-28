using Assets.Scripts.Concrete.Movements;
using UnityEditor.VersionControl;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Update()
    {
        // Raycast ýþýný gönder. Yön, oyuncunun baktýðý yönde (transform.right), mesafe ise belirlenen uzunlukta.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1, LayerMask.GetMask("Elevations"));

        // Çarpan bir nesne varsa
        if (hit.collider != null)
        {
            // Nesne ile etkileþim baþlat
            Debug.Log("Etkileþim baþlatýldý!");

        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * 1);
    }
}




