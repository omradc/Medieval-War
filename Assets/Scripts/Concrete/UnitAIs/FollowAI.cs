using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Orders
{
    internal class FollowAI : UnitAI
    {
        GameObject followingObj;
        bool work = true;
        float currentTime;

        public FollowAI(UnitController unitController, UnitPathFinding2D pF2D) : base(unitController, pF2D)
        {
        }

        public void FollowMode()
        {
            FollowUnit();
            CatchNeraestTarget();
            StopWhenAttackDistance();
            SetRanges();
            ReturnAtYourPosition();
        }
        void FollowUnit()
        {
            if (uC.unitOrderEnum == UnitOrderEnum.FollowOrder)
            {
                if (uC.workOnce && InteractManager.Instance.interactedUnit != null)
                {
                    followingObj = InteractManager.Instance.interactedUnit;
                    uC.workOnce = false;
                }
                if (!uC.workOnce)
                {
                    currentTime += Time.deltaTime;
                    if (currentTime > uC.followingTargetPerTime)
                    {
                        uC.sightRangePosition = followingObj.transform.position;
                        currentTime = 0;
                    }
                }

            }
        }
        void SetRanges()
        {
            if (work)
            {
                uC.currentSightRange = uC.sightRange + uC.currentAttackRange / 2;
                work = false;
            }
        }
        void ReturnAtYourPosition()
        {
            // Menzilinde düşman yoksa hedefi(kendi görüş menzilini) takip et
            if (uC.followTargets.Length == 0 && Vector2.Distance((Vector3)uC.sightRangePosition, uC.transform.position) > 1)
            {
                pF2D.AIGetMoveCommand(uC.sightRangePosition);
                pF2D.direction.Turn2Direction(uC.sightRangePosition.x);
                StopCloseToSightRange();
            }

            // Menzilde düşman varsa, menzilden çıkmayacak şekilde düşmanı takip et
            if (uC.followTargets.Length > 0 && Vector2.Distance((Vector3)uC.sightRangePosition, uC.transform.position) < uC.currentSightRange)
            {
                if (DetechNearestTarget() == null) return;
                pF2D.AIGetMoveCommand(DetechNearestTarget().transform.position);
                work = true;
            }
        }
        void StopCloseToSightRange()
        {
            // Devam et
            if (Vector2.Distance(uC.sightRangePosition, uC.attackRangePosition) > 3)
            {
                pF2D.isPathEnd = false;
                AnimationManager.Instance.RunAnim(pF2D.animator, 1);
            }

            // Dur
            if (Vector2.Distance(uC.sightRangePosition, uC.attackRangePosition) < 3)
            {
                pF2D.isPathEnd = true;
                AnimationManager.Instance.IdleAnim(pF2D.animator);

                if (!work)
                    // menzilini eski haline getir
                    uC.currentSightRange = uC.sightRange;
            }
        }
    }
}
