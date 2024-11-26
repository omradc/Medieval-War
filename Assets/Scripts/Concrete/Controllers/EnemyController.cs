using Assets.Scripts.Concrete.Combats;
using Assets.Scripts.Concrete.EnemyAIs;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Movements;
using Assets.Scripts.Concrete.Orders;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class EnemyController : MonoBehaviour
    {
        [Header("ENEMY TYPE")]
        public EnemyTypeEnum enemyTypeEnum;

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
        [Range(0.1f, 1f)] public float detechTargetPerTime = 0.5f;
        [Range(0.1f, 1f)] public float turnDirectionPerTime = 0.5f;
        public bool attack;
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
        public PatrolTypeEnum patrolType;
        public Transform path;
        [HideInInspector] public Transform[] patrolPoints;


        [HideInInspector] public GameObject explosion;
        [HideInInspector] public GameObject dynamite;
        [HideInInspector] public Vector2 attackRangePosition;
        [HideInInspector] public Vector2 sightRangePosition;
        [HideInInspector] public int currentDamage;
        [HideInInspector] public float currentMoveSpeed;
        [HideInInspector] public float currentAttackSpeed;
        [HideInInspector] public float currentAttackDelay;
        [HideInInspector] public float currentAttackRange;
        [HideInInspector] public float currentSightRange;
        [HideInInspector] public float currentAttackRadius;
        [HideInInspector] public float currentDynamiteSpeed;
        [HideInInspector] public float currentDynamiteExplosionRadius;
        [HideInInspector] public float currentBarrelExplosionRadius;
        [HideInInspector] public EnemyDirection direction;
        public bool aI = true;
        public bool onTower;


        EnemyAttack enemyAttack;
        EnemyAI enemyAI;
        EnemyPathFinding2D ePF2D;
        AnimationEventController animationEventController;
        [HideInInspector] public Animator animator;
        Vector3 gizmosPos;
        Rigidbody2D rb2D;
        private void Awake()
        {
            ePF2D = GetComponent<EnemyPathFinding2D>();
            direction = new(ePF2D, this);
            enemyAI = new(this, ePF2D);
            animationEventController = transform.GetChild(0).GetComponent<AnimationEventController>();
        }
        private void Start()
        {
            enemyAttack = new(this, enemyAI, ePF2D, animationEventController);
            currentDamage = damage;
            currentAttackSpeed = attackSpeed;
            currentAttackDelay = attackDelay;
            currentSightRange = sightRange;
            currentAttackRange = attackRange;
            currentAttackRadius = attackRadius;
            currentDynamiteSpeed = dynamiteSpeed;
            currentDynamiteExplosionRadius = dynamiteExplosionRadius;
            currentBarrelExplosionRadius = barrelExplosionRadius;
            gizmosPos = transform.position;
            rb2D = GetComponent<Rigidbody2D>();
            animator = transform.GetChild(0).GetComponent<Animator>();
            //PatrolSetup();


            // Invoke
            InvokeRepeating(nameof(OptimumEnemyAI), .1f, enemyAIPerTime);
            InvokeRepeating(nameof(OptimumDetech), .5f, detechTargetPerTime);
            InvokeRepeating(nameof(OptimumAITurnDirection), 0.1f, turnDirectionPerTime);
        }

        private void Update()
        {
            // Hareket hızını fps farkına göre ayarla
            currentMoveSpeed = moveSpeed * Time.deltaTime;
        }
        void OptimumEnemyAI()
        {
            attackRangePosition = transform.GetChild(0).position;
            sightRangePosition = transform.GetChild(0).position;

            if (aI)
            {
                enemyAI.CatchNeraestTarget();
                enemyAI.StopWhenAttackDistance();
                enemyAI.Patrolling();
                enemyAttack.Attack();
                enemyAI.RigidbodyControl(rb2D);

            }

            enemyAI.GoUpToTower();


        }
        void OptimumDetech()
        {
            // Varil için iki farklı hedef türü vardır, önceliği yapılar.
            if (enemyTypeEnum == EnemyTypeEnum.Barrel)
            {
                playerUnits = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, targetUnits);
                playerBuildings = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, targetBuildings);
            }
            else
                playerObjs = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, targetAll);

            if (enemyTypeEnum == EnemyTypeEnum.Tnt)
            {
                woodTowers = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, woodTower);
            }
        }
        void OptimumAITurnDirection()
        {
            // ePF2D.pathLeftToGo[0]; hedefe giderken kullandığı yol
            if (enemyAI.nearestTarget == null) return;
            if (enemyTypeEnum == EnemyTypeEnum.Tnt || enemyTypeEnum == EnemyTypeEnum.Barrel)
            {
                // Durduğunda hadefe bak
                if (ePF2D.isPathEnd)
                    direction.Turn2Direction(enemyAI.nearestTarget.transform.position.x);

                // İlerlediğinde yola bak
                else if (ePF2D.pathLeftToGo.Count > 0)
                    direction.Turn2Direction(ePF2D.pathLeftToGo[0].x);

            }
            if (enemyTypeEnum == EnemyTypeEnum.Torch && ePF2D.pathLeftToGo.Count > 0)
                direction.Turn4Direction(ePF2D.pathLeftToGo[0]);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(sightRangePosition, currentSightRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackRangePosition, currentAttackRange);

            if (enemyTypeEnum == EnemyTypeEnum.Torch)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawWireSphere(torchAttackPoint.position, currentAttackRadius);

            }

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(gizmosPos, patrollingRadius);
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
    }
}
