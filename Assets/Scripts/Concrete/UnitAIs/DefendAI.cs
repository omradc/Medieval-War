using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Orders
{
    internal class DefendAI : UnitAI
    {

        bool work = true;
        public DefendAI(KnightController unitController, UnitPathFinding2D pF2D) : base(unitController, pF2D)
        {

        }

        public void DefendMode()
        {
            if (pF2D.lastMousePos == Vector2.zero) return;
            SetRanges();
            ReturnAtYourPosition(0.1f);
        }

        void SetRanges()
        {
            kC.sightRangePosition = pF2D.lastMousePos;
            if (work)
            {
                kC.currentSightRange = kC.sightRange + kC.currentAttackRange / 2;
                work = false;
            }
        }
        void ReturnAtYourPosition(float distance)
        {
            if (kC.followTargets.Length == 0)
            {
                pF2D.AIGetMoveCommand(kC.sightRangePosition);
                pF2D.direction.Turn2Direction(kC.sightRangePosition.x);
                StopCloseToSightRange();
            }

            if (kC.followTargets.Length > 0)
                work = true;
        }

        void StopCloseToSightRange()
        {
            // Görüş menzilinin merkezinde değilsen koşmaya devam et
            if (Vector2.Distance(kC.sightRangePosition, kC.transform.position) > 0.15f)
            {
                pF2D.isPathEnd = false;
                AnimationManager.Instance.RunAnim(pF2D.animator, 1);
            }
            // Görüş menzilinin merkezinde dur
            if (Vector2.Distance(kC.sightRangePosition, kC.transform.position) < 0.15f)
            {
                pF2D.isPathEnd = true;
                AnimationManager.Instance.IdleAnim(pF2D.animator);
                if (!work)
                    kC.currentSightRange = kC.sightRange;
            }
        }
    }
}
