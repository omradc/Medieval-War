using Assets.Scripts.Concrete.Movements;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Concrete.Controllers
{
    public class ConstructController : MonoBehaviour
    {
        public GameObject constructing;
        public Transform visual;
        public Image constructionTimerImage;
        public float hitNumber;
        [SerializeField] Transform OrderInLayerSpriteAnchor;
        [HideInInspector] public float currentHitNumber;
        [HideInInspector] public bool isFull;
        SpriteRenderer[] visualSprites;
        DynamicOrderInLayer dynamicOrderInLayer;

        private void Awake()
        {
            dynamicOrderInLayer = new();

            if (visual.childCount == 0)
            {
                visualSprites = new SpriteRenderer[1];
                visualSprites[0] = visual.GetComponent<SpriteRenderer>();
            }
            else
            {
                visualSprites = new SpriteRenderer[visual.childCount];
                for (int i = 0; i < visual.childCount; i++)
                {
                    visualSprites[i] = visual.GetChild(i).GetComponent<SpriteRenderer>();
                }

            }
        }
        private void Start()
        {
            dynamicOrderInLayer.OrderInLayerInitialize(OrderInLayerSpriteAnchor, visualSprites);

        }
        void Update()
        {

            if (currentHitNumber >= hitNumber)
                Build();
        }

        void Build()
        {
            Instantiate(constructing, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        public void UpdateConstructionTimerImage()
        {
            constructionTimerImage.fillAmount = currentHitNumber / hitNumber;
        }

    }
}