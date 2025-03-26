using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.Movements;
using Assets.Scripts.Concrete.Resources;
using Mono.Cecil.Cil;
using UnityEngine;

namespace Assets.Scripts.Concrete.CollectResource
{
    class CollectResourceController : MonoBehaviour
    {
        public GameObject targetResource;

        [Header("MİNE")]
        public float miningTime;
        public GameObject resourceGold;
        public GameObject resourceRock;
        [HideInInspector] public MineController mineControler;

        [HideInInspector] public GameObject repo;
        [HideInInspector] public KnightController kC;
        [HideInInspector] public PathFinding pF;
        [HideInInspector] public SpriteRenderer pawnSpriteRenderer;
        [HideInInspector] public Animator animator;
        [HideInInspector] public GameObject goldIdle;
        [HideInInspector] public GameObject rockIdle;
        [HideInInspector] public GameObject woodIdle;
        [HideInInspector] public GameObject meatIdle;
        CollectGoldOrRock1 collectGoldAndRock1;
        AnimationEventController animationEventController;
        private void Awake()
        {
            kC = GetComponent<KnightController>();
            pF = GetComponent<PathFinding>();
            pawnSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            animator = transform.GetChild(0).GetComponent<Animator>();
            goldIdle = transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
            rockIdle = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
            woodIdle = transform.GetChild(0).GetChild(0).GetChild(2).gameObject;
            meatIdle = transform.GetChild(0).GetChild(0).GetChild(3).gameObject;
            animationEventController = transform.GetChild(0).GetComponent<AnimationEventController>();
            collectGoldAndRock1 = new(this, pF);

        }
        private void Start()
        {
        }

    }
}