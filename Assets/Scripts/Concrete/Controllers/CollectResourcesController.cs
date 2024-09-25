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
        public GameObject nearestTree;
        public Vector2 nearestTreeChopPos;
        public int treeDamagePoint;
        public float chopSpeed;
        public float chopTreeSightRange;
        public float woodCollectTime;
        public LayerMask treeLayer;
        public Vector3 distanceToTreePos;
        public Collider2D[] trees;
        public GameObject resourceWood;
        public int collectWoodAmount;
        public int collectMeatCount;

        [Header("MİNE")]
        public float miningTime;
        public int collectGoldAmount;
        public int collectRockAmount;
        public GameObject resourceGold;
        public GameObject resourceRock;
        public Mine mine;


        [Header("SHEEP")]
        public Fence fence;
        public GameObject fenceObj;
        public GameObject resourceMeat;
        public float meatCollectTime;
        

        public Vector3 homePos;
        public float currentChopTreeSightRange;
        public int currentTreeDamagePoint;
        public float tChop;
        public float tCollect;
        public float tMining;
        public bool returnHome;
        public bool returnFences;
        public bool workOnce;
        public bool workOnce2;
        public bool workOnceForTree = true;
        public bool isMineEmpty;
        public bool isTree;
        public bool isMine;
        public bool isSheep;


        [HideInInspector] public UnitController uC;
        [HideInInspector] public PathFinding2D pF2D;
        [HideInInspector] public SpriteRenderer villagerSpriteRenderer;
        [HideInInspector] public Animator animator;
        [HideInInspector] public GameObject goldIdle;
        [HideInInspector] public GameObject rockIdle;
        [HideInInspector] public GameObject woodIdle;
        [HideInInspector] public GameObject meatIdle;
        [HideInInspector] public Direction direction;
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
            pF2D = GetComponent<PathFinding2D>();
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

            InvokeRepeating(nameof(OptimumTurn2Direction), .1f, .5f);
        }
        private void Update()
        {
            collectResources.ReadyToNextCommand();
            collectResources.SelectResourceType();
            goldAndRock.GoToMine();
            collectWood.GoToTree();
            collectFood.GoToSheep();
            collectFood.GoToFences();
            collectResources.GoToHome();
        }
        public void OptimumTurn2Direction()
        {
            if (returnFences && fenceObj != null && isSheep)
                direction.Turn2Direction(fenceObj.transform.position.x);
            if (targetResource == null) return;
            if (!returnHome)
            {
                if (isMine)
                    direction.Turn2Direction(targetResource.transform.position.x);
                if (isTree && nearestTree != null)
                    direction.Turn2Direction(nearestTree.transform.position.x);
                if (isSheep && !returnFences)
                    direction.Turn2Direction(targetResource.transform.position.x);
            }
            if (returnHome)
                direction.Turn2Direction(homePos.x);

        }
        private void OnDrawGizmos()
        {
            if (!isTree) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(targetResource.transform.position, currentChopTreeSightRange);
        }
    }
}
