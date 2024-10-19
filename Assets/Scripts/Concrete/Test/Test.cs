using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class Test : MonoBehaviour
{
    public float speed;
    Vector3 dir;
    public Transform atrget;
    private void Start()
    {
        
    }

    private void Update()
    {
        dir = atrget.position - transform.position;
        transform.position += dir.normalized * speed * Time.deltaTime;
    }
}

