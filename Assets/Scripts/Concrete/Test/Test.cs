using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Inputs;
using UnityEngine;

public class Test : MonoBehaviour
{
    IInput input;
    private void Awake()
    {
        input = new PcInput();
    }
    private void Update()
    {
        //if (Input.touchCount == 1)
        //{
        //    touch0 = Input.GetTouch(0);
        //    print(1);
        //    if (touch0.phase == TouchPhase.Began)
        //        print("Began");
        //    if (touch0.phase == TouchPhase.Stationary)
        //        print("Stationary");
        //    if (touch0.phase == TouchPhase.Moved)
        //        print("Moved");
        //    if (touch0.phase == TouchPhase.Ended)
        //        print("Ended");
        //}

        if (input.GetButtonDown0())
            print("Began");
        if (input.GetButtonUp0())
            print("Ended");
        if (input.GetButton0())
            print("Countinue");

    }
}




