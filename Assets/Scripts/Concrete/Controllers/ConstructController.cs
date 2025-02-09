using Assets.Scripts.Concrete.Movements;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Concrete.Controllers
{
    public class ConstructController : MonoBehaviour
    {
        public GameObject constructing;
        public SpriteRenderer visualSprite;
        public float hitNumber;
        public float currentHitNumber;
        public bool isFull;
        public Image constructionTimerImage;
        DynamicOrderInLayer dynamicOrderInLayer;

        private void Awake()
        {
            dynamicOrderInLayer = new();
        }
        private void Start()
        {
            dynamicOrderInLayer.OrderInLayerWithYPos(visualSprite.transform, visualSprite);
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