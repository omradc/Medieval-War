using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Orders
{
    internal class AttackAI : UnitAI
    {
        public AttackAI(UnitController uC, UnitPathFinding2D pF2D) : base(uC, pF2D)
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
