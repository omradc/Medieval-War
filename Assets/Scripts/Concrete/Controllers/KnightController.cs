using Assets.Scripts.Concrete.AI;
using Assets.Scripts.Concrete.Combats;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using System.Security.Cryptography;
using UnityEngine;


namespace Assets.Scripts.Concrete.Controllers
{
    internal class KnightController : MonoBehaviour
    {
        [Header("UNIT TYPE")]
        public TroopTypeEnum troopType;
        public bool isSeleceted;

        [Header("WORRIOR AND VILLAGER")]
        Transform attackPoint;

        [Header("ARCHER")]
        public float arrowSpeed;
        [HideInInspector] public GameObject arrow;

        [Space(30)]
        [Header("UNIT")]
        [Range(0.1f, 2f)] public float moveSpeed = 1f;
        public int damage;
        public float attackSpeed;
        public float attackRange;
        public float sightRange;

        [Header("UNIT SETTÝNGS")]
        [Range(0.1f, 1f)] public float unitAIPerTime = 0.5f;
        [Range(0.1f, 1f)] public float collectResourcesPerTime = 1f;
        public LayerMask enemy;

        [HideInInspector] public Collider2D[] followTargets;
        [HideInInspector] public Collider2D[] hitTargets;
        [HideInInspector] public GameObject followingObj;
        [HideInInspector] public bool workOnce = true;
        [HideInInspector] public float currentStoppingDistance;
        [HideInInspector] public float currentSightRange;
        [HideInInspector] public UnitOrderEnum unitOrderEnum;
        [HideInInspector] public Vector2 attackRangePosition;
        [HideInInspector] public Vector2 sightRangePosition;
        [HideInInspector] public KnightAI knightAI;
        [HideInInspector] public Direction direction;
        [HideInInspector] public Animator animator;
        [HideInInspector] public int towerPosIndex;
        [HideInInspector] public bool aI = true;
        [HideInInspector] public bool onBuilding;
        [HideInInspector] public bool onBuildingStay;
        [HideInInspector] public bool goBuilding;
        [HideInInspector] public CircleCollider2D circleCollider;
        AnimationEventController animationEventController;
        PathFindingController pF;
        UnitAttack unitAttack;
        Rigidbody2D rb2D;
        public bool canAttack;

        private void Awake()
        {
            pF = GetComponent<PathFindingController>();
            direction = new Direction(transform);
            knightAI = new KnightAI(this, pF);
            animationEventController = transform.GetChild(0).GetComponent<AnimationEventController>();
        }
        private void Start()
        {
            unitAttack = new UnitAttack(this, knightAI, animationEventController, pF);
            currentSightRange = sightRange;
            currentStoppingDistance = attackRange;
            pF.agent.stoppingDistance = currentStoppingDistance;
            pF.agent.speed = moveSpeed;
            rb2D = GetComponent<Rigidbody2D>();
            animator = transform.GetChild(0).GetComponent<Animator>();
            circleCollider = GetComponent<CircleCollider2D>();
            attackPoint = transform.GetChild(1);

            // Invoke
            InvokeRepeating(nameof(OptimumUnitAI), 0.1f, unitAIPerTime);
        }
        private void Update()
        {
            AttackOn();
            AnimationControl();
            RangeControl();

            if (unitOrderEnum == UnitOrderEnum.FollowOrder)  // Sadece takip edilecek birim atamasý yapýlýr
            {
                // Hedef boþ ise, hedefi belirle, sadece 1 kez
                if (InteractManager.Instance.interactedKnight != null && workOnce)
                {
                    followingObj = InteractManager.Instance.interactedKnight;
                    workOnce = false;
                }
            }

            knightAI.SelectTower();
            //towerAI.DestructTower();
        }
        void OptimumUnitAI()
        {
            knightAI.GoTower();
            if (aI) //Knight AI
            {
                DetechEnemies();
                AITurnDirection();
                knightAI.CatchNeraestTarget();
                knightAI.UnitBehaviours();
            }

        }
        void DetechEnemies()
        {
            followTargets = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, enemy);
        }
        void AITurnDirection()
        {
            // Durduysan, hedefe yönel.
            if (pF.isStopped)
            {
                // Hedefte düþman varsa;
                if (knightAI.nearestTarget != null)
                {
                    if (troopType == TroopTypeEnum.Villager)
                        direction.Turn2DirectionWithPos(knightAI.nearestAttackPoint.position.x);
                    if (troopType == TroopTypeEnum.Archer)
                        direction.Turn8Direction(knightAI.nearestAttackPoint.position);
                    if (troopType == TroopTypeEnum.Worrior)
                        direction.Turn4Direction(knightAI.nearestAttackPoint.position);
                }

                // Hedefte düþman yoksa;
                else
                {
                    transform.localScale = Vector3.one;
                }

            }

        }
        void AnimationControl()
        {
            if (canAttack)
            {
                //Animasyonlar, saldýrýlarý event ile tetikler ve yöne göre animasyonlar oynatýlýr.
                if (direction.right || direction.left)
                    AnimationManager.Instance.AttackFrontAnim(animator, attackSpeed);
                else if (direction.up)
                    AnimationManager.Instance.AttackUpAnim(animator, attackSpeed);
                else if (direction.down)
                    AnimationManager.Instance.AttackDownAnim(animator, attackSpeed);
                else if (direction.upRight || direction.upLeft)
                    AnimationManager.Instance.AttackUpFrontAnim(animator, attackSpeed);
                else if (direction.downRight || direction.downLeft)
                    AnimationManager.Instance.AttackDownFrontAnim(animator, attackSpeed);
            }
            else
            {
                // Aðaç kesme ve kaynak taþýma animasyonu yapmýyorsa, koþabilir veya durabilir
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Chop_Sheep") || animator.GetCurrentAnimatorStateInfo(0).IsName("Chop_Wood") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Run_0")) return;
                if (pF.isStopped)  // Durduysan = IdleAnim
                    AnimationManager.Instance.IdleAnim(animator);
                if (!pF.isStopped) // Durmadýysan = RunAnim
                    AnimationManager.Instance.RunAnim(animator, 1);

            }
        }
        void RangeControl()
        {
            attackRangePosition = transform.GetChild(0).position;

            if (!aI)
                sightRangePosition = transform.GetChild(0).position;

            if (pF.isUserControl || knightAI.nearestTarget == null || goBuilding)
                currentStoppingDistance = 0;

            else
                currentStoppingDistance = attackRange;

            pF.agent.stoppingDistance = currentStoppingDistance;
        }
        void AttackOn()
        {
            // Düþman varsa ve saldýrý menzilindeyse, saldýrý aktifleþir
            if (knightAI.nearestTarget != null)
            {
                if (Vector2.Distance(attackRangePosition, knightAI.nearestAttackPoint.position) < attackRange && !pF.isUserControl && !goBuilding)
                    canAttack = true;
                else
                    canAttack = false;
            }

            else
                canAttack = false;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(sightRangePosition, currentSightRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackRangePosition, attackRange);
        }
    }
}
