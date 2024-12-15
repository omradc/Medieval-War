using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class AnimationEventController:MonoBehaviour
    {
        public System.Action AttackEvent;
        public System.Action ChopEvent;
        public System.Action CutSheepEvent;
        public System.Action GetHitTreeEvent;
        public System.Action IsTreeBeingCutAlreadyEvent;
        public System.Action BuildEvent;

        void Attack()
        {
            AttackEvent?.Invoke();
        }
        void Chop()
        {
            ChopEvent?.Invoke();
        }
        void CutSheep()
        {
            CutSheepEvent?.Invoke();
        }
        void GetHitTree()
        {
            GetHitTreeEvent?.Invoke();
        }

        void IsTreeBeingCutAlready()
        {
            IsTreeBeingCutAlreadyEvent?.Invoke();
        }

        void Build()
        {
            BuildEvent?.Invoke();
        }

    }
}
