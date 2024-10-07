using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Orders
{
    internal class UnitAI
    {
        protected UnitController uC;
        protected UnitPathFinding2D pF2D;
        public UnitAI(UnitController unitController, UnitPathFinding2D pF2D)
        {
            uC = unitController;
            this.pF2D = pF2D;
        }

        public GameObject DetechNearestTarget()
        {
            if (uC.followTargets.Length > 0)
            {
                GameObject nearestTarget = null;
                float shortestDistance = Mathf.Infinity;

                for (int i = 0; i < uC.followTargets.Length; i++)
                {
                    if (uC.followTargets[i] != null)
                    {
                        float distanceToEnemy = Vector2.Distance(uC.transform.position, uC.followTargets[i].transform.position);

                        if (shortestDistance > distanceToEnemy)
                        {
                            shortestDistance = distanceToEnemy;
                            nearestTarget = uC.followTargets[i].gameObject;
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
            if (DetechNearestTarget() == null) return;

            if (Vector2.Distance(DetechNearestTarget().transform.position, uC.sightRangePosition) < uC.currentSightRange)
            {
                // hedef, saldırı menziline girerse; yakalamayı bırak
                if (Vector2.Distance(DetechNearestTarget().transform.position, uC.attackRangePosition) < uC.currentAttackRange) return;
                AnimationManager.Instance.RunAnim(pF2D.animator, 1);
                // 1 kez çalışır
                pF2D.AIGetMoveCommand(DetechNearestTarget().transform.position);
            }
        }

        // Yapay zeka düşmanın tam koordinatlarına gider, fakat bu isPathEnd ile engellenir.
        protected void StopWhenAttackDistance()
        {
            if (DetechNearestTarget() != null)
            {
                if (Vector2.Distance(DetechNearestTarget().transform.position, uC.attackRangePosition) < uC.currentAttackRange)
                    pF2D.isPathEnd = true;
                if (Vector2.Distance(DetechNearestTarget().transform.position, uC.attackRangePosition) > uC.currentAttackRange)
                    pF2D.isPathEnd = false;
            }

        }
    }
}

