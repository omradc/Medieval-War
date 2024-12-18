using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Orders
{
    internal class AttackAI : UnitAI
    {
        public AttackAI(KnightController kC, UnitPathFinding2D pF2D) : base(kC, pF2D)
        {
        }
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
