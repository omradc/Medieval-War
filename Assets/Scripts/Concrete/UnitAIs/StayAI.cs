using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Movements;

namespace Assets.Scripts.Concrete.Orders
{
    internal class StayAI : UnitAI
    {
        public StayAI(KnightController kC, UnitPathFinding2D pF2D) : base(kC, pF2D)
        {
        }

        public void StaticMode()
        {
            SetRange();
        }

        void SetRange()
        {
            kC.currentSightRange = kC.currentAttackRange;
            kC.sightRangePosition = kC.transform.GetChild(0).position;

        }

    }
}
