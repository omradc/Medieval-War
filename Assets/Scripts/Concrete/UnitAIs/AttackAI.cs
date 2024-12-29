using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Movements;

namespace Assets.Scripts.Concrete.Orders
{
    internal class AttackAI : UnitAI
    {
        public AttackAI(KnightController kC, PathFindingController pF) : base(kC, pF) { }

        public void AttackMode()
        {
            SetRange();
        }

        void SetRange()
        {
            kC.currentSightRange = kC.sightRange;
            kC.sightRangePosition = kC.transform.GetChild(0).position;
        }

    }
}
