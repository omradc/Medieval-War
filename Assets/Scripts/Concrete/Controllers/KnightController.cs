using Assets.Scripts.Concrete.AI;
using Assets.Scripts.Concrete.Combats;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using Assets.Scripts.Concrete.PowerStats;
using UnityEngine;


namespace Assets.Scripts.Concrete.Controllers
{
    [RequireComponent(typeof(PathFinding))]
    internal class KnightController : MonoBehaviour
    {
        [Header("SETTÝNGS")]
        public FactionTypeEnum factionType;
        [Range(0.1f, 1f)] public float turnDirectionPerTime = 0.5f;
        [Range(0.1f, 1f)] public float aIPerTime = 0.5f;
        [Range(0.1f, 1f)] public float collectResourcesPerTime = 1f;
        public LineRenderer drawSightRange;
        public LineRenderer drawAttackRange;
        public LineRenderer drawPath;
        public GameObject targetImage;
        public Transform orderInLayerSpriteAnchor;
        public SpriteRenderer visual;
        public LayerMask enemiesLayer;
        [HideInInspector] public int lineWidth;
        [HideInInspector] public int damage;
        [HideInInspector] public float moveSpeed;
        [HideInInspector] public int towerPosIndex;
        [HideInInspector] public float attackRange;
        [HideInInspector] public float currentAttackRange;
        [HideInInspector] public float sightRange;
        [HideInInspector] public float currentSightRange;
        [HideInInspector] public float arrowSpeed;
        [HideInInspector] public float arrowDestroyTime;
        [HideInInspector] public float followDistance;
        [HideInInspector] public bool isSeleceted;
        [HideInInspector] public bool aI = true;
        [HideInInspector] public bool onBuilding;
        [HideInInspector] public bool onBuildingStay;
        [HideInInspector] public bool goBuilding;
        [HideInInspector] public bool workOnce = true;
        [HideInInspector] public KnightOrderEnum unitOrderEnum;
        [HideInInspector] public Vector2 attackRangePosition;
        [HideInInspector] public Vector2 sightRangePosition;
        [HideInInspector] public KnightAI knightAI;
        [HideInInspector] public Direction direction;
        [HideInInspector] public Collider2D[] targetEnemies;
        [HideInInspector] public CircleCollider2D knightCollider;
        [HideInInspector] public GameObject arrow;
        [HideInInspector] public GameObject followingObj;
        [HideInInspector] public Camera cam;
        Animator animator;
        AnimationEventController animationEventController;
        PathFinding pF;
        PawnController pawnController;
        DynamicOrderInLayer dynamicOrderInLayer;
        WarriorStats warriorStats;
        ArcherStats archerStats;
        VillagerStats villagerStats;
        HealthController healthController;
        TargetPriority targetPriority;
        float attackInterval;
        float attackSpeed;
        float time;
        bool canAttack;
        bool obstacle;
        bool height;
        RaycastHit2D hitObj;
        private void Awake()
        {
            pF = GetComponent<PathFinding>();
            knightCollider = GetComponent<CircleCollider2D>();
            animator = transform.GetChild(0).GetComponent<Animator>();
            animationEventController = transform.GetChild(0).GetComponent<AnimationEventController>();
            healthController = GetComponent<HealthController>();
            direction = new(transform);
            knightAI = new(this, pF);
            new KnightAttack(this, knightAI, animationEventController, pF);
            dynamicOrderInLayer = new();
            cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            targetPriority = GetComponent<TargetPriority>();
            if (factionType == FactionTypeEnum.Pawn)
                pawnController = GetComponent<PawnController>();
            Instantiate(targetImage, transform.position, Quaternion.identity);
        }
        private void Start()
        {
            PowerStatsAssign();
            lineWidth = 500;
            pF.agent.speed = moveSpeed;
            currentSightRange = sightRange;
            currentAttackRange = attackRange;
            time = attackInterval;
            animationEventController.ResetAttackEvent += ResetAttack;

            // Invoke
            InvokeRepeating(nameof(OptimumTurnDirection), 0, turnDirectionPerTime);
            InvokeRepeating(nameof(OptimumAI), 0, aIPerTime);
        }
        void PowerStatsAssign()
        {
            if (factionType == FactionTypeEnum.Warrior)
                warriorStats = GetComponent<WarriorStats>();
            if (factionType == FactionTypeEnum.Archer)
                archerStats = GetComponent<ArcherStats>();
            if (factionType == FactionTypeEnum.Pawn)
                villagerStats = GetComponent<VillagerStats>();

            if (warriorStats != null)
            {
                healthController.health = warriorStats.health;
                healthController.regeneration = warriorStats.regrenation;
                healthController.regenerationAmount = warriorStats.regenerationAmount;
                healthController.regrenationPerTime = warriorStats.regrenationPerTime;
                healthController.regrenationAfterDamageTime = warriorStats.regrenationAfterDamageTime;
                moveSpeed = warriorStats.moveSpeed;
                damage = warriorStats.damage;
                attackSpeed = warriorStats.attackSpeed;
                attackInterval = warriorStats.attackInterval;
                attackRange = warriorStats.attackRange;
                sightRange = warriorStats.sightRange;
                followDistance = warriorStats.followDistance;
                targetPriority.priority = warriorStats.priority;
                targetPriority.maxAttacker = warriorStats.maxAttacker;
            }
            if (archerStats != null)
            {
                healthController.health = archerStats.health;
                healthController.regeneration = archerStats.regrenation;
                healthController.regenerationAmount = archerStats.regenerationAmount;
                healthController.regrenationPerTime = archerStats.regrenationPerTime;
                healthController.regrenationAfterDamageTime = archerStats.regrenationAfterDamageTime;
                moveSpeed = archerStats.moveSpeed;
                damage = archerStats.damage;
                attackSpeed = archerStats.attackSpeed;
                attackInterval = archerStats.attackInterval;
                attackRange = archerStats.attackRange;
                sightRange = archerStats.sightRange;
                arrowSpeed = archerStats.arrowSpeed;
                arrowDestroyTime = archerStats.arrowDestroyTime;
                followDistance = archerStats.followDistance;
                targetPriority.priority = archerStats.priority;
                targetPriority.maxAttacker = archerStats.maxAttacker;
            }
            if (villagerStats != null)
            {
                healthController.health = villagerStats.health;
                healthController.regeneration = villagerStats.regrenation;
                healthController.regenerationAmount = villagerStats.regenerationAmount;
                healthController.regrenationPerTime = villagerStats.regrenationPerTime;
                healthController.regrenationAfterDamageTime = villagerStats.regrenationAfterDamageTime;
                moveSpeed = villagerStats.moveSpeed;
                damage = villagerStats.damage;
                attackSpeed = villagerStats.attackSpeed;
                attackInterval = villagerStats.attackInterval;
                attackRange = villagerStats.attackRange;
                sightRange = villagerStats.sightRange;
                pawnController.treeDamage = villagerStats.treeDamage;
                pawnController.chopSpeed = villagerStats.chopSpeed;
                followDistance = villagerStats.followDistance;
                targetPriority.priority = villagerStats.priority;
                targetPriority.maxAttacker = villagerStats.maxAttacker;
            }

        }
        private void Update()
        {
            if (!UIManager.Instance.drawRangeToggle.isOn)
            {
                drawSightRange.positionCount = 0;
                drawAttackRange.positionCount = 0;
            }
            if (!pF.isStopping)
                targetImage.transform.position = pF.targetPos;
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
            knightAI.DynamicLineRendererWidthness();
            knightAI.GoTower();
            knightAI.DrawPath(pF.agent.path, drawPath, targetImage);
            if (aI)
            {
                knightAI.UnitBehaviours();
                DetechEnemies();
                knightAI.CatchNeraestTarget();
                //ResetPath();
                ObsticleControl();
            }
        }
        void DetechEnemies()
        {
            targetEnemies = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, enemiesLayer);
        }
        void AITurnDirection()
        {
            // Durduysan, hedefe yönel.
            if (pF.isStopping)
            {
                // Hedefte düþman varsa;
                if (knightAI.target != null)
                {
                    if (factionType == FactionTypeEnum.Pawn)
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
                    if (factionType != FactionTypeEnum.Pawn)
                        transform.localScale = Vector3.one;

                    // Köylü ise
                    else
                    {
                        // Köylü çalýþmýyorsa, saða bak
                        if (pawnController.targetResource == null)
                            transform.localScale = Vector3.one;
                        // Köylü çalýþýyorsa, hedefe bakar
                        else
                        {
                            direction.Turn2DirectionWithPos(pawnController.targetResource.transform.position.x);
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
                if (pF.isMovementStopping)  // Durduysan = IdleAnim
                    AnimationManager.Instance.IdleAnim(animator);
                if (!pF.isMovementStopping) // Durmadýysan = RunAnim
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
                float enemyDistance = Vector2.Distance(attackRangePosition, knightAI.nearestAttackPoint.position);
                if (enemyDistance < attackRange && !pF.isUserControl && !goBuilding) // Saldýrabilir
                {
                    if (enemyDistance > 1) // DÜþan çok yakýnsa engel tanýma
                    {
                        if (onBuilding) // Kule üstündeyse hiçbir yükselti veya engel atýcýyý engellemez
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
            if (knightAI.target == null) return;

            // Yükselti kontrolü
            if (healthController.elevationFloor >= knightAI.target.GetComponent<HealthController>().elevationFloor)
                height = false;
            else
                height = true;

            // Engel kontrolü
            hitObj = Physics2D.Raycast(transform.position, (knightAI.nearestAttackPoint.position - transform.position).normalized, attackRange, LayerMask.GetMask("Goblin", "Wall", "House", "Tower", "WoodTower", "WoodTowerFull", "GoblinHouse"));
            if (hitObj.collider != null)
            {
                if (knightAI.target.layer == hitObj.collider.gameObject.layer) // Hedefi engel olabilecek bir obje ise artýk engel deðildir
                    obstacle = false;
                else // Engel
                    obstacle = true;
            }
            else
                obstacle = false;

            if (obstacle || height) // Arada engel veya yükselti varsa, 
                currentAttackRange = 0;
            if (!obstacle && !height && currentAttackRange != attackRange) // Arada engel ve yükselti yoksa ve sadece 1 kez çalýþ,
            {
                currentAttackRange = attackRange;
                pF.agent.ResetPath();
            }
        }
        void ResetPath() // SOn düþmaný öldürdüðü noktaya gitmesini engellemek için
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
            Gizmos.color = Color.white;
            if (knightAI != null)
                if (knightAI.target != null)
                    Gizmos.DrawRay(transform.position, (knightAI.nearestAttackPoint.position - transform.position).normalized * attackRange);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, currentSightRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, currentAttackRange);
        }
    }
}
