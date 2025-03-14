using Assets.Scripts.Concrete.Combats;
using Assets.Scripts.Concrete.AI;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;
using Assets.Scripts.Concrete.Managers;
using System;
using Assets.Scripts.Concrete.PowerStats;

namespace Assets.Scripts.Concrete.Controllers
{
    [RequireComponent(typeof(PathFinding))]
    internal class GoblinController : MonoBehaviour
    {

        [Header("SETTİNGS")]
        public FactionTypeEnum factionType;
        [SerializeField][Range(0.1f, 1f)] float turnDirectionPerTime = 0.5f;
        [SerializeField][Range(0.1f, 1f)] float aIPerTime = 0.5f;
        public LayerMask enemiesLayer;
        [SerializeField] LayerMask friendsLayer;
        [SerializeField] LayerMask targetWoodTower;
        [SerializeField] Transform orderInLayerSpriteAnchor;
        [SerializeField] SpriteRenderer visual;

        [Header("PATROLLİNG")]
        public float patrollingRadius;
        public float waitingTime;
        public BehaviorEnum behavior;
        public Transform path;

        [HideInInspector] public int damage;
        [HideInInspector] public float dynamiteSpeed;
        [HideInInspector] public float dynamiteExplosionRadius = .5f;
        [HideInInspector] public float barrelExplosionRadius = 2;
        [HideInInspector] public float longRange;
        public float sightRange;
        public float currentSightRange;
        public float attackRange;
        public float currentAttackRange;
        [HideInInspector] public float currentLongRange;
        [HideInInspector] public bool aI = true;
        [HideInInspector] public bool onBuilding;
        [HideInInspector] public bool goBuilding;
        [HideInInspector] public Collider2D[] woodTowers;
        [HideInInspector] public Collider2D[] sightRangeDetechEnemies;
        [HideInInspector] public Collider2D[] longRangeDetechEnemies;
        [HideInInspector] public Collider2D[] friendsDetech;
        [HideInInspector] public CircleCollider2D goblinCollider;
        [HideInInspector] public Transform[] patrolPoints;
        [HideInInspector] public GameObject nonRangeDetechEnemy;
        [HideInInspector] public GameObject explosion;
        [HideInInspector] public GameObject dynamite;
        Direction direction;
        Animator animator;
        GoblinAI goblinAI;
        PathFinding pF;
        AnimationEventController animationEventController;
        DynamicOrderInLayer dynamicOrderInLayer;
        TorchStats torchStats;
        TntStats tntStats;
        BarrelStats barrelStats;
        HealthController healthController;
        TargetPriority targetPriority;
        Vector3 gizmosPos;
        float moveSpeed;
        float time;
        float attackSpeed;
        float attackInterveal;
        float reportRange;
        bool canAttack;
        public bool obstacle;
        public bool height;
        RaycastHit2D hitObj;

        private void Awake()
        {
            animator = transform.GetChild(0).GetComponent<Animator>();
            goblinCollider = GetComponent<CircleCollider2D>();
            pF = GetComponent<PathFinding>();
            healthController = GetComponent<HealthController>();
            direction = new Direction(transform);
            goblinAI = new(this, pF);
            animationEventController = transform.GetChild(0).GetComponent<AnimationEventController>();
            new EnemyAttack(this, goblinAI, animationEventController, pF);
            dynamicOrderInLayer = new();
            targetPriority = GetComponent<TargetPriority>();
        }
        private void Start()
        {
            PowerStatsAssign();
            pF.agent.speed = moveSpeed;
            currentAttackRange = attackRange;
            currentSightRange = sightRange;
            gizmosPos = transform.position;
            time = attackInterveal;
            animationEventController.ResetAttackEvent += ResetAttack;
            //PatrolSetup();
            // Invoke
            InvokeRepeating(nameof(OptimumTurnDirection), 0, turnDirectionPerTime);
            InvokeRepeating(nameof(OptimumAI), 0, aIPerTime);
        }
        void PowerStatsAssign()
        {
            if (factionType == FactionTypeEnum.Torch)
                torchStats = GetComponent<TorchStats>();
            if (factionType == FactionTypeEnum.Tnt)
                tntStats = GetComponent<TntStats>();
            if (factionType == FactionTypeEnum.Barrel)
                barrelStats = GetComponent<BarrelStats>();

            if (torchStats != null)
            {
                healthController.health = torchStats.health;
                healthController.regeneration = torchStats.regrenation;
                healthController.regenerationAmount = torchStats.regenerationAmount;
                healthController.regrenationPerTime = torchStats.regrenationPerTime;
                healthController.regrenationAfterDamageTime = torchStats.regrenationAfterDamageTime;
                moveSpeed = torchStats.moveSpeed;
                damage = torchStats.damage;
                attackSpeed = torchStats.attackSpeed;
                attackInterveal = torchStats.attackInterval;
                attackRange = torchStats.attackRange;
                sightRange = torchStats.sightRange;
                longRange = torchStats.longRange;
                reportRange = torchStats.reportRange;
                targetPriority.priority = torchStats.priority;
                targetPriority.maxAttacker = torchStats.maxAttacker;
            }

            if (tntStats != null)
            {
                healthController.health = tntStats.health;
                healthController.regeneration = tntStats.regrenation;
                healthController.regenerationAmount = tntStats.regenerationAmount;
                healthController.regrenationPerTime = tntStats.regrenationPerTime;
                healthController.regrenationAfterDamageTime = tntStats.regrenationAfterDamageTime;
                moveSpeed = tntStats.moveSpeed;
                damage = tntStats.damage;
                attackSpeed = tntStats.attackSpeed;
                attackInterveal = tntStats.attackInterval;
                attackRange = tntStats.attackRange;
                sightRange = tntStats.sightRange;
                longRange = tntStats.longRange;
                reportRange = tntStats.reportRange;
                dynamiteSpeed = tntStats.dynamiteSpeed;
                dynamiteExplosionRadius = tntStats.dynamiteExplosionRadius;
                targetPriority.priority = tntStats.priority;
                targetPriority.maxAttacker = tntStats.maxAttacker;
            }

            if (barrelStats != null)
            {
                healthController.health = barrelStats.health;
                healthController.regeneration = barrelStats.regrenation;
                healthController.regenerationAmount = barrelStats.regenerationAmount;
                healthController.regrenationPerTime = barrelStats.regrenationPerTime;
                healthController.regrenationAfterDamageTime = barrelStats.regrenationAfterDamageTime;
                moveSpeed = barrelStats.moveSpeed;
                damage = barrelStats.damage;
                attackSpeed = barrelStats.attackSpeed;
                attackInterveal = barrelStats.attackInterval;
                attackRange = barrelStats.attackRange;
                sightRange = barrelStats.sightRange;
                longRange = barrelStats.longRange;
                reportRange = barrelStats.reportRange;
                barrelExplosionRadius = barrelStats.barrelExplosionRadius;
                targetPriority.priority = barrelStats.priority;
                targetPriority.maxAttacker = barrelStats.maxAttacker;
            }

        }
        private void Update()
        {
            dynamicOrderInLayer.OrderInLayerUpdate(orderInLayerSpriteAnchor, visual);
            AttackOn();
            AnimationControl();
            goblinAI.DestructTower();
        }
        void OptimumTurnDirection()
        {
            if (aI)
            {
                AITurnDirection();
            }

        }
        void OptimumAI()
        {
            if (aI)
            {
                goblinAI.GoblinBehaviour();
                DetechEnemies();
                goblinAI.CatchNeraestTarget();
                //ResetPath();
                ObsticleControl();
            }
            goblinAI.GoUpToTower();
        }
        void DetechEnemies()
        {
            // Görüş menzilindeki düşmanları bulur
            sightRangeDetechEnemies = Physics2D.OverlapCircleAll(transform.position, currentSightRange, enemiesLayer);
            friendsDetech = Physics2D.OverlapCircleAll(transform.position, reportRange, friendsLayer);

            // Saldırı emri varsa ve görüş menzlinde düşman yoksa, düşmaları bulur
            if (behavior == BehaviorEnum.AttackOrder && sightRangeDetechEnemies.Length == 0)
                longRangeDetechEnemies = Physics2D.OverlapCircleAll(transform.position, currentLongRange, enemiesLayer);

            if (factionType == FactionTypeEnum.Tnt)
                woodTowers = Physics2D.OverlapCircleAll(transform.position, currentSightRange, targetWoodTower);
        }
        void AITurnDirection()
        {
            // Hedefte düşman varsa ve durduysan, hedefe yönel.
            if (goblinAI.target != null && pF.isStopping)
            {
                if (factionType == FactionTypeEnum.Tnt || factionType == FactionTypeEnum.Barrel)
                    direction.Turn2DirectionWithPos(goblinAI.nearestAttackPoint.position.x);
                if (factionType == FactionTypeEnum.Torch)
                    direction.Turn4Direction(goblinAI.nearestAttackPoint.position);
            }
        }
        void PatrolSetup()
        {
            if (path == null) return;
            patrolPoints = new Transform[path.childCount];
            for (int i = 0; i < path.childCount; i++)
            {
                patrolPoints[i] = path.GetChild(i);
            }
        }
        void AnimationControl()
        {
            if (goBuilding)
            {
                AnimationManager.Instance.RunAnim(animator, 1);
            }
            else if (canAttack)
            {
                time += Time.deltaTime;
                if (time >= attackInterveal)
                {
                    //Animasyonlar, saldırıları event ile tetikler ve yöne göre animasyonlar oynatılır.
                    if (direction.right || direction.left)
                        AnimationManager.Instance.AttackFrontAnim(animator, attackSpeed);
                    if (direction.up)
                        AnimationManager.Instance.AttackUpAnim(animator, attackSpeed);
                    if (direction.down)
                        AnimationManager.Instance.AttackDownAnim(animator, attackSpeed);
                }

                else
                    AnimationManager.Instance.IdleAnim(animator);
            }
            else
            {
                if (pF.isMovementStopping) // Durduysan = IdleAnim
                    AnimationManager.Instance.IdleAnim(animator);
                if (!pF.isMovementStopping)                           // Durmadıysan = RunAnim
                    AnimationManager.Instance.RunAnim(animator, 1);

            }
        }
        void AttackOn()
        {
            // Düşman varsa ve saldırı menzilindeyse, saldırı aktifleşir
            if (goblinAI.target != null)
            {
                float enemyDistance = Vector2.Distance(transform.position, goblinAI.nearestAttackPoint.position);
                if (enemyDistance < currentAttackRange && !goBuilding)
                {
                    if (enemyDistance > 1) // DÜşan çok yakınsa engel tanıma
                    {
                        if (onBuilding) // Kule üstündeyse hiçbir yükselti veya engel atıcıyı engellemez
                        {
                            canAttack = true;
                            currentAttackRange = attackRange;
                        }
                        else if (!obstacle && !height) // Arada engel ve yükselti yoksa
                            canAttack = true;
                        else
                            canAttack = false;
                    }
                    else
                        canAttack = true;
                }
                else
                    canAttack = false;
            }
            else
                canAttack = false;
        }
        void ObsticleControl()
        {
            if (goblinAI.target == null) return;

            // Yükselti kontrolü
            if (healthController.elevationFloor >= goblinAI.target.GetComponent<HealthController>().elevationFloor)
                height = false;
            else
                height = true;

            // Engel kontrolü
            hitObj = Physics2D.Raycast(transform.position, (goblinAI.nearestAttackPoint.position - transform.position).normalized, attackRange, LayerMask.GetMask("Knight", "Wall", "House", "Tower", "WoodTower", "WoodTowerFull", "GoblinHouse"));
            if (hitObj.collider != null)
            {
                if (goblinAI.target.layer == hitObj.collider.gameObject.layer) // Hedefi engel olabilecek bir obje ise artık engel değildir
                    obstacle = false;
                else // Engel
                    obstacle = true;
            }
            else
                obstacle = false;

            if (obstacle || height) // Arada engel veya yükselti varsa, 
                currentAttackRange = 0;
            if (!obstacle && !height && currentAttackRange != attackRange) // Arada engel ve yükselti yoksa ve sadece 1 kez çalış,
            {
                currentAttackRange = attackRange;
                pF.agent.ResetPath();
            }
        }
        void ResetPath()
        {
            if (sightRangeDetechEnemies.Length == 0 && pF.agent.velocity.magnitude < 0.1f && pF.agent.velocity.magnitude > 0)
            {
                pF.agent.ResetPath();
            }
        }
        void ResetAttack()
        {
            time = 0;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(gizmosPos, patrollingRadius);

            Gizmos.color = Color.white;
            if (goblinAI != null)
                if (goblinAI.target != null)
                    Gizmos.DrawRay(transform.position, (goblinAI.nearestAttackPoint.position - transform.position).normalized * attackRange);
        }
    }
}
