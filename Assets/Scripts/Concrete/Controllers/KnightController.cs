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
        [Header("UNIT TYPE")]
        public UnitTypeEnum unitTypeEnum;
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
        public Collider2D[] followTargets;

        [HideInInspector] public Collider2D[] hitTargets;
        public GameObject followingObj;
        [HideInInspector] public bool workOnce = true;
        [HideInInspector] public float currentStoppingDistance;
        [HideInInspector] public float currentSightRange;
        public UnitOrderEnum unitOrderEnum;
        [HideInInspector] public bool attack;
        [HideInInspector] public Vector2 attackRangePosition;
        [HideInInspector] public Vector2 sightRangePosition;
        [HideInInspector] public UnitAI unitAI;
        [HideInInspector] public Direction direction;
        [HideInInspector] public Animator animator;
        [HideInInspector] public int towerPosIndex;
         public bool aI = true;
        [HideInInspector] public bool onBuilding;
        [HideInInspector] public bool onBuildingStay;
        [HideInInspector] public bool goBuilding;
        [HideInInspector] public CircleCollider2D circleCollider;

        AnimationEventController animationEventController;
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

            towerAI.SelectTower();
            //towerAI.DestructTower();
        }
        void OptimumUnitAI()
        {
            
            towerAI.GoTower();

            if (aI) //Knight AI
            {
                DetechEnemies();
                AITurnDirection();
                unitAI.CatchNeraestTarget();
                unitAI.UnitBehaviours();
            }

        }
        void DetechEnemies()
        {
            followTargets = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, enemy);
        }
        void AITurnDirection()
        {
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
                //Animasyonlar, saldýrýlarý event ile tetikler ve yöne göre animasyonlar oynatýlýr.
                if (direction.right || direction.left)
                    AnimationManager.Instance.AttackFrontAnim(animator, attackSpeed);
                if (direction.up)
                    AnimationManager.Instance.AttackUpAnim(animator, attackSpeed);
                if (direction.down)
                    AnimationManager.Instance.AttackDownAnim(animator, attackSpeed);
                if (direction.upRight || direction.upLeft)
                    AnimationManager.Instance.AttackUpFrontAnim(animator, attackSpeed);
                if (direction.downRight || direction.downLeft)
                    AnimationManager.Instance.AttackDownFrontAnim(animator, attackSpeed);
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

            if (!aI)
                sightRangePosition = transform.GetChild(0).position;

            if (unitOrderEnum != UnitOrderEnum.FollowOrder)
            {
                if (pF.isUserControl || unitAI.nearestTarget == null || goBuilding)
                    currentStoppingDistance = 0;
            }
            else
                currentStoppingDistance = attackRange;

            pF.agent.stoppingDistance = currentStoppingDistance;
        }
        void AttackOn()
        {
            // Düþman varse ve saldýrý menzilindeyse, saldýrý aktifleþir
            if (unitAI.nearestTarget != null)
            {
                if (Vector2.Distance(attackRangePosition, unitAI.nearestAttackPoint.position) < attackRange && !pF.isUserControl && !goBuilding)
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
            Gizmos.DrawWireSphere(attackRangePosition, attackRange);
        }
    }
}
