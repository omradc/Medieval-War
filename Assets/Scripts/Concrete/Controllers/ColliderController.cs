using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    public class ColliderController: MonoBehaviour
    {
        public GameObject colliderObj;

        public void ColliderStatus(bool colliderStatus)
        {
            if (colliderStatus)
                colliderObj.SetActive(true);
            else
                colliderObj.SetActive(false);

          
        }
    }
}