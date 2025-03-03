using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class AnimationEventController:MonoBehaviour
    {
        public System.Action AttackEvent;
        public System.Action ResetAttackEvent;
        public System.Action ChopWoodEvent;
        public System.Action ChopSheepEvent;
        public System.Action GetHitSheepEvent;
        public System.Action GetHitTreeEvent;
        public System.Action IsTreeBeingCutAlreadyEvent;
        public System.Action BuildEvent;
        public System.Action BuildEndEvent;

        void ResetAttack()
        {
            ResetAttackEvent?.Invoke();
        }
        void Attack()
        {
            AttackEvent?.Invoke();
        }
        void ChopSheep()
        {
            ChopSheepEvent?.Invoke();
        }
        void GetHitSheep()
        {
            GetHitSheepEvent?.Invoke();
        }
        void ChopWood()
        {
            ChopWoodEvent?.Invoke();
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
        void BuildEnd()
        {
            BuildEndEvent?.Invoke();
        }

    }
}
