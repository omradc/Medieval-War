using Assets.Scripts.Concrete.Combats;
using Assets.Scripts.Concrete.AI;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;
using Assets.Scripts.Concrete.Managers;
using System;
using Assets.Scripts.Concrete.KnightBuildings;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class GoblinController : MonoBehaviour
    {
        [Header("ENEMY TYPE")]
        public TroopTypeEnum troopType;

        [Header("TORCH")]
        public float attackRadius;
        public Transform torchAttackPoint;
        public float torchAttackPointDistance;

        [Header("DYNAMİTE")]
        public float dynamiteSpeed;
        public float dynamiteExplosionRadius = .5f;

        [Header("BARREL")]
        public float barrelExplosionRadius = 2;

        [Header("ENEMY UNİT")]
        [Range(0.1f, 2f)] public float moveSpeed;
        public int damage;
        public float attackSpeed;
        public float attackDelay;
        public float sightRange;
        public float attackRange;

        [Header("ENEMY SETTİNGS")]
        [Range(0.1f, 1f)] public float enemyAIPerTime = 0.5f;
        public bool attackTheAllKnights;
        public Collider2D[] playerUnits;
        public Collider2D[] playerBuildings;
        public Collider2D[] playerObjs;
        public Collider2D[] hitTargets;
        public Collider2D[] woodTowers;
        public LayerMask targetAll;
        public LayerMask targetUnits;
        public LayerMask targetBuildings;
        public LayerMask woodTower;

        [Header("PATROLLİNG")]
        public float patrollingRadius;
        public float waitingTime;
        public BehaviorEnum behavior;
        public Transform path;

        [HideInInspector] public Transform[] patrolPoints;
        [HideInInspector] public GameObject explosion;
        [HideInInspector] public GameObject dynamite;
        [HideInInspector] public Vector2 attackRangePosition;
        [HideInInspector] public Vector2 sightRangePosition;
        [HideInInspector] public float currentSightRange;
        [HideInInspector] public Direction direction;
        [HideInInspector] public Animator animator;

        public bool aI = true;
        public bool onBuilding;
        public bool onBuildingStay;
        public bool goBuilding;

        EnemyAttack enemyAttack;
        EnemyAI enemyAI;
        PathFindingController pF;
        AnimationEventController animationEventController;
        Vector3 gizmosPos;
        bool canAttack;
        float currentStoppingDistance;
        private void Awake()
        {
            animator = transform.GetChild(0).GetComponent<Animator>();
            pF = GetComponent<PathFindingController>();
            direction = new Direction(transform);
            enemyAI = new(this, pF);
            animationEventController = transform.GetChild(0).GetComponent<AnimationEventController>();
        }
        private void Start()
        {
            enemyAttack = new(this, enemyAI, animationEventController, pF);
            pF.agent.speed = moveSpeed;
            currentStoppingDistance = attackRange;
            pF.agent.stoppingDistance = currentStoppingDistance;
            currentSightRange = sightRange;
            gizmosPos = transform.position;
            //PatrolSetup();


            // Invoke
            InvokeRepeating(nameof(OptimumEnemyAI), .1f, enemyAIPerTime);
        }

        private void Update()
        {
            AttackOn();
            AnimationControl();
            RangeControl();
            enemyAI.DestructTower();
        }
        void OptimumEnemyAI()
        {
            attackRangePosition = transform.GetChild(0).position;
            sightRangePosition = transform.GetChild(0).position;

            if (aI)
            {
                DetechEnemies();
                AITurnDirection();
                enemyAI.CatchNeraestTarget();
                enemyAI.GoblinBehaviour();
            }

            enemyAI.GoUpToTower();
        }
        void DetechEnemies()
        {
            // Varil için iki farklı hedef türü vardır, önceliği yapılar.
            if (troopType == TroopTypeEnum.Barrel)
            {
                playerUnits = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, targetUnits);
                playerBuildings = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, targetBuildings);
            }
            else
                playerObjs = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, targetAll);

            if (troopType == TroopTypeEnum.Tnt)
            {
                woodTowers = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, woodTower);
            }
        }

        void AITurnDirection()
        {
            // Hedefte düşman varsa ve durduysan, hedefe yönel.
            if (enemyAI.nearestTarget != null && pF.isStopped)
            {
                if (troopType == TroopTypeEnum.Tnt || troopType == TroopTypeEnum.Barrel)
                    direction.Turn2DirectionWithPos(enemyAI.nearestAttackPoint.position.x);
                if (troopType == TroopTypeEnum.Torch)
                    direction.Turn4Direction(enemyAI.nearestAttackPoint.position);
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
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(sightRangePosition, currentSightRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackRangePosition, attackRange);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(gizmosPos, patrollingRadius);
        }

        void RangeControl()
        {
            attackRangePosition = transform.GetChild(0).position;

            if (!aI)
                sightRangePosition = transform.GetChild(0).position;

            if (behavior == BehaviorEnum.Default && troopType != TroopTypeEnum.Tnt)
                currentStoppingDistance = attackRange;

            else
                currentStoppingDistance = 0;

            pF.agent.stoppingDistance = currentStoppingDistance;
        }

        void AnimationControl()
        {
            if (canAttack)
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
            {
                if (pF.isStopped) // Durduysan = IdleAnim
                    AnimationManager.Instance.IdleAnim(animator);
                if (!pF.isStopped)                           // Durmadıysan = RunAnim
                    AnimationManager.Instance.RunAnim(animator, 1);

            }
        }

        void AttackOn()
        {
            // Düşman varsa ve saldırı menzilindeyse, saldırı aktifleşir
            if (enemyAI.nearestTarget != null)
            {
                if (Vector2.Distance(attackRangePosition, enemyAI.nearestAttackPoint.position) < attackRange)
                    canAttack = true;
                else
                    canAttack = false;
            }

            else
                canAttack = false;
        }
    }
}
