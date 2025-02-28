using Assets.Scripts.Concrete.Movements;
using UnityEditor.VersionControl;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Update()
    {
        // Raycast ���n� g�nder. Y�n, oyuncunun bakt��� y�nde (transform.right), mesafe ise belirlenen uzunlukta.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1, LayerMask.GetMask("Elevations"));

        // �arpan bir nesne varsa
        if (hit.collider != null)
        {
            // Nesne ile etkile�im ba�lat
            Debug.Log("Etkile�im ba�lat�ld�!");

        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * 1);
    }
}




