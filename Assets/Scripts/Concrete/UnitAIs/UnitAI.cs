﻿using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Orders
{
    internal class UnitAI
    {
        protected KnightController kC;
        protected UnitPathFinding2D pF2D;
        public GameObject nearestTarget;
        public Transform nearestAttackPoint;
        public UnitAI(KnightController kC, UnitPathFinding2D pF2D)
        {
            this.kC = kC;
            this.pF2D = pF2D;
        }

        public GameObject DetechNearestTarget()
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
            StopWhenAttackDistance();

            if (Vector2.Distance(nearestAttackPoint.position, kC.sightRangePosition) < kC.currentSightRange)
            {
                // hedef, saldırı menziline girerse; yakalamayı bırak
                if (Vector2.Distance(nearestAttackPoint.position, kC.attackRangePosition) < kC.currentAttackRange) return;

                AnimationManager.Instance.RunAnim(pF2D.animator, 1);

                pF2D.AIGetMoveCommand(nearestAttackPoint.position);
            }

        }
        protected void StopWhenAttackDistance() // Yapay zeka düşmanın tam koordinatlarına gider, fakat bu isPathEnd ile engellenir.
        {

                if (Vector2.Distance(nearestAttackPoint.position, kC.attackRangePosition) < kC.currentAttackRange)
                    pF2D.isPathEnd = true;

                if (Vector2.Distance(nearestAttackPoint.position, kC.attackRangePosition) >= kC.currentAttackRange)
                    pF2D.isPathEnd = false;




        }
        public void RigidbodyControl(Rigidbody2D rb2D, bool stayBuilding)
        {
            if (stayBuilding)
            {
                rb2D.bodyType = RigidbodyType2D.Kinematic;
                return;
            }
            // Menzilde düşman yoksa ve kullanıcıdan emir almadıysa rigidbody aktif olur.
            if (nearestTarget != null && !pF2D.isUserPathFinding)
                rb2D.bodyType = RigidbodyType2D.Dynamic;
            else
                rb2D.bodyType = RigidbodyType2D.Kinematic;

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
    }
}

