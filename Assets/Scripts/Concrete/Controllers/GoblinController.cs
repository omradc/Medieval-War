using Assets.Scripts.Concrete.Combats;
using Assets.Scripts.Concrete.AI;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;
using Assets.Scripts.Concrete.Managers;
using System;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class GoblinController : MonoBehaviour
    {
        [Header("ENEMY")]
        public TroopTypeEnum troopType;

        [Header("DYNAMİTE")]
        public float dynamiteSpeed;
        public float dynamiteExplosionRadius = .5f;

        [Header("BARREL")]
        public float barrelExplosionRadius = 2;

        [Header("ENEMY UNİT")]
        [Range(2f, 4f)] public float moveSpeed;
        public int damage;
        public float attackSpeed;
        public float attackInterveal;
        public float largeRange;
        public float sightRange;
        public float attackRange;

        [Header("ENEMY SETTİNGS")]
        [Range(0.1f, 1f)] public float enemyAIPerTime = 0.5f;
        public LayerMask enemiesLayer;
        public LayerMask targetWoodTower;

        [Header("PATROLLİNG")]
        public float patrollingRadius;
        public float waitingTime;
        public BehaviorEnum behavior;
        public Transform path;

        [HideInInspector] public bool attackTheAllKnights;
        [HideInInspector] public bool aI = true;
        [HideInInspector] public bool onBuilding;
        [HideInInspector] public bool goBuilding;
        [HideInInspector] public float currentSightRange;
        [HideInInspector] public Vector2 sightRangePosition;
        [HideInInspector] public Vector2 attackRangePosition;
        [HideInInspector] public Collider2D[] woodTowers;
        [HideInInspector] public Collider2D[] targetEenemies;
        [HideInInspector] public Transform[] patrolPoints;
        [HideInInspector] public GameObject explosion;
        [HideInInspector] public GameObject dynamite;
        [HideInInspector] public Direction direction;
        [HideInInspector] public Animator animator;
        [HideInInspector] public CircleCollider2D goblinCollider;


        bool canAttack;
        float time;
        EnemyAttack enemyAttack;
        GoblinAI enemyAI;
        PathFindingController pF;
        AnimationEventController animationEventController;
        Vector3 gizmosPos;
        private void Awake()
        {
            animator = transform.GetChild(0).GetComponent<Animator>();
            goblinCollider = GetComponent<CircleCollider2D>();
            pF = GetComponent<PathFindingController>();
            direction = new Direction(transform);
            enemyAI = new(this, pF);
            animationEventController = transform.GetChild(0).GetComponent<AnimationEventController>();
        }
        private void Start()
        {
            enemyAttack = new(this, enemyAI, animationEventController, pF);
            pF.agent.speed = moveSpeed;
            currentSightRange = sightRange;
            gizmosPos = transform.position;
            time = attackInterveal;
            animationEventController.ResetAttackEvent += ResetAttack;
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
                ResetPath();
            }

            enemyAI.GoUpToTower();
        }
        void DetechEnemies()
        {
            targetEenemies = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, enemiesLayer);

            if (troopType == TroopTypeEnum.Tnt)
                woodTowers = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, targetWoodTower);
        }
        void AITurnDirection()
        {
            // Hedefte düşman varsa ve durduysan, hedefe yönel.
            if (enemyAI.nearestTarget != null && pF.isStoping)
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
        void RangeControl()
        {
            attackRangePosition = transform.GetChild(0).position;

            if (!aI)
                sightRangePosition = transform.GetChild(0).position;
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
                if (pF.isStoping) // Durduysan = IdleAnim
                    AnimationManager.Instance.IdleAnim(animator);
                if (!pF.isStoping)                           // Durmadıysan = RunAnim
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
        void ResetPath()
        {
            if (targetEenemies.Length == 0 && pF.agent.velocity.magnitude < 0.1f && pF.agent.velocity.magnitude > 0)
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
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(sightRangePosition, currentSightRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackRangePosition, attackRange);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(gizmosPos, patrollingRadius);
        }
    }
}
