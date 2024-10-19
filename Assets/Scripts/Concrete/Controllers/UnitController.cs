using Assets.Scripts.Concrete.Combats;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using Assets.Scripts.Concrete.Orders;
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
        public GameObject arrow;
        public float arrowSpeed;


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
        public Collider2D[] followTargets;
        public Collider2D[] hitTargets;
        public LayerMask enemy;

        public GameObject followingObj;
        public bool workOnce = true;

        public int currentDamage;
        public float currentMoveSpeed;
        public float currentAttackSpeed;
        public float currentAttackRange;
        public float currentSightRange;
        public float currentAttackRadius;
        public float currentArrowSpeed;
        public UnitOrderEnum unitOrderEnum;
        [HideInInspector] public Vector2 attackRangePosition;
        [HideInInspector] public Vector2 sightRangePosition;

        AnimationEventController animationEventController;
        UnitPathFinding2D pF2D;
        public UnitAI unitAI;
        AttackAI attackAI;
        DefendAI defendAI;
        StayAI stayAI;
        FollowAI followAI;
        UnitAttack unitAttack;
        [HideInInspector] public UnitDirection direction;




        private void Awake()
        {
            pF2D = GetComponent<UnitPathFinding2D>();
            direction = new UnitDirection(pF2D, this);
            unitAI = new UnitAI(this, pF2D);
            attackAI = new AttackAI(this, pF2D);
            defendAI = new DefendAI(this, pF2D);
            stayAI = new StayAI(this, pF2D);
            followAI = new FollowAI(this, pF2D);
            animationEventController = transform.GetChild(0).GetComponent<AnimationEventController>();
        }
        private void Start()
        {
            unitAttack = new UnitAttack(this, unitAI, pF2D, animationEventController);
            //currentSpeed = moveSpeed / 100;
            currentDamage = damage;
            currentAttackSpeed = attackSpeed;
            currentAttackRadius = attackRadius;
            currentSightRange = sightRange;
            currentAttackRange = attackRange;
            currentArrowSpeed = arrowSpeed;

            // Invoke
            InvokeRepeating(nameof(OptimumUnitAI), 0.1f, unitAIPerTime);
            InvokeRepeating(nameof(OptimumDetechEnemies), .5f, detechTargetPerTime);
            InvokeRepeating(nameof(OptimumAITurnDirection), 0.1f, turnDirectionPerTime);

        }
        private void Update()
        {
            currentMoveSpeed = moveSpeed * Time.deltaTime;
            // Sadece takip edilecek birim atamasý yapýlýr
            if (unitOrderEnum == UnitOrderEnum.FollowOrder)
                followAI.SetFollowUnit();
        }
        void OptimumUnitAI()
        {
            attackRangePosition = transform.GetChild(0).position;

            //Unit AI
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
        void OptimumDetechEnemies()
        {
            //if (unitTypeEnum == UnitTypeEnum.Villager) return;
            followTargets = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, enemy);
        }
        void OptimumAITurnDirection()
        {
            // Oyuncunun hareket emri her zaman önceliklidir || Sadece düþman varsa çalýþýr
            if (pF2D.moveCommand || unitAI.DetechNearestTarget() == null) return;

            // pF2D.pathLeftToGo[0]; hedefe giderken kullandýðý yol
            if (unitTypeEnum == UnitTypeEnum.Villager)
            {
                if (pF2D.isPathEnd)
                    direction.Turn2Direction(unitAI.DetechNearestTarget().transform.position.x);
                else if (pF2D.pathLeftToGo.Count > 0)
                    direction.Turn2Direction(pF2D.pathLeftToGo[0].x);
            }

            if (unitTypeEnum == UnitTypeEnum.Archer)
            {
                // Durduðunda hadefe bak
                if (pF2D.isPathEnd)
                    direction.Turn8Direction(unitAI.DetechNearestTarget().transform.position);
                // Ýlerlediðinde yola bak
                else if (pF2D.pathLeftToGo.Count > 0)
                    direction.Turn8Direction(pF2D.pathLeftToGo[0]);

            }
            if (unitTypeEnum == UnitTypeEnum.Worrior)
            {
                // Durduðunda hadefe bak
                if (pF2D.isPathEnd)
                    direction.Turn4Direction(unitAI.DetechNearestTarget().transform.position);
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
