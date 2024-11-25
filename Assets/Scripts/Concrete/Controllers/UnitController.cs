using Assets.Scripts.Concrete.Combats;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using Assets.Scripts.Concrete.Orders;
using Assets.Scripts.Concrete.UnitAIs;
using UnityEngine;


namespace Assets.Scripts.Concrete.Controllers
{
    internal class UnitController : MonoBehaviour
    {

        [Header("UNIT TYPE")]
        public UnitTypeEnum unitTypeEnum;
        public bool isSeleceted;

        [Header("Worrior And Villager")]
        public float attackRadius;
        public Transform attackPoint;
        public float attackPointDistance;

        [Header("Archer")]
        public float arrowSpeed;
        [HideInInspector] public GameObject arrow;


        [Space(30)]
        [Header("UNIT")]
        public int damage;
        [Range(0.1f, 2f)] public float moveSpeed = .7f;
        public float attackSpeed;
        public float attackRange;
        public float sightRange;
        [Header("UNIT SETTÝNGS")]
        [Range(0.1f, 1f)] public float unitAIPerTime = 0.5f;
        [Range(0.1f, 1f)] public float detechTargetPerTime = 0.5f;
        [Range(0.1f, 1f)] public float turnDirectionPerTime = 0.5f;
        [Range(0.1f, 1f)] public float collectResourcesPerTime = 1f;
        public LayerMask enemy;

        [HideInInspector] public Collider2D[] followTargets;
        [HideInInspector] public Collider2D[] hitTargets;

        [HideInInspector] public GameObject followingObj;
        [HideInInspector] public bool workOnce = true;
        [HideInInspector] public int currentDamage;
        [HideInInspector] public float currentMoveSpeed;
        [HideInInspector] public float currentAttackSpeed;
        [HideInInspector] public float currentAttackRange;
        [HideInInspector] public float currentSightRange;
        [HideInInspector] public float currentAttackRadius;
        [HideInInspector] public float currentArrowSpeed;
        [HideInInspector] public UnitOrderEnum unitOrderEnum;
        [HideInInspector] public Vector2 attackRangePosition;
        [HideInInspector] public Vector2 sightRangePosition;
        [HideInInspector] public UnitAI unitAI;
        [HideInInspector] public UnitDirection direction;
        [HideInInspector] public Animator animator;
        public int towerPosIndex;
        public bool aI = true;
        public bool onBuilding;
        public bool stayBuilding;
        public bool goBuilding;


        [HideInInspector] public CircleCollider2D circleCollider;
        AnimationEventController animationEventController;
        UnitPathFinding2D pF2D;
        AttackAI attackAI;
        DefendAI defendAI;
        StayAI stayAI;
        FollowAI followAI;
        UnitAttack unitAttack;
        Rigidbody2D rb2D;
        TowerAI towerAI;


        private void Awake()
        {
            pF2D = GetComponent<UnitPathFinding2D>();
            direction = new UnitDirection(pF2D, this);
            unitAI = new UnitAI(this, pF2D);
            attackAI = new AttackAI(this, pF2D);
            defendAI = new DefendAI(this, pF2D);
            stayAI = new StayAI(this, pF2D);
            followAI = new FollowAI(this, pF2D);
            towerAI = new TowerAI(this, pF2D);
            animationEventController = transform.GetChild(0).GetComponent<AnimationEventController>();
        }
        private void Start()
        {
            unitAttack = new UnitAttack(this, unitAI, pF2D, animationEventController);
            currentDamage = damage;
            currentAttackSpeed = attackSpeed;
            currentAttackRadius = attackRadius;
            currentSightRange = sightRange;
            currentAttackRange = attackRange;
            currentArrowSpeed = arrowSpeed;
            rb2D = GetComponent<Rigidbody2D>();
            animator = transform.GetChild(0).GetComponent<Animator>();
            circleCollider = GetComponent<CircleCollider2D>();

            // Invoke
            InvokeRepeating(nameof(OptimumUnitAI), 0.1f, unitAIPerTime);
            InvokeRepeating(nameof(OptimumDetechEnemies), .5f, detechTargetPerTime);
            InvokeRepeating(nameof(OptimumAITurnDirection), 0.1f, turnDirectionPerTime);

        }
        private void Update()
        {
            // Hareket hýzýný fps farkýna göre ayarla
            currentMoveSpeed = moveSpeed * Time.deltaTime;

            // Sadece takip edilecek birim atamasý yapýlýr
            if (unitOrderEnum == UnitOrderEnum.FollowOrder)
                followAI.SetFollowUnit();

            towerAI.SelectTower();
            towerAI.DestructTower();
        }
        void OptimumUnitAI()
        {
            attackRangePosition = transform.GetChild(0).position;

            //Unit AI
            if (aI)
            {
                if (unitOrderEnum == UnitOrderEnum.AttackOrder)
                    attackAI.AttackMode();
                if (unitOrderEnum == UnitOrderEnum.DefendOrder)
                    defendAI.DefendMode();
                if (unitOrderEnum == UnitOrderEnum.StayOrder)
                    stayAI.StaticMode();
                if (unitOrderEnum == UnitOrderEnum.FollowOrder)
                    followAI.FollowMode();

                unitAttack.AttackOn();

            }
            unitAI.RigidbodyControl(rb2D, stayBuilding);
            towerAI.GoTower();
        }
        void OptimumDetechEnemies()
        {
            followTargets = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, enemy);
        }
        void OptimumAITurnDirection()
        {
            // Oyuncunun hareket emri her zaman önceliklidir || Sadece düþman varsa çalýþýr
            if (pF2D.moveCommand || unitAI.nearestTarget == null) return;

            // pF2D.pathLeftToGo[0]; hedefe giderken kullandýðý yol
            if (unitTypeEnum == UnitTypeEnum.Villager)
            {
                // Durduðunda hadefe bak
                if (pF2D.isPathEnd)
                    direction.Turn2Direction(unitAI.nearestTarget.transform.position.x);
                // Ýlerlediðinde yola bak
                else if (pF2D.pathLeftToGo.Count > 0)
                    direction.Turn2Direction(pF2D.pathLeftToGo[0].x);
            }

            if (unitTypeEnum == UnitTypeEnum.Archer)
            {
                // Durduðunda hadefe bak
                if (pF2D.isPathEnd)
                    direction.Turn8Direction(unitAI.nearestTarget.transform.position);
                // Ýlerlediðinde yola bak
                else if (pF2D.pathLeftToGo.Count > 0)
                    direction.Turn8Direction(pF2D.pathLeftToGo[0]);

            }
            if (unitTypeEnum == UnitTypeEnum.Worrior)
            {
                // Durduðunda hadefe bak
                if (pF2D.isPathEnd)
                    direction.Turn4Direction(unitAI.nearestTarget.transform.position);
                // Ýlerlediðinde yola bak
                else if (pF2D.pathLeftToGo.Count > 0)
                    direction.Turn4Direction(pF2D.pathLeftToGo[0]);
            }

        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(sightRangePosition, currentSightRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackRangePosition, currentAttackRange);

            if (unitTypeEnum == UnitTypeEnum.Worrior || unitTypeEnum == UnitTypeEnum.Villager)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawWireSphere(attackPoint.position, currentAttackRadius);

            }
        }
    }
}
