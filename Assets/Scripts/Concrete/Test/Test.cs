using UnityEngine;

public class ChildDontMoveWithParent : MonoBehaviour
{
    public Transform child; // Çocuk objesi
    public Transform parent; // Ana obje (Ebeveyn)

    private Vector3 initialChildPosition; // Çocuðun ilk pozisyonu

    void Start()
    {
        // Çocuðun dünya pozisyonunu kaydet
        initialChildPosition = child.position;
    }

    void Update()
    {
        // Ebeveynin hareketinden baðýmsýz olarak, çocuðun pozisyonunu sabit tut
        child.position = initialChildPosition;
    }
}




