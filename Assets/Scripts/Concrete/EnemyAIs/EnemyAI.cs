using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Concrete.EnemyAIs
{
    internal class EnemyAI
    {
        EnemyController eC;
        EnemyPathFinding2D ePF2D;
        Vector3 targetPoint;
        float time;
        public EnemyAI(EnemyController enemyController, EnemyPathFinding2D enemyPathFinding2D)
        {
            eC = enemyController;
            ePF2D = enemyPathFinding2D;
        }
        public GameObject DetechNearestTarget()
        {
            if (eC.enemyTypeEnum == EnemyTypeEnum.Barrel)
            {

                // Varilin Önceliği yapılardır
                if (eC.playerBuildings.Length > 0)
                {
                    GameObject nearestTarget = null;
                    float shortestDistance = Mathf.Infinity;

                    for (int i = 0; i < eC.playerBuildings.Length; i++)
                    {
                        if (eC.playerBuildings[i] != null)
                        {
                            float distanceToEnemy = Vector2.Distance(eC.transform.position, eC.playerBuildings[i].transform.position);

                            if (shortestDistance > distanceToEnemy)
                            {
                                shortestDistance = distanceToEnemy;
                                nearestTarget = eC.playerBuildings[i].gameObject;
                            }

                        }
                    }

                    return nearestTarget;
                }

                // Yapı yoksa birimler
                if (eC.playerUnits.Length > 0)
                {
                    GameObject nearestTarget = null;
                    float shortestDistance = Mathf.Infinity;

                    for (int i = 0; i < eC.playerUnits.Length; i++)
                    {
                        if (eC.playerUnits[i] != null)
                        {
                            float distanceToEnemy = Vector2.Distance(eC.transform.position, eC.playerUnits[i].transform.position);

                            if (shortestDistance > distanceToEnemy)
                            {
                                shortestDistance = distanceToEnemy;
                                nearestTarget = eC.playerUnits[i].gameObject;
                            }

                        }
                    }

                    return nearestTarget;
                }
                else
                    return null;
            }
            if (eC.enemyTypeEnum != EnemyTypeEnum.Barrel)
            {
                // Yapı yoksa birimler
                if (eC.playerObjs.Length > 0)
                {
                    GameObject nearestTarget = null;
                    float shortestDistance = Mathf.Infinity;

                    for (int i = 0; i < eC.playerObjs.Length; i++)
                    {
                        if (eC.playerObjs[i] != null)
                        {
                            float distanceToEnemy = Vector2.Distance(eC.transform.position, eC.playerObjs[i].transform.position);

                            if (shortestDistance > distanceToEnemy)
                            {
                                shortestDistance = distanceToEnemy;
                                nearestTarget = eC.playerObjs[i].gameObject;
                            }

                        }
                    }

                    return nearestTarget;
                }
                else
                    return null;
            }
            else
                return null;
        }
        public void CatchNeraestTarget()
        {
            if (DetechNearestTarget() == null) return;
            if (Vector2.Distance(DetechNearestTarget().transform.position, eC.sightRangePosition) < eC.currentSightRange)
            {
                // hedef, saldırı menziline girerse; yakalamayı bırak
                if (Vector2.Distance(DetechNearestTarget().transform.position, eC.attackRangePosition) < eC.currentAttackRange) return;

                AnimationManager.Instance.RunAnim(ePF2D.animator, 1);

                // Hareket etmeden önce çarpıştırıcıyı kapalıdır
                eC.colliderController.ColliderStatus(false);

                ePF2D.AIGetMoveCommand(DetechNearestTarget().transform.position);

                // Hareket başladıktan sonra çarpıştırıcı açıktır
                eC.colliderController.ColliderStatus(true);
            }
        }
        public void StopWhenAttackDistance() // Yapay zeka düşmanın tam koordinatlarına gider, fakat bu isPathEnd ile engellenir.
        {
            if (DetechNearestTarget() != null)
            {
                if (Vector2.Distance(DetechNearestTarget().transform.position, eC.attackRangePosition) < eC.currentAttackRange)
                    ePF2D.isPathEnd = true;
                if (Vector2.Distance(DetechNearestTarget().transform.position, eC.attackRangePosition) > eC.currentAttackRange)
                    ePF2D.isPathEnd = false;
            }

        }
        public void CirclePatrolling()
        {
            if (DetechNearestTarget() != null) return;

            if (eC.patrolling)
            {
                eC.patrolling = false;
                targetPoint = Vector2.zero;
                targetPoint = new Vector3(Random.Range(-eC.patrollingDistance, eC.patrollingDistance), Random.Range(-eC.patrollingDistance, eC.patrollingDistance));
                ePF2D.AIGetMoveCommand(targetPoint);
                AnimationManager.Instance.RunAnim(ePF2D.animator, 1);
            }

            if (ePF2D.pathLeftToGo.Count == 0)
            {
                time++;

                if (time >= eC.waitingTime)
                {
                    time = 0;
                    eC.patrolling = true;
                }
            }
        }


        public void RandomPatrolling()
        {

        }
        public void TwoPointPatrolling()
        {

        }

    }
}
