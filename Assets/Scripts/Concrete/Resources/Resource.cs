using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    sealed class Resource : MonoBehaviour
    {
        [SerializeField] float destroyTime = 3;
        private void Start()
        {
            Destroy(gameObject, destroyTime);
        }
    }
}
