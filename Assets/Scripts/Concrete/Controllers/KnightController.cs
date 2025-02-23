using Assets.Scripts.Concrete.AI;
using Assets.Scripts.Concrete.Combats;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using Assets.Scripts.Concrete.PowerStats;
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

        //Archer
        public float arrowSpeed = 25;
        public float arrowDestroyTime = 10;

        [Header("UNIT SETTÝNGS")]
        [Range(0.1f, 1f)] public float turnDirectionPerTime = 0.5f;
        [Range(0.1f, 1f)] public float aIPerTime = 0.5f;
        [Range(0.1f, 1f)] public float collectResourcesPerTime = 1f;
        public LayerMask enemiesLayer;
        [SerializeField] Transform orderInLayerSpriteAnchor;
        public SpriteRenderer visual;

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
        VillagerController villagerController;
        float time;
        DynamicOrderInLayer dynamicOrderInLayer;
        //WarriorStats warriorStats;
        //ArcherStats archerStats;
        //VillagerStats villagerStats;
        //HealthController healthController;
        private void Awake()
        {
            pF = GetComponent<PathFindingController>();
            knightCollider = GetComponent<CircleCollider2D>();
            animator = transform.GetChild(0).GetComponent<Animator>();
            animationEventController = transform.GetChild(0).GetComponent<AnimationEventController>();
            //healthController = GetComponent<HealthController>();
            direction = new(transform);
            knightAI = new(this, pF);
            new KnightAttack(this, knightAI, animationEventController, pF);
            dynamicOrderInLayer = new();
            if (factionType == FactionTypeEnum.Villager)
            {
                villagerController = GetComponent<VillagerController>();
                //villagerStats = GetComponent<VillagerStats>();
            }
            //if (factionType == FactionTypeEnum.Warrior)
            //    warriorStats = GetComponent<WarriorStats>();
            //if (factionType == FactionTypeEnum.Archer)
            //    archerStats = GetComponent<ArcherStats>();

        }
        private void Start()
        {
            //PowerStatsAssign();
            currentSightRange = sightRange;
            pF.agent.speed = moveSpeed;
            time = attackInterval;
            animationEventController.ResetAttackEvent += ResetAttack;
            // Invoke
            InvokeRepeating(nameof(OptimumTurnDirection), 0, turnDirectionPerTime);
            InvokeRepeating(nameof(OptimumAI), 0, aIPerTime);
        }

        //void PowerStatsAssign()
        //{
        //    if (warriorStats != null)
        //    {
        //        healthController.health = warriorStats.health;
        //        healthController.regenerationAmount = warriorStats.regenerationAmount;
        //        healthController.regrenationPerTime = warriorStats.regrenationPerTime;
        //        healthController.regrenationAfterDamageTime = warriorStats.regrenationAfterDamageTime;
        //        moveSpeed = warriorStats.moveSpeed;
        //        damage = warriorStats.damage;
        //        attackSpeed = warriorStats.attackSpeed;
        //        attackInterval = warriorStats.attackInterval;
        //        attackRange = warriorStats.attackRange;
        //        sightRange = warriorStats.sightRange;
        //    }
        //    if (archerStats !=null)
        //    {
        //        healthController.health = archerStats.health;
        //        healthController.regenerationAmount = archerStats.regenerationAmount;
        //        healthController.regrenationPerTime = archerStats.regrenationPerTime;
        //        healthController.regrenationAfterDamageTime = archerStats.regrenationAfterDamageTime;
        //        moveSpeed = archerStats.moveSpeed;
        //        damage = archerStats.damage;
        //        attackSpeed = archerStats.attackSpeed;
        //        attackInterval = archerStats.attackInterval;
        //        attackRange = archerStats.attackRange;
        //        sightRange = archerStats.sightRange;
        //        arrowSpeed = archerStats.arrowSpeed;
        //        arrowDestroyTime = archerStats.arrowDestroyTime;
        //    }
        //    if(villagerStats!=null)
        //    {
        //        healthController.health = villagerStats.health;
        //        healthController.regenerationAmount = villagerStats.regenerationAmount;
        //        healthController.regrenationPerTime = villagerStats.regrenationPerTime;
        //        healthController.regrenationAfterDamageTime = villagerStats.regrenationAfterDamageTime;
        //        moveSpeed = villagerStats.moveSpeed;
        //        damage = villagerStats.damage;
        //        attackSpeed = villagerStats.attackSpeed;
        //        attackInterval = villagerStats.attackInterval;
        //        attackRange = villagerStats.attackRange;
        //        sightRange = villagerStats.sightRange;
        //        villagerController.treeDamage = villagerStats.treeDamage;
        //        villagerController.chopSpeed = villagerStats.chopSpeed;
        //    }

        //}
        private void Update()
        {
            if (!onBuilding)
                dynamicOrderInLayer.OrderInLayerUpdate(orderInLayerSpriteAnchor, visual);
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
        void OptimumTurnDirection()
        {
            if (aI) //Knight AI
            {
                AITurnDirection();
            }
        }

        void OptimumAI()
        {
            knightAI.GoTower();
            if (aI)
            {
                knightAI.UnitBehaviours();
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
                if (knightAI.target != null)
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
            if (knightAI.target != null)
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
