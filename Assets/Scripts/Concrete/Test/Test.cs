using UnityEngine;

public class Test : MonoBehaviour
{
    Touch touch0;
    private void Update()
    {
        if (Input.touchCount == 1)
        {
            touch0 = Input.GetTouch(0);
            print(1);
            if (touch0.phase == TouchPhase.Began)
                print("Began");
            if (touch0.phase == TouchPhase.Stationary)
                print("Stationary");
            if (touch0.phase == TouchPhase.Moved)
                print("Moved");
            if (touch0.phase == TouchPhase.Ended)
                print("Ended");
            if (touch0.phase == TouchPhase.Canceled)
                print("Canceled");
        }
    }
}




