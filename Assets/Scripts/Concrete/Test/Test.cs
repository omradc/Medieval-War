using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform target; // Hedef
    public float gravity = 9.81f; // Yerçekimi
    public float speed;
    Vector2 direction;
    public Quaternion rot;
    void Start()
    {
        direction = (target.position - transform.position);
        transform.rotation = rot;
        Destroy(gameObject, 2);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.Self);
    }
}




