using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Orders
{
    internal class FollowAI : UnitAI
    {
        public FollowAI(KnightController kC, PathFindingController pF) : base(kC, pF) { }

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
            // Hedef belirlendiyse menzilini ona ver ve takip et
            if (kC.followingObj != null)
                kC.sightRangePosition = kC.followingObj.transform.position;
        }
        void ReturnAtYourPosition()
        {
            // Menzilinde düşman yoksa hedefi(kendi görüş menzilini) takip et
            if (kC.followTargets.Length == 0 && Vector2.Distance((Vector3)kC.sightRangePosition, kC.transform.position) > 2)
            {
                pF.MoveAI(kC.sightRangePosition);
            }

            // Menzilde düşman varsa, menzilden çıkmayacak şekilde düşmanı takip et
            if (kC.followTargets.Length > 0 && Vector2.Distance((Vector3)kC.sightRangePosition, kC.transform.position) < kC.currentSightRange)
            {
                if (nearestTarget== null) return;
                pF.MoveAI(nearestTarget.transform.position);
            }
        }
    }
}
