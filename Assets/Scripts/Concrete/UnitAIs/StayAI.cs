using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Movements;

namespace Assets.Scripts.Concrete.Orders
{
    internal class StayAI : UnitAI
    {
        public StayAI(UnitController unitController, UnitPathFinding2D pF2D) : base(unitController, pF2D)
        {
        }

        public void StaticMode()
        {
            SetRange();
        }

        void SetRange()
        {
            uC.currentSightRange = uC.currentAttackRange;
            uC.sightRangePosition = uC.transform.GetChild(0).position;

        }

    }
}
