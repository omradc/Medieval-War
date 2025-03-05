using UnityEngine;

public class Test : MonoBehaviour
{
    public bool isFull;
    private void OnTriggerStay2D(Collider2D collision)
    {
        print("ok;");

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}




