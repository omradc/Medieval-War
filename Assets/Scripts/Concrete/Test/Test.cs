using UnityEngine;

public class ChildDontMoveWithParent : MonoBehaviour
{
    public Transform child; // �ocuk objesi
    public Transform parent; // Ana obje (Ebeveyn)

    private Vector3 initialChildPosition; // �ocu�un ilk pozisyonu

    void Start()
    {
        // �ocu�un d�nya pozisyonunu kaydet
        initialChildPosition = child.position;
    }

    void Update()
    {
        // Ebeveynin hareketinden ba��ms�z olarak, �ocu�un pozisyonunu sabit tut
        child.position = initialChildPosition;
    }
}




