using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("�asfgwepy�ufgw�egfwe");
        }
        if (Input.GetMouseButton(1))
        {
            print("�asfgwepy�ufgw�egfwe");

        }

    }
}
