using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    class BuildingController : MonoBehaviour
    {
        public GameObject construction;
        [SerializeField] GameObject visualBuilding;
        [SerializeField] GameObject visualDestructed;
        [SerializeField] SpriteRenderer top_1xDownVisualSprite;
        [SerializeField] SpriteRenderer doorCloseVisualSprite;
        [SerializeField] SpriteRenderer doorOpen1VisualSprite;
        [SerializeField] SpriteRenderer doorOpen2VisualSprite;
        [SerializeField] bool destroyPermanent;
        [SerializeField] int destroyTime;

        SpriteRenderer visualSprite;
        SpriteRenderer visualDestructedSprite;
        // Kule yapay zekası için gerekli değerler
        [HideInInspector] public bool isFull;
        [HideInInspector] public bool destruct;
        [HideInInspector] public int unitValue;

        [HideInInspector] public BuildingPanelController buildingPanelController;
        ButtonController buttonController;
        HealthController healthController;
        DynamicOrderInLayer dynamicOrderInLayer;
        bool workOnce = true;

        private void Awake()
        {
            healthController = GetComponent<HealthController>();
            buttonController = GetComponent<ButtonController>();
            buildingPanelController = GetComponent<BuildingPanelController>();
            dynamicOrderInLayer = new();
            visualSprite = visualBuilding.GetComponent<SpriteRenderer>();
            visualDestructedSprite = visualDestructed.GetComponent<SpriteRenderer>();
        }
        private void Start()
        {
            dynamicOrderInLayer.OrderInLayerWithYPos(visualSprite.transform, visualSprite);
            dynamicOrderInLayer.OrderInLayerWithYPos(visualDestructedSprite.transform, visualDestructedSprite);

            if (top_1xDownVisualSprite != null)
                dynamicOrderInLayer.OrderInLayerWithNumber(visualSprite.transform, top_1xDownVisualSprite, -1);
            if (doorCloseVisualSprite != null)
                dynamicOrderInLayer.OrderInLayerWithYPos(doorCloseVisualSprite.transform, doorCloseVisualSprite);
            if (doorOpen1VisualSprite != null)
                dynamicOrderInLayer.OrderInLayerWithYPos(doorOpen1VisualSprite.transform, doorOpen1VisualSprite);
            if (doorOpen2VisualSprite != null)
                dynamicOrderInLayer.OrderInLayerWithYPos(doorOpen2VisualSprite.transform, doorOpen2VisualSprite);



        }
        private void Update()
        {
            if (healthController.isDead)
                Destruct();
            Upgrade();

        }

        void Destruct()
        {
            if (!workOnce) return;
            destruct = true;
            healthController.enabled = false;
            healthController.HealthBarVisibility(false);
            visualBuilding.SetActive(false);
            visualDestructed.SetActive(true);
            gameObject.layer = 26; // Katman = Destructed
            workOnce = false;

            if (buildingPanelController != null)
                buildingPanelController.interactablePanel.SetActive(false);

            if (destroyPermanent)
                Destroy(gameObject, destroyTime);
        }

        void Upgrade()
        {
            if (buttonController == null) return;
            if (buttonController.upgrade)
            {
                GameObject obj = Instantiate(buttonController.construct, transform.position, Quaternion.identity);
                obj.GetComponent<ConstructController>().constructing = buttonController.upgradedBuilding;
                Destroy(gameObject, 0);

            }
        }
    }
}