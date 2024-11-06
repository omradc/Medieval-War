using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.Movements;
using Assets.Scripts.Concrete.Resources;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class CollectResourcesController : MonoBehaviour
    {
        public GameObject targetResource;
        public float dropResourceLifeTime = 3;

        [Header("TREE")]
        [HideInInspector] public GameObject nearestTree;
        [HideInInspector] public Vector2 treeChopPos;
        public int treeDamagePoint;
        public float chopSpeed;
        public float chopTreeSightRange;
        public float woodCollectTime;
        public LayerMask treeLayer;
        [HideInInspector] public Vector3 distanceToTreePos;
        [HideInInspector] public Collider2D[] trees;
        public GameObject resourceWood;
        public int collectWoodAmount;
        public int collectMeatCount;
        [HideInInspector] public bool isFirstTree;

        [Header("MİNE")]
        public float miningTime;
        public int collectGoldAmount;
        public int collectRockAmount;
        public GameObject resourceGold;
        public GameObject resourceRock;
        [HideInInspector] public Mine mine;


        [Header("SHEEP")]
        [HideInInspector] public Fence fence;
        [HideInInspector] public GameObject fenceObj;
        public GameObject resourceMeat;
        public float meatCollectTime;


        [HideInInspector] public Vector3 homePos;
        [HideInInspector] public float currentChopTreeSightRange;
        [HideInInspector] public int currentTreeDamagePoint;
        [HideInInspector] public float tChop;
        [HideInInspector] public float tCollect;
        [HideInInspector] public float tMining;
        [HideInInspector] public bool returnHome;
        [HideInInspector] public bool returnFences;
        [HideInInspector] public bool workOnce;
        [HideInInspector] public bool workOnce2;
        [HideInInspector] public bool workOnceForTree = true;
        [HideInInspector] public bool isMineEmpty;
        [HideInInspector] public bool isTree;
        [HideInInspector] public bool isMine;
        [HideInInspector] public bool isSheep;


        [HideInInspector] public UnitController uC;
        [HideInInspector] public UnitPathFinding2D pF2D;
        [HideInInspector] public SpriteRenderer villagerSpriteRenderer;
        [HideInInspector] public Animator animator;
        [HideInInspector] public GameObject goldIdle;
        [HideInInspector] public GameObject rockIdle;
        [HideInInspector] public GameObject woodIdle;
        [HideInInspector] public GameObject meatIdle;
        [HideInInspector] public UnitDirection direction;
        [HideInInspector] public IInput ıInput;
        [HideInInspector] public Sheep sheep;
        AnimationEventController animationEventController;
        Resources.Tree tree;
        CollectResources collectResources;
        CollectGoldOrRock goldAndRock;
        CollectWood collectWood;
        CollectFood collectFood;

        private void Awake()
        {
            uC = GetComponent<UnitController>();
            pF2D = GetComponent<UnitPathFinding2D>();
            villagerSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            animator = transform.GetChild(0).GetComponent<Animator>();
            goldIdle = transform.GetChild(1).gameObject;
            rockIdle = transform.GetChild(2).gameObject;
            woodIdle = transform.GetChild(3).gameObject;
            meatIdle = transform.GetChild(4).gameObject;
            ıInput = new PcInput();
            animationEventController = transform.GetChild(0).GetComponent<AnimationEventController>();
            collectResources = new(this);
            goldAndRock = new(this, pF2D);
            collectWood = new(this, pF2D);
            collectFood = new(this, pF2D);
        }
        private void Start()
        {
            direction = uC.direction;
            currentTreeDamagePoint = treeDamagePoint;
            currentChopTreeSightRange = chopTreeSightRange;

            //  Events
            animationEventController.ChopEvent += collectWood.Chop;
            animationEventController.GetHitTreeEvent += collectWood.GetHitTree;

            //Invoke
            InvokeRepeating(nameof(OptimumCollectResources), 0.1f, uC.collectResourcesPerTime);
        }
        private void Update()
        {
            collectResources.ReadyToNextCommand();
            collectResources.SelectResourceType();
        }

        void OptimumCollectResources()
        {
            //Düşman varsa kaynak toplama
            if (uC.unitAI.DetechNearestTarget() != null)
            {
                // Elinde herhangi bir kaynak varsa onu yere at
                collectResources.DropAnyResources();
                return;
            }

            //Düşman yoksa işine devam et
            if (uC.unitAI.DetechNearestTarget() == null) pF2D.isPathEnd = false;

            goldAndRock.GoToMine();
            collectWood.GoToTree();
            collectFood.GoToSheep();
            collectFood.GoToFences();
            collectResources.GoToHome();
        }
        private void OnDrawGizmos()
        {
            if (!isTree) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(targetResource.transform.position, currentChopTreeSightRange);
        }
    }
}
