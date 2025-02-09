using Assets.Scripts.Concrete.Movements;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Update()
    {

        if (transform.hasChanged)
        {
            Debug.Log("Nesne hareket etti veya deðiþti!");
            transform.hasChanged = false; // Deðiþiklik algýlandýktan sonra sýfýrla
        }
    }
}




