using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using Assets.Scripts.Concrete.Resources;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class PawnController : MonoBehaviour
    {
        public GameObject targetResource;
        public float dropResourceLifeTime = 3;

        [Header("TREE")]
        [HideInInspector] public GameObject nearestTree;
        [HideInInspector] public Vector2 treeChopPos;
        public int treeDamage = 1;
        public float chopSpeed;
        public float chopTreeSightRange;
        public float woodCollectTime;
        public LayerMask treeLayer;
        [HideInInspector] public Vector3 distanceToTreePos;
        [HideInInspector] public Collider2D[] trees;
        public GameObject resourceWood;
        [HideInInspector] public bool isFirstTree;

        [Header("MİNE")]
        public float miningTime;
        public GameObject resourceGold;
        public GameObject resourceRock;
        public MineController mineController;


        [Header("SHEEP")]
        public GameObject fenceObj;
        public GameObject resourceMeat;
        public float meatCollectTime;
        [HideInInspector] public FenceController fence;



        [Header("CONSTRUCTION")]
        public float buildSpeed;
        public GameObject constructionObj;
        public ConstructController constructController;

        public GameObject repo;
        [HideInInspector] public float currentChopTreeSightRange;
        [HideInInspector] public int currentTreeDamage;
        [HideInInspector] public float tChop;
        [HideInInspector] public float tCollect;
        public float tMining;
        [HideInInspector] public bool returnHome;
        [HideInInspector] public bool returnFences;
        [HideInInspector] public bool workOnce;
        [HideInInspector] public bool workOnce2;
        [HideInInspector] public bool workOnceForTree = true;
        public bool isMineEmpty;
        [HideInInspector] public bool isTree;
        public bool isMine;
        [HideInInspector] public bool isSheep;
        [HideInInspector] public KnightController kC;
        [HideInInspector] public PathFinding pF;
        [HideInInspector] public SpriteRenderer villagerSpriteRenderer;
        [HideInInspector] public Animator animator;
        [HideInInspector] public GameObject goldIdle;
        [HideInInspector] public GameObject rockIdle;
        [HideInInspector] public GameObject woodIdle;
        [HideInInspector] public GameObject meatIdle;
        [HideInInspector] public IInput ıInput;
        [HideInInspector] public AnimationEventController animationEventController;
        [HideInInspector] public SheepController sheepController;
        CollectResources collectResources;
        CollectGoldOrRock collectGoldAndRock;
        CollectWood collectWood;
        CollectFood collectFood;
        Construction construction;
        private void Awake()
        {
            kC = GetComponent<KnightController>();
            pF = GetComponent<PathFinding>();
            villagerSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            animator = transform.GetChild(0).GetComponent<Animator>();
            goldIdle = transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
            rockIdle = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
            woodIdle = transform.GetChild(0).GetChild(0).GetChild(2).gameObject;
            meatIdle = transform.GetChild(0).GetChild(0).GetChild(3).gameObject;
            ıInput = new PcInput();
            animationEventController = transform.GetChild(0).GetComponent<AnimationEventController>();
            collectResources = new(this, pF);
            collectGoldAndRock = new(this, pF);
            collectWood = new(this, pF);
            construction = new(this, pF);
            collectFood = new(this, pF);
        }
        private void Start()
        {
            currentChopTreeSightRange = chopTreeSightRange;
            currentTreeDamage = treeDamage;

            //  Events
            animationEventController.ChopWoodEvent += collectWood.Chop;
            animationEventController.GetHitTreeEvent += collectWood.GetHitTree;
            animationEventController.ChopSheepEvent += Idle;
            animationEventController.GetHitSheepEvent += collectFood.GetHitSheep;
            animationEventController.BuildEvent += construction.Build;
            animationEventController.BuildEndEvent += construction.BuildEnd;
            //Invoke
            InvokeRepeating(nameof(OptimumVillager), 0.1f, kC.collectResourcesPerTime);
        }
        private void Update()
        {

            collectResources.ReadyToNextCommand();
            collectResources.SelectResourceType();

            if (transform.hasChanged)
            {
                goldIdle.GetComponent<SpriteRenderer>().sortingOrder = kC.visual.sortingOrder + 1;
                rockIdle.GetComponent<SpriteRenderer>().sortingOrder = kC.visual.sortingOrder + 1;
                meatIdle.GetComponent<SpriteRenderer>().sortingOrder = kC.visual.sortingOrder + 1;
                woodIdle.GetComponent<SpriteRenderer>().sortingOrder = kC.visual.sortingOrder + 1;
                transform.hasChanged = false;
            }
        }
        void OptimumVillager()
        {
            if (kC.knightAI.target != null && targetResource != null) //Düşman varsa kaynak toplama
            {
                // Elinde herhangi bir kaynak varsa onu yere at
                collectResources.DropAnyResources();
            }

            else //Düşman yoksa kaynak toplayabilir
            {
                Debug.Log("OptimumVillager");
                construction.GoConstruct();
                collectFood.GoToFences();
                collectFood.GoToSheep();
                collectResources.GoToHome();
                if (repo != null) // Depo varsa kaynak topla
                {
                    collectGoldAndRock.GoToMine();
                    collectWood.GoToTree();
                    collectFood.CutSheep();
                }
            }
        }
        void Idle()
        {
            AnimationManager.Instance.IdleAnim(animator);
            targetResource = null;
        }
        private void OnDrawGizmos()
        {
            if (!isTree) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(targetResource.transform.position, currentChopTreeSightRange);
        }
    }
}
