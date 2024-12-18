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

        public FollowAI(KnightController kC, UnitPathFinding2D pF2D) : base(kC, pF2D)
        {
        }

        public void FollowMode()
        {
            SetRanges();
            ReturnAtYourPosition();
        }
        public void SetFollowUnit()
        {
            // Hedef boş ise, hedefi belirle, sadece 1 kez
            if (InteractManager.Instance.interactedUnit != null && kC.workOnce)
            {
                kC.followingObj = InteractManager.Instance.interactedUnit;
                kC.workOnce = false;
            }

        }
        void SetRanges()
        {
            if (followRange)
            {
                kC.currentSightRange = kC.sightRange + kC.currentAttackRange / 2;
                followRange = false;
            }

            // Hedef belirlendiyse menzilini ona ver ve takip et
            if (kC.followingObj != null)
                kC.sightRangePosition = kC.followingObj.transform.position;
        }
        void ReturnAtYourPosition()
        {
            // Menzilinde düşman yoksa hedefi(kendi görüş menzilini) takip et
            if (kC.followTargets.Length == 0 && Vector2.Distance((Vector3)kC.sightRangePosition, kC.transform.position) > 1)
            {
                pF2D.AIGetMoveCommand(kC.sightRangePosition);
                pF2D.direction.Turn2Direction(kC.sightRangePosition.x);
                StopCloseToSightRange();
            }

            // Menzilde düşman varsa, menzilden çıkmayacak şekilde düşmanı takip et
            if (kC.followTargets.Length > 0 && Vector2.Distance((Vector3)kC.sightRangePosition, kC.transform.position) < kC.currentSightRange)
            {
                if (nearestTarget== null) return;
                pF2D.AIGetMoveCommand(nearestTarget.transform.position);
                followRange = true;
            }
        }
        void StopCloseToSightRange()
        {
            // Devam et
            if (Vector2.Distance(kC.sightRangePosition, kC.attackRangePosition) > 3)
            {
                pF2D.isPathEnd = false;
                AnimationManager.Instance.RunAnim(pF2D.animator, 1);
            }

            // Dur
            if (Vector2.Distance(kC.sightRangePosition, kC.attackRangePosition) < 3)
            {
                pF2D.isPathEnd = true;
                AnimationManager.Instance.IdleAnim(pF2D.animator);

                if (!followRange)
                    // menzilini eski haline getir
                    kC.currentSightRange = kC.sightRange;
            }
        }
    }
}
