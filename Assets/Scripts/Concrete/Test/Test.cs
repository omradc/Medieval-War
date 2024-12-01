using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class Test : MonoBehaviour
{
    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}

