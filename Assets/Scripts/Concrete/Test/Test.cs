using Assets.Scripts.Concrete.Movements;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Update()
    {

        if (transform.hasChanged)
        {
            Debug.Log("Nesne hareket etti veya de�i�ti!");
            transform.hasChanged = false; // De�i�iklik alg�land�ktan sonra s�f�rla
        }
    }
}




