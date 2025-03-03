using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using NUnit.Framework;
using System.Collections.Generic;
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
        [HideInInspector] public GameObject previousBuilding;
        [SerializeField] Transform OrderInLayerSpriteAnchor;
        [HideInInspector] public float currentHitNumber;
        [HideInInspector] public bool isFull;
        SpriteRenderer[] visualSprites;
        DynamicOrderInLayer dynamicOrderInLayer;
        public List<GameObject> knights;

        private void Awake()
        {
            dynamicOrderInLayer = new();
            knights = new();
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
            if (previousBuilding != null && gameObject.CompareTag("HouseConstruction"))
                knights = previousBuilding.GetComponent<KnightHouseController>().knights;
        }
        void Update()
        {

            if (currentHitNumber >= hitNumber)
                Build();
        }

        void Build()
        {
            GameObject constructed = Instantiate(constructing, transform.position, Quaternion.identity);
            // Yükseltme yapılınca, bir önceki evin bilgisi yükseltilen evin bilgisine eşitlenir.
            if (constructed.TryGetComponent(out KnightHouseController knightHouseController) && previousBuilding != null)
            {
                knightHouseController.knights = previousBuilding.GetComponent<KnightHouseController>().knights;
            }
            if (constructed.TryGetComponent(out RepoController repoController) && previousBuilding != null)
            {
                repoController.currentRepoCapacity = previousBuilding.GetComponent<RepoController>().currentRepoCapacity;
            }
            ResourcesManager.Instance.RemoveRepo(previousBuilding);
            Destroy(previousBuilding);
            Destroy(gameObject);
        }

        public void UpdateConstructionTimerImage()
        {
            constructionTimerImage.fillAmount = currentHitNumber / hitNumber;
        }

    }
}