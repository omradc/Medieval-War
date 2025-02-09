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
        public FactionTypeEnum factionType;

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
        public float longRange;
        public float sightRange;
        public float attackRange;
        public float reportRange;

        [Header("ENEMY SETTİNGS")]
        [Range(0.1f, 1f)] public float enemyAIPerTime = 0.5f;
        public LayerMask enemiesLayer;
        public LayerMask friendsLayer;
        public LayerMask targetWoodTower;

        [Header("PATROLLİNG")]
        public float patrollingRadius;
        public float waitingTime;
        public BehaviorEnum behavior;
        public Transform path;

        [HideInInspector] public bool aI = true;
        [HideInInspector] public bool onBuilding;
        [HideInInspector] public bool goBuilding;
        [HideInInspector] public float currentSightRange;
        public float currentLongRange;
        [HideInInspector] public Collider2D[] woodTowers;
        public Collider2D[] sightRangeDetechEnemies;
        public Collider2D[] longRangeDetechEnemies;
        public Collider2D[] friendsDetech;
        public GameObject nonRangeDetechEnemy;
        [HideInInspector] public Transform[] patrolPoints;
        [HideInInspector] public GameObject explosion;
        [HideInInspector] public GameObject dynamite;
        [HideInInspector] public Direction direction;
        [HideInInspector] public Animator animator;
        [HideInInspector] public CircleCollider2D goblinCollider;


        public bool canAttack;
        float time;
        EnemyAttack enemyAttack;
        GoblinAI goblinAI;
        PathFindingController pF;
        AnimationEventController animationEventController;
        Vector3 gizmosPos;
        private void Awake()
        {
            animator = transform.GetChild(0).GetComponent<Animator>();
            goblinCollider = GetComponent<CircleCollider2D>();
            pF = GetComponent<PathFindingController>();
            direction = new Direction(transform);
            goblinAI = new(this, pF);
            animationEventController = transform.GetChild(0).GetComponent<AnimationEventController>();
        }
        private void Start()
        {
            enemyAttack = new(this, goblinAI, animationEventController, pF);
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
            goblinAI.DestructTower();
        }
        void OptimumEnemyAI()
        {
            if (aI)
            {
                DetechEnemies();
                AITurnDirection();
                goblinAI.CatchNeraestTarget();
                goblinAI.GoblinBehaviour();
                ResetPath();
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
            if (goblinAI.target != null && pF.isStoping)
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
                if (pF.isStoping) // Durduysan = IdleAnim
                    AnimationManager.Instance.IdleAnim(animator);
                if (!pF.isStoping)                           // Durmadıysan = RunAnim
                    AnimationManager.Instance.RunAnim(animator, 1);

            }
        }
        void AttackOn()
        {
            // Düşman varsa ve saldırı menzilindeyse, saldırı aktifleşir
            if (goblinAI.target != null)
            {
                if (Vector2.Distance(transform.position, goblinAI.nearestAttackPoint.position) < attackRange)
                    canAttack = true;
                else
                    canAttack = false;
            }

            else
                canAttack = false;
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
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, currentSightRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(gizmosPos, patrollingRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, currentLongRange);

            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, reportRange);
        }
    }
}
