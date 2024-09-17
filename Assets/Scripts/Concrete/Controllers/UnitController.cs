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
        [Header("Worrior")]
        public float attackRadius;
        public Transform worriorAttackPoint;
        public float worriorAttackPointDistance;

        [Header("Archer")]
        public GameObject arrow;
        public float arrowSpeed;

        [Space(30)]
        [Header("UNIT")]
        public int damage;
        [Range(0, 10f)] public float speed;
        public float attackSpeed;
        public float attackDelay;
        public float attackRange;
        public float sightRange;

        [Header("UNIT SETTÝNGS")]
        [Range(0.1f, 1f)] public float detechTargetPerTime = 0.5f;
        [Range(0.1f, 1f)] public float followingTargetPerTime = 0.5f;
        [Range(0.1f, 1f)] public float turnDirectionPerTime = 0.5f;
        public Collider2D[] followTargets;
        public Collider2D[] hitTargets;
        public LayerMask targetLayer;

        public int currentDamage;
        public float currentSpeed;
        public float currentAttackSpeed;
        public float currentAttackDelay;
        public float currentAttackRange;
        public float currentSightRange;
        public float currentAttackRadius;
        public float currentArrowSpeed;
        public UnitOrderEnum unitOrderEnum;
        [HideInInspector] public Vector2 attackRangePosition;
        [HideInInspector] public Vector2 sightRangePosition;
        public bool workOnce = true;

        AnimationEventController animationEventController;
        PathFinding2D pF2D;
        Order order;
        AttackOrder attackOrder;
        DefendOrder defendOrder;
        StayOrder stayOrder;
        FollowOrder followOrder;
        Attack attack;
        Direction direction;




        private void Awake()
        {
            pF2D = GetComponent<PathFinding2D>();
            direction = new Direction(pF2D, this);
            order = new Order(this, pF2D);
            attackOrder = new AttackOrder(this, pF2D);
            defendOrder = new DefendOrder(this, pF2D);
            stayOrder = new StayOrder(this, pF2D);
            followOrder = new FollowOrder(this, pF2D);
            animationEventController = transform.GetChild(0).GetComponent<AnimationEventController>();
        }
        private void Start()
        {
            attack = new Attack(this, order, pF2D, animationEventController);
            currentSpeed = speed / 100;
            currentDamage = damage;
            currentAttackSpeed = attackSpeed;
            currentAttackDelay = attackDelay;
            currentAttackRadius = attackRadius;
            currentSightRange = sightRange;
            currentAttackRange = attackRange;
            currentArrowSpeed = arrowSpeed;

            // Invoke
            InvokeRepeating(nameof(OptimumDetechTargets), 0.1f, detechTargetPerTime);
            InvokeRepeating(nameof(OptimumAITurnDirection), 0.1f, turnDirectionPerTime);

        }

        private void Update()
        {
            //Unit Behaviours
            attackRangePosition = transform.GetChild(0).position;
            if (unitOrderEnum == UnitOrderEnum.AttackOrder)
                attackOrder.AttackMode();
            if (unitOrderEnum == UnitOrderEnum.DefendOrder)
                defendOrder.DefendMode();
            if (unitOrderEnum == UnitOrderEnum.StayOrder)
                stayOrder.StaticMode();
            if (unitOrderEnum == UnitOrderEnum.FollowOrder)
                followOrder.FollowMode();

            attack.AttackOn();
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(sightRangePosition, currentSightRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackRangePosition, currentAttackRange);

            if (unitTypeEnum == UnitTypeEnum.Worrior)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawWireSphere(worriorAttackPoint.position, currentAttackRadius);

            }

        }
        void OptimumDetechTargets()
        {
            followTargets = Physics2D.OverlapCircleAll(sightRangePosition, currentSightRange, targetLayer);
        }
      
        void OptimumAITurnDirection()
        {
            if (pF2D.moveCommand || order.DetechNearestTarget() == null) return;
            if (unitTypeEnum == UnitTypeEnum.Archer)
                direction.Turn8Direction(order.DetechNearestTarget().transform.position);
            if (unitTypeEnum == UnitTypeEnum.Worrior)
                direction.Turn4Direction(order.DetechNearestTarget().transform.position);
        }
    }
}
