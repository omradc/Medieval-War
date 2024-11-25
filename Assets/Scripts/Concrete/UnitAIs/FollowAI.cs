using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Orders
{
    internal class FollowAI : UnitAI
    {

        bool followRange = true;
        float currentTime;

        public FollowAI(UnitController unitController, UnitPathFinding2D pF2D) : base(unitController, pF2D)
        {
        }

        public void FollowMode()
        {
            CatchNeraestTarget();
            StopWhenAttackDistance();
            SetRanges();
            ReturnAtYourPosition();
        }
        public void SetFollowUnit()
        {
            // Hedef boş ise, hedefi belirle, sadece 1 kez
            if (InteractManager.Instance.interactedUnit != null && uC.workOnce)
            {
                uC.followingObj = InteractManager.Instance.interactedUnit;
                uC.workOnce = false;
            }

        }
        void SetRanges()
        {
            if (followRange)
            {
                uC.currentSightRange = uC.sightRange + uC.currentAttackRange / 2;
                followRange = false;
            }

            // Hedef belirlendiyse menzilini ona ver ve takip et
            if (uC.followingObj != null)
                uC.sightRangePosition = uC.followingObj.transform.position;
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
                followRange = true;
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

                if (!followRange)
                    // menzilini eski haline getir
                    uC.currentSightRange = uC.sightRange;
            }
        }
    }
}
