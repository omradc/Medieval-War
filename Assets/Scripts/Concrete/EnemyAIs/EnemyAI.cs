using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.KnightBuildings;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;


namespace Assets.Scripts.Concrete.EnemyAIs
{
    internal class EnemyAI
    {
        EnemyController eC;
        EnemyPathFinding2D ePF2D;
        WoodTowerController woodTowerController;
        SpriteRenderer tntSpriteRenderer;
        public Transform nearestAttackPoint;
        public GameObject nearestTarget;
        Vector3 targetPoint;
        Vector2 gatePos;
        Vector2 woodTowerPos;
        readonly Vector3 firstPoint;
        float time;
        float timeToGetOffTower = 1;
        bool patrol;
        int index;

        public EnemyAI(EnemyController enemyController, EnemyPathFinding2D enemyPathFinding2D)
        {
            eC = enemyController;
            ePF2D = enemyPathFinding2D;
            firstPoint = eC.transform.position;
            targetPoint = firstPoint;
            tntSpriteRenderer = eC.transform.GetChild(0).GetComponent<SpriteRenderer>();
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
            nearestTarget = DetechNearestTarget();

            if (nearestTarget == null) return;

            CalculateNearestAttackPoint();

            if (Vector2.Distance(nearestAttackPoint.position, eC.sightRangePosition) < eC.currentSightRange)
            {
                // hedef, saldırı menziline girerse; yakalamayı bırak
                if (Vector2.Distance(nearestAttackPoint.position, eC.attackRangePosition) < eC.currentAttackRange) return;
                AnimationManager.Instance.RunAnim(ePF2D.animator, 1);
                ePF2D.AIGetMoveCommand(nearestAttackPoint.position);
            }
        }
        public void StopWhenAttackDistance() // Yapay zeka düşmanın tam koordinatlarına gider, fakat bu isPathEnd ile engellenir.
        {
            if (nearestTarget != null)
            {
                if (Vector2.Distance(nearestAttackPoint.position, eC.attackRangePosition) < eC.currentAttackRange)
                    ePF2D.isPathEnd = true;
                if (Vector2.Distance(nearestAttackPoint.position, eC.attackRangePosition) > eC.currentAttackRange)
                    ePF2D.isPathEnd = false;
            }
            else
                ePF2D.isPathEnd = false;

        }
        public void Patrolling()
        {
            if (eC.onBuilding) return;
            CirclePatrollingAnchor();
            CirclePatrollingFree();
            PointPatrolling();
            AttackTheAllUnit();
        }
        void CirclePatrollingAnchor()
        {
            if (eC.patrolType != PatrolTypeEnum.CirclePatrollingAnchor) return;
            if (nearestTarget != null) return;
            if (patrol)
            {
                patrol = false;
                targetPoint = firstPoint;
                targetPoint += new Vector3(Random.Range(-eC.patrollingRadius, eC.patrollingRadius), Random.Range(-eC.patrollingRadius, eC.patrollingRadius));
                ePF2D.AIGetMoveCommand(targetPoint);
                AnimationManager.Instance.RunAnim(ePF2D.animator, 1);
            }

            if (ePF2D.pathLeftToGo.Count == 0)
            {
                time++;

                if (time >= eC.waitingTime)
                {
                    time = 0;
                    patrol = true;
                }
            }
        }
        void CirclePatrollingFree()
        {
            if (eC.patrolType != PatrolTypeEnum.CirclePatrollingFree) return;
            if (nearestTarget != null) return;

            if (patrol)
            {
                patrol = false;
                targetPoint += new Vector3(Random.Range(-eC.patrollingRadius, eC.patrollingRadius), Random.Range(-eC.patrollingRadius, eC.patrollingRadius));
                ePF2D.AIGetMoveCommand(targetPoint);
                AnimationManager.Instance.RunAnim(ePF2D.animator, 1);
            }

            if (ePF2D.pathLeftToGo.Count == 0)
            {
                time++;

                if (time >= eC.waitingTime)
                {
                    time = 0;
                    patrol = true;
                }
            }
        }
        void PointPatrolling()
        {
            if (eC.patrolType != PatrolTypeEnum.PointPatrolling) return;
            if (nearestTarget != null) return;

            // Devriye gez
            if (patrol)
            {
                targetPoint = eC.patrolPoints[index].position;
                ePF2D.AIGetMoveCommand(targetPoint);
                index++;
                if (index == eC.patrolPoints.Length)
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
            if (eC.patrolType != PatrolTypeEnum.FindNearestPlayerUnit) return;
            eC.currentSightRange = 100;
            eC.attackTheAllKnights = true;
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
                float distanceToTarget = Vector2.Distance(eC.transform.position, obj.GetChild(i).position);
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
            if (eC.enemyTypeEnum == EnemyTypeEnum.Tnt)
            {

                if (!eC.attackTheAllKnights)
                {
                    if (eC.woodTowers.Length == 0 || eC.onBuilding)
                    {
                        eC.aI = true;
                        tntSpriteRenderer.enabled = true;
                        return;
                    }

                    Debug.Log(0);
                    // En yakın kuleyi bul
                    GameObject nearestWoodTower = DetechNearestTower();

                    if (nearestWoodTower == null) return;

                    Debug.Log("kuleye git");
                    woodTowerController = nearestWoodTower.GetComponent<WoodTowerController>();
                    eC.aI = false;
                    ePF2D.isPathEnd = false;
                    // Etrafta düşman varken yapay zeka kapatıldığında düşmanın son konumuna gitmemesi için, yolları temizle
                    ePF2D.path.Clear();
                    ePF2D.pathLeftToGo.Clear();
                    gatePos = nearestWoodTower.transform.GetChild(0).position;
                    woodTowerPos = nearestWoodTower.transform.GetChild(1).position;
                    ePF2D.AIGetMoveCommand(gatePos);
                    AnimationManager.Instance.RunAnim(eC.animator, 1);
                    eC.stayBuilding = true;

                    // Kuleye çık
                    if (Vector2.Distance(eC.transform.position, gatePos) < .3f)
                    {
                        Debug.Log(1);
                        tntSpriteRenderer.enabled = false;

                        // Kule dolu ise çıkma
                        if (woodTowerController.isFull)
                        {
                            Debug.Log("Kule dolu");
                            nearestWoodTower = null;
                            tntSpriteRenderer.enabled = true;
                            time = 0;
                            return;
                        }

                        time++;
                        // Kulede birim yoksa, çık
                        if (time > timeToGetOffTower && !woodTowerController.isFull)
                        {
                            Debug.Log("Kuleye çık");
                            tntSpriteRenderer.enabled = true;
                            ePF2D.isPathEnd = true; // Dur
                            eC.currentSightRange = eC.currentAttackRange;
                            eC.aI = true;
                            eC.onBuilding = true;
                            eC.transform.position = woodTowerPos; // Birimi kuleye ışınla
                            eC.gameObject.layer = 25; // ölümsüz ol
                            AnimationManager.Instance.IdleAnim(eC.animator);
                            nearestWoodTower = null;
                            woodTowerController.isFull = true;
                            woodTowerController.gameObject.layer = 28; // Kulenin katmanı dolu olacak şekilde değişti
                            time = 0;
                        }
                    }
                }

                // Kuleden in
                if (eC.onBuilding && eC.attackTheAllKnights && !ePF2D.isPathEnd)
                {
                    tntSpriteRenderer.enabled = false;
                    time++;
                    if (time > timeToGetOffTower)
                    {
                        Debug.Log("Kuleden in");
                        tntSpriteRenderer.enabled = true;
                        eC.stayBuilding = false;
                        eC.currentSightRange = eC.sightRange;
                        eC.onBuilding = false;
                        eC.gameObject.layer = 13; // ölümlü ol
                        eC.transform.position = gatePos; // kulenin kapısına git
                        woodTowerController.isFull = false;
                        woodTowerController.gameObject.layer = 27; // Kulenin katmanı boş olacak şekilde değişti
                        time = 0;
                    }
                }
            }
        }
        public void DestructTower()
        {
            if (woodTowerController == null) return;
            if (woodTowerController.destruct && eC.onBuilding)
            {
                Debug.Log("Kuleden düş");
                tntSpriteRenderer.enabled = true;
                eC.stayBuilding = false;
                eC.currentSightRange = eC.sightRange;
                eC.onBuilding = false;
                eC.gameObject.layer = 13; // ölümlü ol
                eC.transform.position = gatePos; // kulenin kapısına git
                woodTowerController.isFull = false;
                woodTowerController.gameObject.layer = 29; // Kulenin katmanı yıkıldı olacak şekilde değişti
                time = 0;

            }
        }
        GameObject DetechNearestTower()
        {
            if (eC.woodTowers.Length > 0)
            {
                GameObject nearestTarget = null;
                float shortestDistance = Mathf.Infinity;
                for (int i = 0; i < eC.woodTowers.Length; i++)
                {
                    if (eC.woodTowers[i] != null)
                    {
                        float distanceToEnemy = Vector2.Distance(eC.transform.position, eC.woodTowers[i].transform.position);

                        if (shortestDistance > distanceToEnemy)
                        {
                            shortestDistance = distanceToEnemy;
                            nearestTarget = eC.woodTowers[i].gameObject;
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
