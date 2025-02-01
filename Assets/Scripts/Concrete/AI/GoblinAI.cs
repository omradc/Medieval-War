using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.AI
{
    internal class GoblinAI
    {
        readonly GoblinController gC;
        readonly PathFindingController pF;
        BuildingController bC;
        GameObject nearestWoodTower;
        readonly SpriteRenderer goblinSpriteRenderer;
        public Transform nearestAttackPoint;
        public GameObject nearestTarget;
        Vector3 targetPoint;
        Vector2 gatePos;
        Vector2 towerPos;
        readonly Vector3 firstPoint;
        bool patrol;
        int index;
        float time;
        public GoblinAI(GoblinController goblinController, PathFindingController pF)
        {
            gC = goblinController;
            this.pF = pF;
            firstPoint = gC.transform.position;
            targetPoint = firstPoint;
            goblinSpriteRenderer = gC.transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        public GameObject DetechNearestTarget()
        {
            if (gC.troopType == TroopTypeEnum.Barrel)
            {

                // Varilin Önceliği yapılardır
                if (gC.playerBuildings.Length > 0)
                {
                    GameObject nearestTarget = null;
                    float shortestDistance = Mathf.Infinity;

                    for (int i = 0; i < gC.playerBuildings.Length; i++)
                    {
                        if (gC.playerBuildings[i] != null)
                        {
                            float distanceToEnemy = Vector2.Distance(gC.transform.position, gC.playerBuildings[i].transform.position);

                            if (shortestDistance > distanceToEnemy)
                            {
                                shortestDistance = distanceToEnemy;
                                nearestTarget = gC.playerBuildings[i].gameObject;
                            }

                        }
                    }

                    return nearestTarget;
                }

                // Yapı yoksa birimler
                if (gC.playerUnits.Length > 0)
                {
                    GameObject nearestTarget = null;
                    float shortestDistance = Mathf.Infinity;

                    for (int i = 0; i < gC.playerUnits.Length; i++)
                    {
                        if (gC.playerUnits[i] != null)
                        {
                            float distanceToEnemy = Vector2.Distance(gC.transform.position, gC.playerUnits[i].transform.position);

                            if (shortestDistance > distanceToEnemy)
                            {
                                shortestDistance = distanceToEnemy;
                                nearestTarget = gC.playerUnits[i].gameObject;
                            }

                        }
                    }

                    return nearestTarget;
                }
                else
                    return null;
            }
            if (gC.troopType != TroopTypeEnum.Barrel)
            {
                // Yapı yoksa birimler
                if (gC.enemies.Length > 0)
                {
                    GameObject nearestTarget = null;
                    float shortestDistance = Mathf.Infinity;

                    for (int i = 0; i < gC.enemies.Length; i++)
                    {
                        if (gC.enemies[i] != null)
                        {
                            float distanceToEnemy = Vector2.Distance(gC.transform.position, gC.enemies[i].transform.position);

                            if (shortestDistance > distanceToEnemy)
                            {
                                shortestDistance = distanceToEnemy;
                                nearestTarget = gC.enemies[i].gameObject;
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
            nearestTarget = DetechNearestTarget();
            if (nearestTarget == null) return;

            CalculateNearestAttackPoint();

            if (Vector2.Distance(nearestAttackPoint.position, gC.sightRangePosition) < gC.currentSightRange)
            {
                // hedef, saldırı menziline girerse; yakalamayı bırak
                if (Vector2.Distance(nearestAttackPoint.position, gC.attackRangePosition) < gC.attackRange) return;

                pF.MoveAI(nearestAttackPoint.position, gC.attackRange);

            }
        }
        public void GoblinBehaviour()
        {
            AttackTheAllUnit();
            if (gC.onBuilding) return;
            CirclePatrollingAnchor();
            CirclePatrollingFree();
            PointPatrolling();
        }
        void CirclePatrollingAnchor()
        {
            if (gC.behavior != BehaviorEnum.CirclePatrollingAnchor) return;
            if (nearestTarget != null) return;
            if (patrol)
            {
                patrol = false;
                targetPoint = firstPoint;
                targetPoint += new Vector3(Random.Range(-gC.patrollingRadius, gC.patrollingRadius), Random.Range(-gC.patrollingRadius, gC.patrollingRadius));
                pF.MoveAI(targetPoint, 0);
            }

            if (pF.isStoping)
            {
                time++;

                if (time >= gC.waitingTime)
                {
                    time = 0;
                    patrol = true;
                }
            }
        }
        void CirclePatrollingFree()
        {
            if (gC.behavior != BehaviorEnum.CirclePatrollingFree) return;
            if (nearestTarget != null) return;

            if (patrol)
            {
                patrol = false;
                targetPoint += new Vector3(Random.Range(-gC.patrollingRadius, gC.patrollingRadius), Random.Range(-gC.patrollingRadius, gC.patrollingRadius));
                pF.MoveAI(targetPoint, 0);
            }

            if (pF.isStoping)
            {
                time++;

                if (time >= gC.waitingTime)
                {
                    time = 0;
                    patrol = true;
                }
            }
        }
        void PointPatrolling()
        {
            if (gC.behavior != BehaviorEnum.PointPatrolling) return;
            if (nearestTarget != null) return;

            // Devriye gez
            if (patrol)
            {
                targetPoint = gC.patrolPoints[index].position;
                pF.MoveAI(targetPoint, 0);
                index++;
                if (index == gC.patrolPoints.Length)
                    index = 0;
                patrol = false;
            }

            // Devriye naktasına ulaşıldı
            if (pF.isStoping)
                patrol = true;
        }
        void AttackTheAllUnit()
        {
            if (gC.behavior != BehaviorEnum.FindNearestPlayerUnit) return;
            gC.currentSightRange = 100;
            gC.attackTheAllKnights = true;
        }
        void CalculateNearestAttackPoint()
        {
            // Düşman şovalye ise
            if (nearestTarget.layer == 6)
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
                float distanceToTarget = Vector2.Distance(gC.transform.position, obj.GetChild(i).position);
                if (shortestDistance > distanceToTarget)
                {
                    shortestDistance = distanceToTarget;
                    nearestAttackPoint = obj.GetChild(i);
                }
            }

        }
        public void GoUpToTower()
        {
            if (gC.troopType == TroopTypeEnum.Tnt) // Goblin türü tnt ise
            {
                if (!gC.attackTheAllKnights) // Saldırı emri yoksa
                {
                    if (gC.woodTowers.Length == 0 || gC.onBuilding) // Kuledeyse veya tespit edilen kuleler yoksa
                    {
                        gC.aI = true;
                        return;
                    }

                    nearestWoodTower = DetechNearestTower(); // En yakın kuleyi bul
                    if (nearestWoodTower != null)
                    {
                        Debug.Log("kuleye git");
                        bC = nearestWoodTower.GetComponent<BuildingController>();
                        gC.aI = false;

                        gatePos = nearestWoodTower.transform.GetChild(0).position;
                        towerPos = nearestWoodTower.transform.GetChild(1).position;
                        pF.MoveAI(gatePos, 0);
                        gC.onBuildingStay = true;
                        gC.goBuilding = true;

                        // Kule dolu değilse, sıkışmaları engellemek için collider ları kapat
                        if (!bC.isFull && Vector2.Distance(gC.transform.position, gatePos) <= 1f)
                        {
                            gC.goblinCollider.isTrigger = true;
                            pF.agent.radius = 0;

                        }
                        // Kuleye çık
                        if (Vector2.Distance(gC.transform.position, gatePos) < .1f)
                        {
                            // Kulede birim varsa, çıkma
                            if (bC.isFull)
                            {
                                for (int i = 0; i < gC.woodTowers.Length; i++) // kuleyi boşa düşür
                                {
                                    gC.woodTowers[i] = null;
                                }
                                gC.goblinCollider.isTrigger = false;
                                pF.agent.radius = gC.goblinCollider.radius;
                                pF.agent.ResetPath();
                                gC.aI = true;
                                gC.goBuilding = false;
                                gC.onBuildingStay = false;
                                return;
                            }
                            // Kulede birim yoksa, çık
                            if (!bC.isFull)
                            {
                                Debug.Log("Kuleye çık");
                                pF.agent.ResetPath();
                                goblinSpriteRenderer.sortingOrder = 12;
                                gC.currentSightRange = gC.attackRange; // Kulede sabit kal
                                gC.aI = true;
                                gC.onBuilding = true;
                                gC.goBuilding = false;
                                gC.transform.position = towerPos; // Birimi kuleye ışınla
                                gC.gameObject.layer = 24; // ölümsüz ol
                                bC.isFull = true;
                                bC.gameObject.layer = 28; // Kulenin katmanı dolu olacak şekilde değişti
                            }
                        }
                    }
                }

                // Kuleden in
                if (gC.onBuilding && gC.attackTheAllKnights)
                {
                    Debug.Log("Kuleden in");
                    goblinSpriteRenderer.sortingOrder = 9;
                    gC.onBuildingStay = false;
                    gC.currentSightRange = gC.sightRange;
                    gC.onBuilding = false;
                    gC.gameObject.layer = 13; // ölümlü ol
                    gC.transform.position = gatePos; // kulenin kapısına git
                    bC.isFull = false;
                    bC.gameObject.layer = 27; // Kulenin katmanı boş olacak şekilde değişti
                    gC.goblinCollider.isTrigger = false;
                    pF.agent.radius = gC.goblinCollider.radius;

                }

                //if (gC.onBuilding)
                //{
                //    gC.transform.position = towerPos; // Birimi kuleye ışınla
                //}
                //if (nearestWoodTower == null)
                //{
                //    gC.goblinCollider.isTrigger = false;
                //    pF.agent.radius = gC.goblinCollider.radius;
                //}
            }
        }
        public void DestructTower()
        {
            if (bC == null) return;
            if (bC.destruct && gC.onBuilding)
            {
                Debug.Log("Kuleden düş");
                goblinSpriteRenderer.sortingOrder = 9;
                gC.onBuildingStay = false;
                gC.currentSightRange = gC.sightRange;
                gC.onBuilding = false;
                gC.gameObject.layer = 13; // ölümlü ol
                gC.transform.position = gatePos; // kulenin kapısına git
                bC.isFull = false;
                bC.gameObject.layer = 29; // Kulenin katmanı yıkıldı olacak şekilde değişti
            }
        }
        GameObject DetechNearestTower()
        {
            if (gC.woodTowers.Length > 0)
            {
                GameObject nearestTarget = null;
                float shortestDistance = Mathf.Infinity;
                for (int i = 0; i < gC.woodTowers.Length; i++)
                {
                    if (gC.woodTowers[i] != null)
                    {
                        float distanceToEnemy = Vector2.Distance(gC.transform.position, gC.woodTowers[i].transform.position);

                        if (shortestDistance > distanceToEnemy)
                        {
                            shortestDistance = distanceToEnemy;
                            nearestTarget = gC.woodTowers[i].gameObject;
                        }

                    }
                }

                return nearestTarget;
            }
            else
                return null;
        }


    }
}
