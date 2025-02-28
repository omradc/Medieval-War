using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Movements;
using Assets.Scripts.Concrete.Combats;
using UnityEngine;
using Unity.VisualScripting.FullSerializer;

namespace Assets.Scripts.Concrete.AI
{
    internal class GoblinAI
    {
        readonly GoblinController gC;
        readonly PathFinding pF;
        BuildingController bC;
        GameObject nearestWoodTower;
        readonly SpriteRenderer goblinSpriteRenderer;
        public Transform nearestAttackPoint;
        public GameObject target;
        Vector3 targetPoint;
        Vector2 gatePos;
        Vector2 towerPos;
        readonly Vector3 firstPoint;
        bool patrol;
        int index;
        float time;
        Transform obj;

        public GoblinAI(GoblinController goblinController, PathFinding pF)
        {
            gC = goblinController;
            this.pF = pF;
            firstPoint = gC.transform.position;
            targetPoint = firstPoint;
            goblinSpriteRenderer = gC.transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        GameObject ChooseTarget()
        {
            // Görüş menzilinde düşman varsa
            if (gC.sightRangeDetechEnemies.Length > 0)
            {
                GameObject bestTarget = null;
                float bestScore = Mathf.Infinity;

                for (int i = 0; i < gC.sightRangeDetechEnemies.Length; i++)
                {
                    float distance = Vector2.Distance(gC.transform.position, gC.sightRangeDetechEnemies[i].transform.position);
                    TargetPriority targetPriority = gC.sightRangeDetechEnemies[i].GetComponent<TargetPriority>();
                    float currentScore = targetPriority.attackingPersonNumber + distance - targetPriority.currentPriority;
                    if (bestScore > currentScore)
                    {
                        bestScore = currentScore;
                        bestTarget = gC.sightRangeDetechEnemies[i].gameObject;
                    }
                }
                SpotEnemy(bestTarget);
                return bestTarget;
            }

            // Saldırı emri varsa
            else if (gC.currentLongRange > 0)
            {
                GameObject target = null;
                float nearestDistance = Mathf.Infinity;

                for (int i = 0; i < gC.longRangeDetechEnemies.Length; i++)
                {
                    float distance = Vector2.Distance(gC.transform.position, gC.longRangeDetechEnemies[i].transform.position);

                    if (nearestDistance > distance)
                    {
                        nearestDistance = distance;
                        target = gC.longRangeDetechEnemies[i].gameObject;
                    }
                }
                return target;
            }

            // Saldırı emri yoksa ve görüş menzilinde düşman yoksa 
            else
            {
                // Görüş menzili dışından saldırılırsa
                if (gC.nonRangeDetechEnemy != null)
                {
                    GameObject nonRangeTarget = gC.nonRangeDetechEnemy;
                    SpotEnemy(nonRangeTarget);
                    return nonRangeTarget;
                }
                else
                    return null;
            }
        }

        void SpotEnemy(GameObject enemy)
        {
            if (target == null)
                for (int i = 0; i < gC.friendsDetech.Length; i++)
                {
                    gC.friendsDetech[i].GetComponent<GoblinController>().nonRangeDetechEnemy = enemy;
                }
        }
        public void CatchNeraestTarget()
        {
            target = ChooseTarget();
            if (target == null) return;

            CalculateNearestAttackPoint();

            // hedef, saldırı menziline girerse; yakalamayı bırak
            if (Vector2.Distance(nearestAttackPoint.position, gC.transform.position) < gC.currentAttackRange) return;
            pF.MoveAI(nearestAttackPoint.position, gC.currentAttackRange);

        }
        public void GoblinBehaviour()
        {
            AttackOrder();
            if (gC.onBuilding) return;
            CirclePatrollingAnchor();
            CirclePatrollingFree();
            PointPatrolling();
        }
        void CirclePatrollingAnchor()
        {
            if (gC.behavior != BehaviorEnum.CirclePatrollingAnchor) return;
            if (target != null) return;
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
            if (target != null) return;

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
            if (target != null) return;

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
        void AttackOrder()
        {
            if (gC.behavior != BehaviorEnum.AttackOrder) return;

            // Görüş menzilinde düşman yoksa, uzun menzil etkindir
            if (gC.sightRangeDetechEnemies.Length == 0)
                gC.currentLongRange = gC.longRange;
            // Görüş menzilinde düşman varsa, uzun menzil kapalıdır
            if (gC.sightRangeDetechEnemies.Length > 0)
                gC.currentLongRange = 0;
        }
        void CalculateNearestAttackPoint()
        {
            // Düşman şovalye ise
            if (target.layer == 6)
            {
                nearestAttackPoint = target.transform;
                return;
            }

            for (int i = 0; i < target.transform.childCount; i++)
            {
                if (target.transform.GetChild(i).name == "AttackPoints")
                    obj = target.transform.GetChild(i);
            }
            if (obj == null)
                Debug.Log("AttackPoints Bulunanamadı");

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
            if (gC.factionType == FactionTypeEnum.Tnt) // Goblin türü tnt ise
            {
                if (gC.behavior != BehaviorEnum.AttackOrder) // Saldırı emri yoksa
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
                                return;
                            }
                            // Kulede birim yoksa, çık
                            if (!bC.isFull)
                            {
                                Debug.Log("Kuleye çık");
                                pF.agent.ResetPath();
                                goblinSpriteRenderer.sortingOrder = 12;
                                gC.currentSightRange = gC.currentAttackRange; // Kulede sabit kal
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
                if (gC.onBuilding && gC.behavior == BehaviorEnum.AttackOrder)
                {
                    Debug.Log("Kuleden in");
                    goblinSpriteRenderer.sortingOrder = 9;
                    gC.currentSightRange = gC.sightRange;
                    gC.onBuilding = false;
                    gC.gameObject.layer = 13; // ölümlü ol
                    gC.transform.position = gatePos; // kulenin kapısına git
                    bC.isFull = false;
                    bC.gameObject.layer = 27; // Kulenin katmanı boş olacak şekilde değişti
                    gC.goblinCollider.isTrigger = false;
                    pF.agent.radius = gC.goblinCollider.radius;

                }
            }
        }
        public void DestructTower()
        {
            if (bC == null) return;
            if (bC.destruct && gC.onBuilding)
            {
                Debug.Log("Kuleden düş");
                goblinSpriteRenderer.sortingOrder = 9;
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
