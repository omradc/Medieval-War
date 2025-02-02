using Assets.Scripts.Concrete.AI;
using Assets.Scripts.Concrete.Combats;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;


namespace Assets.Scripts.Concrete.Controllers
{
    internal class KnightController : MonoBehaviour
    {
        [Header("UNIT")]
        public FactionTypeEnum factionType;
        [Space(30)]
        [Header("UNIT")]
        [Range(2, 4f)] public float moveSpeed = 1f;
        public int damage;
        public float attackSpeed;
        public float attackInterval;
        public float attackRange;
        public float sightRange;

        [Header("UNIT SETTÝNGS")]
        [Range(0.1f, 1f)] public float unitAIPerTime = 0.5f;
        [Range(0.1f, 1f)] public float collectResourcesPerTime = 1f;
        public LayerMask enemiesLayer;

        [HideInInspector] public bool isSeleceted;
        [HideInInspector] public Collider2D[] targetEnemies;
        [HideInInspector] public GameObject arrow;
        [HideInInspector] public GameObject followingObj;
        [HideInInspector] public bool workOnce = true;
        [HideInInspector] public float currentSightRange;
        [HideInInspector] public KnightOrderEnum unitOrderEnum;
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
        [HideInInspector] public bool canAttack;
        [HideInInspector] public CircleCollider2D knightCollider;
        AnimationEventController animationEventController;
        PathFindingController pF;
        KnightAttack knightAttack;
        Rigidbody2D rb2D;
        VillagerController villagerController;
        float time;

        private void Awake()
        {
            pF = GetComponent<PathFindingController>();
            direction = new Direction(transform);
            knightAI = new KnightAI(this, pF);
            animationEventController = transform.GetChild(0).GetComponent<AnimationEventController>();
            rb2D = GetComponent<Rigidbody2D>();
            knightCollider = GetComponent<CircleCollider2D>();
            animator = transform.GetChild(0).GetComponent<Animator>();
            knightAttack = new KnightAttack(this, knightAI, animationEventController, pF);
            if (factionType == FactionTypeEnum.Villager)
                villagerController = GetComponent<VillagerController>();
        }
        private void Start()
        {
            currentSightRange = sightRange;
            pF.agent.speed = moveSpeed;
            time = attackInterval;
            animationEventController.ResetAttackEvent += ResetAttack;
            // Invoke
            InvokeRepeating(nameof(OptimumUnitAI), 0, unitAIPerTime);
        }
        private void Update()
        {
            AttackOn();
            AnimationControl();
            RangeControl();

            if (unitOrderEnum == KnightOrderEnum.FollowOrder)  // Sadece takip edilecek birim atamasý yapýlýr
            {
                // Hedef boþ ise, hedefi belirle, sadece 1 kez
                if (InteractManager.Instance.interactedKnight != null && workOnce)
                {
                    followingObj = InteractManager.Instance.interactedKnight;
                    workOnce = false;
                }
            }

            knightAI.SelectTower();
            knightAI.DestructTower();
        }
        void OptimumUnitAI()
        {
            knightAI.GoTower();
            if (aI) //Knight AI
            {
                knightAI.UnitBehaviours();
                AITurnDirection();
                DetechEnemies();
                knightAI.CatchNeraestTarget();
                ResetPath();
            }

        }
        void DetechEnemies()
        {
            targetEnemies = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, enemiesLayer);
        }
        void AITurnDirection()
        {
            // Durduysan, hedefe yönel.
            if (pF.isStoping)
            {
                // Hedefte düþman varsa;
                if (knightAI.nearestTarget != null)
                {
                    if (factionType == FactionTypeEnum.Villager)
                        direction.Turn2DirectionWithPos(knightAI.nearestAttackPoint.position.x);
                    if (factionType == FactionTypeEnum.Archer)
                        direction.Turn8Direction(knightAI.nearestAttackPoint.position);
                    if (factionType == FactionTypeEnum.Warrior)
                        direction.Turn4Direction(knightAI.nearestAttackPoint.position);
                }

                // hedefte düþman yoksa;
                else
                {
                    // Köylü deðilse
                    if (factionType != FactionTypeEnum.Villager)
                        transform.localScale = Vector3.one;

                    // Köylü ise
                    else
                    {
                        // Köylü çalýþmýyorsa, saða bak
                        if (villagerController.targetResource == null)
                            transform.localScale = Vector3.one;
                        // Köylü çalýþýyorsa, hedefe bakar
                        else
                        {
                            direction.Turn2DirectionWithPos(villagerController.targetResource.transform.position.x);
                        }
                    }
                }

            }

        }
        void AnimationControl()
        {
            if (canAttack)
            {
                time += Time.deltaTime;
                if (time >= attackInterval)
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
                    // time = 0; // Animasyon bittiðinde süre sýfýrlanýr.
                }

                else
                    AnimationManager.Instance.IdleAnim(animator);

            }
            else
            {
                // Aðaç kesme ve kaynak taþýma animasyonu yapmýyorsa, koþabilir veya durabilir
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Chop_Sheep") || animator.GetCurrentAnimatorStateInfo(0).IsName("Chop_Wood") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Run_0") || animator.GetCurrentAnimatorStateInfo(0).IsName("Build")) return;
                if (pF.isStoping)  // Durduysan = IdleAnim
                    AnimationManager.Instance.IdleAnim(animator);
                if (!pF.isStoping) // Durmadýysan = RunAnim
                    AnimationManager.Instance.RunAnim(animator, 1);

            }
        }
        void RangeControl()
        {
            attackRangePosition = transform.GetChild(0).position;

            if (!aI)
                sightRangePosition = transform.GetChild(0).position;
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
        void ResetPath()
        {
            if (targetEnemies.Length == 0 && pF.agent.velocity.magnitude < 0.1f && pF.agent.velocity.magnitude > 0)
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
        }
    }
}
