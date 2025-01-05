using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;


namespace Assets.Scripts.Concrete.AI
{
    internal class EnemyAI
    {
        GoblinController gC;
        EnemyPathFinding2D ePF2D;
        BuildingController buildingController;
        SpriteRenderer tntSpriteRenderer;
        public Transform nearestAttackPoint;
        public GameObject nearestTarget;
        Vector3 targetPoint;
        Vector2 gatePos;
        Vector2 towerPos;
        readonly Vector3 firstPoint;
        float time;
        float timeToGetOffTower = 1;
        bool patrol;
        int index;

        public EnemyAI(GoblinController goblinController, EnemyPathFinding2D enemyPathFinding2D)
        {
            gC = goblinController;
            ePF2D = enemyPathFinding2D;
            firstPoint = gC.transform.position;
            targetPoint = firstPoint;
            tntSpriteRenderer = gC.transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        public GameObject DetechNearestTarget()
        {
            if (gC.enemyTypeEnum == EnemyTypeEnum.Barrel)
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
            if (gC.enemyTypeEnum != EnemyTypeEnum.Barrel)
            {
                // Yapı yoksa birimler
                if (gC.playerObjs.Length > 0)
                {
                    GameObject nearestTarget = null;
                    float shortestDistance = Mathf.Infinity;

                    for (int i = 0; i < gC.playerObjs.Length; i++)
                    {
                        if (gC.playerObjs[i] != null)
                        {
                            float distanceToEnemy = Vector2.Distance(gC.transform.position, gC.playerObjs[i].transform.position);

                            if (shortestDistance > distanceToEnemy)
                            {
                                shortestDistance = distanceToEnemy;
                                nearestTarget = gC.playerObjs[i].gameObject;
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
            StopWhenAttackDistance();

            if (Vector2.Distance(nearestAttackPoint.position, gC.sightRangePosition) < gC.currentSightRange)
            {
                // hedef, saldırı menziline girerse; yakalamayı bırak
                if (Vector2.Distance(nearestAttackPoint.position, gC.attackRangePosition) < gC.currentAttackRange) return;
                AnimationManager.Instance.RunAnim(ePF2D.animator, 1);
                ePF2D.AIGetMoveCommand(nearestAttackPoint.position);
            }
        }
        public void StopWhenAttackDistance() // Yapay zeka düşmanın tam koordinatlarına gider, fakat bu isPathEnd ile engellenir.
        {
            if (Vector2.Distance(nearestAttackPoint.position, gC.attackRangePosition) < gC.currentAttackRange)
                ePF2D.isPathEnd = true;
            if (Vector2.Distance(nearestAttackPoint.position, gC.attackRangePosition) >= gC.currentAttackRange)
                ePF2D.isPathEnd = false;
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
            if (gC.goblinBehaviour != GoblinBehaviorEnum.CirclePatrollingAnchor) return;
            if (nearestTarget != null) return;
            if (patrol)
            {
                patrol = false;
                targetPoint = firstPoint;
                targetPoint += new Vector3(Random.Range(-gC.patrollingRadius, gC.patrollingRadius), Random.Range(-gC.patrollingRadius, gC.patrollingRadius));
                ePF2D.AIGetMoveCommand(targetPoint);
                AnimationManager.Instance.RunAnim(ePF2D.animator, 1);
            }

            if (ePF2D.pathLeftToGo.Count == 0)
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
            if (gC.goblinBehaviour != GoblinBehaviorEnum.CirclePatrollingFree) return;
            if (nearestTarget != null) return;

            if (patrol)
            {
                patrol = false;
                targetPoint += new Vector3(Random.Range(-gC.patrollingRadius, gC.patrollingRadius), Random.Range(-gC.patrollingRadius, gC.patrollingRadius));
                ePF2D.AIGetMoveCommand(targetPoint);
                AnimationManager.Instance.RunAnim(ePF2D.animator, 1);
            }

            if (ePF2D.pathLeftToGo.Count == 0)
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
            if (gC.goblinBehaviour != GoblinBehaviorEnum.PointPatrolling) return;
            if (nearestTarget != null) return;

            // Devriye gez
            if (patrol)
            {
                targetPoint = gC.patrolPoints[index].position;
                ePF2D.AIGetMoveCommand(targetPoint);
                index++;
                if (index == gC.patrolPoints.Length)
                    index = 0;
                AnimationManager.Instance.RunAnim(ePF2D.animator, 1);
                patrol = false;
            }

            // Devriye naktasına ulaşıldı
            if (ePF2D.pathLeftToGo.Count == 0)
                patrol = true;
        }
        void AttackTheAllUnit()
        {
            if (gC.goblinBehaviour != GoblinBehaviorEnum.FindNearestPlayerUnit) return;
            gC.currentSightRange = 100;
            gC.attackTheAllKnights = true;
        }
        public void RigidbodyControl(Rigidbody2D rb2D, bool stayBuilding)
        {
            if (stayBuilding)
            {
                rb2D.bodyType = RigidbodyType2D.Kinematic;
                return;
            }
            // Menzilde düşman yoksa ve kullanıcıdan emir almadıysa rigidbody aktif olur.
            if (nearestTarget != null)
                rb2D.bodyType = RigidbodyType2D.Dynamic;
            else
                rb2D.bodyType = RigidbodyType2D.Kinematic;

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
            // Eğer goblin türü tnt ise, görüş menzili içerisindeki boş bir kuleye çıkar
            if (gC.enemyTypeEnum == EnemyTypeEnum.Tnt)
            {
                if (!gC.attackTheAllKnights)
                {
                    if (gC.woodTowers.Length == 0 || gC.onBuilding)
                    {
                        gC.aI = true;
                        gC.stayBuilding = false;
                        tntSpriteRenderer.enabled = true;

                        if (gC.onBuilding)
                            AnimationManager.Instance.IdleAnim(ePF2D.animator);
                        return;
                    }

                    // En yakın kuleyi bul
                    GameObject nearestWoodTower = DetechNearestTower();

                    if (nearestWoodTower == null) return;

                    Debug.Log("kuleye git");
                    buildingController = nearestWoodTower.GetComponent<BuildingController>();
                    gC.aI = false;
                    ePF2D.isPathEnd = false;
                    // Etrafta düşman varken yapay zeka kapatıldığında düşmanın son konumuna gitmemesi için, yolları temizle
                    ePF2D.path.Clear();
                    ePF2D.pathLeftToGo.Clear();
                    gatePos = nearestWoodTower.transform.GetChild(0).position;
                    towerPos = nearestWoodTower.transform.GetChild(1).position;
                    ePF2D.AIGetMoveCommand(gatePos);
                    AnimationManager.Instance.RunAnim(ePF2D.animator, 1);
                    gC.stayBuilding = true;

                    // Kuleye çık
                    if (Vector2.Distance(gC.transform.position, gatePos) < .3f)
                    {
                        tntSpriteRenderer.enabled = false;

                        time++;
                        // Kulede birim yoksa, çık
                        if (time > timeToGetOffTower /*&& !buildingController.isFull*/)
                        {
                            Debug.Log("Kuleye çık");
                            tntSpriteRenderer.enabled = true;
                            tntSpriteRenderer.sortingOrder = 12;
                            ePF2D.isPathEnd = true; // Dur
                            gC.currentSightRange = gC.currentAttackRange; // Kulede sabit kal
                            gC.aI = true;
                            gC.onBuilding = true;
                            gC.transform.position = towerPos; // Birimi kuleye ışınla
                            gC.gameObject.layer = 25; // ölümsüz ol
                            nearestWoodTower = null;
                            buildingController.isFull = true;
                            buildingController.gameObject.layer = 28; // Kulenin katmanı dolu olacak şekilde değişti
                            time = 0;
                        }
                    }
                }

                // Kuleden in
                if (gC.onBuilding && gC.attackTheAllKnights && !ePF2D.isPathEnd)
                {
                    tntSpriteRenderer.enabled = false;
                    time++;
                    if (time > timeToGetOffTower)
                    {
                        Debug.Log("Kuleden in");
                        tntSpriteRenderer.enabled = true;
                        tntSpriteRenderer.sortingOrder = 9;
                        gC.stayBuilding = false;
                        gC.currentSightRange = gC.sightRange;
                        gC.onBuilding = false;
                        gC.gameObject.layer = 13; // ölümlü ol
                        gC.transform.position = gatePos; // kulenin kapısına git
                        buildingController.isFull = false;
                        buildingController.gameObject.layer = 27; // Kulenin katmanı boş olacak şekilde değişti
                        time = 0;
                    }
                }
            }
        }
        public void DestructTower()
        {
            if (buildingController == null) return;
            if (buildingController.destruct && gC.onBuilding)
            {
                Debug.Log("Kuleden düş");
                tntSpriteRenderer.enabled = true;
                gC.stayBuilding = false;
                gC.currentSightRange = gC.sightRange;
                gC.onBuilding = false;
                gC.gameObject.layer = 13; // ölümlü ol
                gC.transform.position = gatePos; // kulenin kapısına git
                buildingController.isFull = false;
                buildingController.gameObject.layer = 29; // Kulenin katmanı yıkıldı olacak şekilde değişti
                time = 0;

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
