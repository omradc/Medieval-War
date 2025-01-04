using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Orders
{
    internal class UnitAI
    {
        protected KnightController kC;
        protected PathFindingController pF;
        public GameObject nearestTarget;
        public Transform nearestAttackPoint;
        public UnitAI(KnightController kC, PathFindingController pF)
        {
            this.kC = kC;
            this.pF = pF;
        }

        GameObject DetechNearestTarget()
        {
            if (kC.followTargets.Length > 0)
            {
                GameObject nearestTarget = null;
                float shortestDistance = Mathf.Infinity;

                for (int i = 0; i < kC.followTargets.Length; i++)
                {
                    if (kC.followTargets[i] != null)
                    {
                        float distanceToEnemy = Vector2.Distance(kC.transform.position, kC.followTargets[i].transform.position);

                        if (shortestDistance > distanceToEnemy)
                        {
                            shortestDistance = distanceToEnemy;
                            nearestTarget = kC.followTargets[i].gameObject;
                        }

                    }
                }
                return nearestTarget;
            }
            else
                return null;

        }
        public void CatchNeraestTarget()
        {
            nearestTarget = DetechNearestTarget();

            if (nearestTarget == null) return;

            CalculateNearestAttackPoint();

            if (Vector2.Distance(nearestAttackPoint.position, kC.sightRangePosition) < kC.currentSightRange)
            {
                // hedef, saldırı menziline girerse; yakalamayı bırak
                if (Vector2.Distance(nearestAttackPoint.position, kC.attackRangePosition) < kC.attackRange) return;

                AnimationManager.Instance.RunAnim(kC.animator, 1);

                pF.MoveAI(nearestAttackPoint.position);
            }

        }
        void CalculateNearestAttackPoint()
        {
            // Düşman goblin ise
            if (nearestTarget.layer == 13)
            {
                nearestAttackPoint = nearestTarget.transform;
                return;
            }

            Transform obj = nearestTarget.transform.GetChild(4);
            // Saldırı noktası sayısı 1 ise
            if (obj.transform.childCount == 0)
            {
                nearestAttackPoint = obj;
                return;
            }

            // Saldırı noktası sayısı 1 den büyük ise
            float shortestDistance = Mathf.Infinity;
            for (int i = 0; i < obj.childCount; i++)
            {
                float distanceToTarget = Vector2.Distance(kC.transform.position, obj.GetChild(i).position);
                if (shortestDistance > distanceToTarget)
                {
                    shortestDistance = distanceToTarget;
                    nearestAttackPoint = obj.GetChild(i);
                }
            }

        }
        public void UnitBehaviours()
        {
            if (kC.unitOrderEnum == UnitOrderEnum.AttackOrder)
                Attack();
            if (kC.unitOrderEnum == UnitOrderEnum.DefendOrder)
                Defend();
            if (kC.unitOrderEnum == UnitOrderEnum.FollowOrder)
                Follow();
            if (kC.unitOrderEnum == UnitOrderEnum.StayOrder)
                Stay();
        }
        void Attack()
        {
            kC.sightRangePosition = kC.transform.GetChild(0).position;
            kC.currentSightRange = kC.sightRange;
        }
        void Defend()
        {
            kC.sightRangePosition = pF.lastMousePos; // Seçili pozisyon merkezdir
            kC.currentSightRange = kC.sightRange;
            if (kC.followTargets.Length == 0) // Düşman yoksa merkeze dön
            {
                pF.MoveAI(kC.sightRangePosition);
                kC.direction.Turn2DirectionWithPos(kC.sightRangePosition.x);
            }
        }
        void Follow()
        {
            kC.currentSightRange = kC.sightRange;
            // Hedef belirlendiyse menzilini ona ver ve takip et
            if (kC.followingObj != null)
                kC.sightRangePosition = kC.followingObj.transform.position;

            // Kendi saldırı menzilini geçmeyecek şekilde, görüş menzilinde düşman yoksa hedefi (kendi görüş menzilini) takip et
            if (kC.followTargets.Length == 0 && Vector2.Distance((Vector3)kC.sightRangePosition, kC.transform.position) > kC.attackRange)
            {
                pF.MoveAI(kC.sightRangePosition);
            }

            // Görüş menzilde düşman varsa, menzilden çıkmayacak şekilde düşmanı takip et
            if (kC.followTargets.Length > 0 && Vector2.Distance((Vector3)kC.sightRangePosition, kC.transform.position) < kC.currentSightRange)
            {
                if (nearestTarget == null) return;
                pF.MoveAI(nearestTarget.transform.position);
            }

        }
        void Stay()
        {
            kC.currentSightRange = kC.attackRange;
            kC.sightRangePosition = kC.transform.GetChild(0).position;
        }

        //public void RigidbodyControl(Rigidbody2D rb2D, bool stayBuilding)
        //{
        //    if (stayBuilding)
        //    {
        //        rb2D.bodyType = RigidbodyType2D.Kinematic;
        //        return;
        //    }
        //    // Menzilde düşman yoksa ve kullanıcıdan emir almadıysa rigidbody aktif olur.
        //    if (nearestTarget != null && !pF.isMoving)
        //        rb2D.bodyType = RigidbodyType2D.Dynamic;
        //    else
        //        rb2D.bodyType = RigidbodyType2D.Kinematic;

        //}
    }
}

