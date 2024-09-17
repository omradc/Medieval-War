using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Orders
{
    internal class DefendOrder : Order
    {

        bool work = true;
        public DefendOrder(UnitController unitController, PathFinding2D pF2D) : base(unitController, pF2D)
        {

        }

        public void DefendMode()
        {
            if (pF2D.lastMousePos == Vector2.zero) return;
            CatchNeraestTarget();
            StopWhenAttackDistance();
            SetRanges();
            ReturnAtYourPosition(0.1f);
        }

        void SetRanges()
        {
            uC.sightRangePosition = pF2D.lastMousePos;
            if (work)
            {
                uC.currentSightRange = uC.sightRange + uC.currentAttackRange / 2;
                work = false;
            }
        }
        void ReturnAtYourPosition(float distance)
        {
            if (uC.followTargets.Length == 0)
            {
                pF2D.AIGetMoveCommand(uC.sightRangePosition);
                pF2D.direction.Turn2Direction(uC.sightRangePosition.x);
                StopCloseToSightRange();
            }

            if (uC.followTargets.Length > 0)
                work = true;
        }

        void StopCloseToSightRange()
        {
            // Görüş menzilinin merkezinde değilsen koşmaya devam et
            if (Vector2.Distance(uC.sightRangePosition, uC.transform.position) > 0.15f)
            {
                pF2D.isPathEnd = false;
                AnimationManager.Instance.RunAnim(pF2D.animator, 1);
            }
            // Görüş menzilinin merkezinde dur
            if (Vector2.Distance(uC.sightRangePosition, uC.transform.position) < 0.15f)
            {
                pF2D.isPathEnd = true;
                AnimationManager.Instance.IdleAnim(pF2D.animator);
                if (!work)
                    uC.currentSightRange = uC.sightRange;
            }
        }
    }
}
