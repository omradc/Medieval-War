using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Orders
{
    internal class DefendAI : UnitAI
    {

        bool work = true;
        public DefendAI(KnightController unitController, PathFindingController pF) : base(unitController, pF) { }


        public void DefendMode()
        {
            if (pF.lastMousePos == Vector2.zero) return;
            SetRanges();
            ReturnAtYourPosition();
        }

        void SetRanges()
        {
            kC.sightRangePosition = pF.lastMousePos;
            if (work)
            {
                kC.currentSightRange = kC.sightRange + kC.currentAttackRange / 2;
                work = false;
            }
        }
        void ReturnAtYourPosition()
        {
            if (kC.followTargets.Length == 0)
            {
                pF.MoveAI(kC.sightRangePosition);
                kC.direction.Turn2DirectionWithPos(kC.sightRangePosition.x);
            }

            if (kC.followTargets.Length > 0)
                work = true;
        }
    }
}
