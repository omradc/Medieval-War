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
        [Header("UNIT TYPE")]
        public EnemyTypeEnum enemyTypeEnum;

        [Header("Torch")]
        public float attackRadius;
        public Transform torchAttackPoint;
        public float torchAttackPointDistance;

        [Header("Dynamite")]
        public GameObject dynamite;
        public float dynamiteSpeed;
        public float dynamiteExplosionRadius = .5f;

        [Header("Barrel")]
        public GameObject explosion;
        public float barrelExplosionRadius = 2;

        [Header("ENEMY UNİT")]
        [Range(0.1f, 2f)] public float speed;
        public int damage;
        public float attackSpeed;
        public float attackDelay;
        public float sightRange;
        public float attackRange;


        public int currentDamage;
        public float currentSpeed;
        public float currentAttackSpeed;
        public float currentAttackDelay;
        public float currentAttackRange;
        public float currentSightRange;
        public float currentAttackRadius;
        public float currentDynamiteSpeed;
        public float currentDynamiteExplosionRadius;
        public float currentBarrelExplosionRadius;

        [Header("UNIT SETTİNGS")]
        [Range(0.1f, 1f)] public float enemyAIPerTime = 0.5f;
        [Range(0.1f, 1f)] public float detechTargetPerTime = 0.5f;
        [Range(0.1f, 1f)] public float turnDirectionPerTime = 0.5f;
        public Collider2D[] playerUnits;
        public Collider2D[] playerBuildings;
        public Collider2D[] hitTargets;
        public LayerMask targetUnits;
        public LayerMask targetBuildings;
        [HideInInspector] public Vector2 attackRangePosition;
        [HideInInspector] public Vector2 sightRangePosition;


        [HideInInspector] public EnemyDirection direction;
        EnemyAttack enemyAttack;
        EnemyAI enemyAI;
        EnemyPathFinding2D ePF2D;
        AnimationEventController animationEventController;
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
            currentSpeed = speed / 100;
            currentDamage = damage;
            currentAttackSpeed = attackSpeed;
            currentAttackDelay = attackDelay;
            currentSightRange = sightRange;
            currentAttackRange = attackRange;
            currentAttackRadius = attackRadius;
            currentDynamiteSpeed = dynamiteSpeed;
            currentDynamiteExplosionRadius = dynamiteExplosionRadius;
            currentBarrelExplosionRadius = barrelExplosionRadius;

            // Invoke
            InvokeRepeating(nameof(OptimumEnemyAI), .1f, enemyAIPerTime);
            InvokeRepeating(nameof(OptimumDetech), .5f, detechTargetPerTime);
            InvokeRepeating(nameof(OptimumAITurnDirection), 0.1f, turnDirectionPerTime);
        }
        void OptimumEnemyAI()
        {
            attackRangePosition = transform.GetChild(0).position;
            sightRangePosition = transform.GetChild(0).position;

            enemyAI.CatchNeraestTarget();
            enemyAI.StopWhenAttackDistance();
            enemyAttack.Attack();
        }

        void OptimumDetech()
        {
            playerUnits = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, targetUnits);
            if (enemyTypeEnum == EnemyTypeEnum.Barrel)
                playerBuildings = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, targetBuildings);
        }

        void OptimumAITurnDirection()
        {
            // ePF2D.pathLeftToGo[0]; hedefe giderken kullandığı yol
            if (enemyAI.DetechNearestTarget() == null) return;
            if (enemyTypeEnum == EnemyTypeEnum.Dynamite || enemyTypeEnum == EnemyTypeEnum.Barrel)
            {
                // Durduğunda hadefe bak
                if (ePF2D.isPathEnd)
                    direction.Turn2Direction(enemyAI.DetechNearestTarget().transform.position.x);

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

        }
    }
}
