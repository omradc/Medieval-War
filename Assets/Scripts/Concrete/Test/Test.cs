using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform a; // �ocuk objesi
    public Transform b; // Ana obje (Ebeveyn)
    public float result;
    void Update()
    {
        result = (a.position - b.position).magnitude;
    }
}




