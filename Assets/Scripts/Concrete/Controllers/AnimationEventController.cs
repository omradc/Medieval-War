using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class AnimationEventController:MonoBehaviour
    {
        public System.Action AttackEvent;
        void Attack()
        {
            AttackEvent?.Invoke();
        }


    }
}
