using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Orders
{
    internal class AttackOrder : Order
    {
        public AttackOrder(UnitController uC, PathFinding2D pF2D) : base(uC, pF2D)
        {
        }
        public void AttackMode()
        {
            CatchNeraestTarget();
            StopWhenAttackDistance();
            SetRange();
        }

        void SetRange()
        {
            uC.currentSightRange = uC.sightRange;
            uC.sightRangePosition = uC.transform.GetChild(0).position;
        }

    }
}
