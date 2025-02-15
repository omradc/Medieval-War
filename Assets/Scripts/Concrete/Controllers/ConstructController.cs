using Assets.Scripts.Concrete.Movements;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Concrete.Controllers
{
    public class ConstructController : MonoBehaviour
    {
        public GameObject constructing;
        public Transform visual;
        public SpriteRenderer[] visualSprites;
        public float hitNumber;
        public float currentHitNumber;
        public bool isFull;
        public Image constructionTimerImage;
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
            dynamicOrderInLayer.OrderInLayerWithYPos(visual, visualSprites);

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