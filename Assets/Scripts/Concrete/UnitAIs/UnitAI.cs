using Assets.Scripts.Concrete.Controllers;
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
        protected void CatchNeraestTarget()
        {
            GameObject nearestTarget = DetechNearestTarget();
            if (nearestTarget == null) return;
            if (Vector2.Distance(nearestTarget.transform.position, kC.sightRangePosition) < kC.currentSightRange)
            {
                // hedef, saldırı menziline girerse; yakalamayı bırak
                if (Vector2.Distance(nearestTarget.transform.position, kC.attackRangePosition) < kC.currentAttackRange) return;

                AnimationManager.Instance.RunAnim(pF2D.animator, 1);

                pF2D.AIGetMoveCommand(nearestTarget.transform.position);
            }
        }
        protected void StopWhenAttackDistance() // Yapay zeka düşmanın tam koordinatlarına gider, fakat bu isPathEnd ile engellenir.
        {
            GameObject nearestTarget = DetechNearestTarget();
            if (kC.stayBuilding) // Kuleye gidiyorsa durmaz
            {
                pF2D.isPathEnd = false;
                return;
            }
            if (nearestTarget != null)
            {
                if (Vector2.Distance(nearestTarget.transform.position, kC.attackRangePosition) < kC.currentAttackRange)
                    pF2D.isPathEnd = true;

                if (Vector2.Distance(nearestTarget.transform.position, kC.attackRangePosition) > kC.currentAttackRange)
                    pF2D.isPathEnd = false;
            }

            //else
            //    pF2D.isPathEnd = false;

        } 
        public void RigidbodyControl(Rigidbody2D rb2D, bool stayBuilding)
        {
            if (stayBuilding)
            {
                rb2D.bodyType = RigidbodyType2D.Kinematic;
                return;
            }
            // Menzilde düşman yoksa ve kullanıcıdan emir almadıysa rigidbody aktif olur.
            if (DetechNearestTarget() != null && !pF2D.isUserPathFinding)
                rb2D.bodyType = RigidbodyType2D.Dynamic;
            else
                rb2D.bodyType = RigidbodyType2D.Kinematic;

        }
    }
}

