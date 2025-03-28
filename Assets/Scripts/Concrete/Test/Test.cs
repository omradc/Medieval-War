using Assets.Scripts.Concrete.Managers;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        AnimationManager.Instance.RunAnim(GetComponent<Animator>());
    }
}




