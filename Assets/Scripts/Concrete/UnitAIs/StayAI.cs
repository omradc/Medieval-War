using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Movements;

namespace Assets.Scripts.Concrete.Orders
{
    internal class StayAI : UnitAI
    {
        public StayAI(KnightController kC, PathFindingController pF) : base(kC, pF) { }

        public void StaticMode()
        {
            SetRange();
        }

        void SetRange()
        {
            kC.currentSightRange = kC.attackRange;
            kC.sightRangePosition = kC.transform.GetChild(0).position;

        }

    }
}
