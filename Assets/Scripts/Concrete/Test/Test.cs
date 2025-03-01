using UnityEngine;

public class Test : MonoBehaviour
{
    private void Update()
    {
        // Raycast ýþýný gönder. Yön, oyuncunun baktýðý yönde (transform.right), mesafe ise belirlenen uzunlukta.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 0.1f);

        // Çarpan bir nesne varsa
        if (hit.collider != null)
        {
            // Nesne ile etkileþim baþlat
            Debug.Log(hit.collider.name);

        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.up * .1f);
    }
}




