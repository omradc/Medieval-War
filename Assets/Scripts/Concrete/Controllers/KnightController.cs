using Assets.Scripts.Concrete.Combats;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using Assets.Scripts.Concrete.Orders;
using Assets.Scripts.Concrete.UnitAIs;
using UnityEngine;


namespace Assets.Scripts.Concrete.Controllers
{
    internal class KnightController : MonoBehaviour
    {
        public bool attack;
        [Header("UNIT TYPE")]
        public UnitTypeEnum unitTypeEnum;
        public bool isSeleceted;

        [Header("WORRIOR AND VILLAGER")]
        Transform attackPoint;

        [Header("Archer")]
        public float arrowSpeed;
        [HideInInspector] public GameObject arrow;


        [Space(30)]
        [Header("UNIT")]
        public int damage;
        [Range(0.1f, 2f)] public float moveSpeed = 1f;
        public float attackSpeed;
        public float attackRange;
        public float sightRange;
        [Header("UNIT SETTÝNGS")]
        [Range(0.1f, 1f)] public float unitAIPerTime = 0.5f;
        [Range(0.1f, 1f)] public float detechTargetPerTime = 0.5f;
        [Range(0.1f, 1f)] public float turnDirectionPerTime = 0.5f;
        [Range(0.1f, 1f)] public float collectResourcesPerTime = 1f;
        public LayerMask enemy;

        public Collider2D[] followTargets;
        [HideInInspector] public Collider2D[] hitTargets;

        [HideInInspector] public GameObject followingObj;
        [HideInInspector] public bool workOnce = true;
        [HideInInspector] public int currentDamage;
        [HideInInspector] public float currentMoveSpeed;
        [HideInInspector] public float currentAttackSpeed;
        public float currentAttackRange;
        [HideInInspector] public float currentSightRange;
        [HideInInspector] public float currentArrowSpeed;
        [HideInInspector] public UnitOrderEnum unitOrderEnum;
        [HideInInspector] public Vector2 attackRangePosition;
        [HideInInspector] public Vector2 sightRangePosition;
        [HideInInspector] public UnitAI unitAI;
        [HideInInspector] public Direction direction;
        [HideInInspector] public Animator animator;
        [HideInInspector] public int towerPosIndex;
        [HideInInspector] public bool aI = true;
        [HideInInspector] public bool onBuilding;
        [HideInInspector] public bool stayBuilding;
        [HideInInspector] public bool goBuilding;


        [HideInInspector] public CircleCollider2D circleCollider;
        AnimationEventController animationEventController;
        // UnitPathFinding2D pF2D;
        PathFindingController pF;
        AttackAI attackAI;
        DefendAI defendAI;
        StayAI stayAI;
        FollowAI followAI;
        UnitAttack unitAttack;
        TowerAI towerAI;
        Rigidbody2D rb2D;

        private void Awake()
        {
            //pF2D = GetComponent<UnitPathFinding2D>();
            pF = GetComponent<PathFindingController>();
            direction = new Direction(transform);
            unitAI = new UnitAI(this, pF);
            attackAI = new AttackAI(this, pF);
            defendAI = new DefendAI(this, pF);
            stayAI = new StayAI(this, pF);
            followAI = new FollowAI(this, pF);
            towerAI = new TowerAI(this, pF);
            animationEventController = transform.GetChild(0).GetComponent<AnimationEventController>();
        }
        private void Start()
        {
            unitAttack = new UnitAttack(this, unitAI, animationEventController);
            currentDamage = damage;
            currentAttackSpeed = attackSpeed;
            currentSightRange = sightRange;
            currentAttackRange = attackRange;
            currentMoveSpeed = moveSpeed;
            pF.agent.stoppingDistance = currentAttackRange;
            pF.agent.speed = currentMoveSpeed;
            currentArrowSpeed = arrowSpeed;
            rb2D = GetComponent<Rigidbody2D>();
            animator = transform.GetChild(0).GetComponent<Animator>();
            circleCollider = GetComponent<CircleCollider2D>();
            attackPoint = transform.GetChild(1);

            // Invoke
            InvokeRepeating(nameof(OptimumUnitAI), 0.1f, unitAIPerTime);
            InvokeRepeating(nameof(OptimumDetechEnemies), .5f, detechTargetPerTime);
            InvokeRepeating(nameof(OptimumAITurnDirection), 0.1f, turnDirectionPerTime);

        }
        private void Update()
        {
            AnimationControl();

            if (unitOrderEnum == UnitOrderEnum.FollowOrder)  // Sadece takip edilecek birim atamasý yapýlýr
                followAI.SetFollowUnit();

            towerAI.SelectTower();
            //towerAI.DestructTower();
        }
        void OptimumUnitAI()
        {
            RangeControl();

            //Unit AI
            if (aI)
            {
                unitAI.CatchNeraestTarget();
                if (unitOrderEnum == UnitOrderEnum.AttackOrder)
                    attackAI.AttackMode();
                if (unitOrderEnum == UnitOrderEnum.DefendOrder)
                    defendAI.DefendMode();
                if (unitOrderEnum == UnitOrderEnum.StayOrder)
                    stayAI.StaticMode();
                if (unitOrderEnum == UnitOrderEnum.FollowOrder)
                    followAI.FollowMode();
            }
            //unitAI.RigidbodyControl(rb2D, stayBuilding);
            //towerAI.GoTower();
        }
        void OptimumDetechEnemies()
        {
            followTargets = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, enemy);
        }
        void OptimumAITurnDirection()
        {
            // Oyuncunun hareket emri her zaman önceliklidir || Sadece düþman varsa çalýþýr

            // Hedefte düþman varsa ve durduysan, hedefe yönel.
            if (unitAI.nearestTarget != null && !pF.isUserControl)
            {
                if (unitTypeEnum == UnitTypeEnum.Villager)
                    direction.Turn2DirectionWithPos(unitAI.nearestAttackPoint.position.x);
                if (unitTypeEnum == UnitTypeEnum.Archer)
                    direction.Turn8Direction(unitAI.nearestAttackPoint.position);
                if (unitTypeEnum == UnitTypeEnum.Worrior)
                    direction.Turn4Direction(unitAI.nearestAttackPoint.position);
            }

        }
        void AnimationControl()
        {
            AttackOn();

            if (attack)
            {
                //Animasyonlar, saldýrýlarý event ile tetikler
                if (direction.right || direction.left)
                    AnimationManager.Instance.AttackFrontAnim(animator, currentAttackSpeed);
                if (direction.up)
                    AnimationManager.Instance.AttackUpAnim(animator, currentAttackSpeed);
                if (direction.down)
                    AnimationManager.Instance.AttackDownAnim(animator, currentAttackSpeed);
                if (direction.upRight || direction.upLeft)
                    AnimationManager.Instance.AttackUpFrontAnim(animator, currentAttackSpeed);
                if (direction.downRight || direction.downLeft)
                    AnimationManager.Instance.AttackDownFrontAnim(animator, currentAttackSpeed);
            }
            else
            {
                if (pF.isStopped) // Durduysan = IdleAnim
                    AnimationManager.Instance.IdleAnim(animator);
                if (!pF.isStopped)                           // Durmadýysan = RunAnim
                    AnimationManager.Instance.RunAnim(animator, 1);

            }
        }
        void RangeControl()
        {
            attackRangePosition = transform.GetChild(0).position;

            if (pF.isUserControl || unitAI.nearestTarget == null)
                currentAttackRange = 0;
            else
                currentAttackRange = attackRange;
            pF.agent.stoppingDistance = currentAttackRange;
        }

        void AttackOn()
        {
            // Düþman varse ve saldýrý menzilindeyse, yöne göre animasyonlar oynatýlýr.
            if (unitAI.nearestTarget != null)
            {
                if (Vector2.Distance(attackRangePosition, unitAI.nearestAttackPoint.position) < currentAttackRange)
                    attack = true;
                else
                    attack = false;
            }

            else
                attack = false;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(sightRangePosition, currentSightRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackRangePosition, currentAttackRange);
        }
    }
}
