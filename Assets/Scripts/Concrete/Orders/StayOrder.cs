using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Movements;

namespace Assets.Scripts.Concrete.Orders
{
    internal class StayOrder : Order
    {
        public StayOrder(UnitController unitController, PathFinding2D pF2D) : base(unitController, pF2D)
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
