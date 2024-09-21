using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class AnimationEventController:MonoBehaviour
    {
        public System.Action AttackEvent;
        public System.Action ChopEvent;
        public System.Action GetHitTreeEvent;
        public System.Action IsTreeBeingCutAlreadyEvent;

        void Attack()
        {
            AttackEvent?.Invoke();
        }
        void Chop()
        {
            ChopEvent?.Invoke();
        }

        void GetHitTree()
        {
            GetHitTreeEvent?.Invoke();
        }

        void IsTreeBeingCutAlready()
        {
            IsTreeBeingCutAlreadyEvent?.Invoke();
        }

    }
}
