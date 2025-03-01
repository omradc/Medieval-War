using Assets.Scripts.Concrete.Movements;
using Assets.Scripts.Concrete.PowerStats;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    [RequireComponent(typeof(BuildingStats))]
    class BuildingController : MonoBehaviour
    {
        public GameObject construction;
        [SerializeField] GameObject visualBuilding;
        [SerializeField] GameObject visualDestructed;
        [SerializeField] bool destroyPermanent;
        [SerializeField] int destroyTime;
        [SerializeField] Transform OrderInLayerSpriteAnchor;

        // Sprite
        SpriteRenderer wallVerticalDownVisualSprite;
        SpriteRenderer wallDoorCloseVisualSprite;
        SpriteRenderer wallDoorOpen1VisualSprite;
        SpriteRenderer wallDoorOpen2VisualSprite;
        Transform wallDestructed;
        public SpriteRenderer[] visualSprites;
        public SpriteRenderer[] destructedVisualSprites;
        SpriteRenderer visualSprite;
        SpriteRenderer visualDestructedSprite;

        // Kule yapay zekası için gerekli değerler
        [HideInInspector] public bool isFull;
        [HideInInspector] public bool destruct;
        [HideInInspector] public int unitValue;

        [HideInInspector] public InteractableObjUIController interactableObjUIController;
        DynamicOrderInLayer dynamicOrderInLayer;
        HealthController healthController;
        BuildingStats buildingStats;
        bool workOnce = true;


        private void Awake()
        {
            healthController = GetComponent<HealthController>();
            interactableObjUIController = GetComponent<InteractableObjUIController>();
            visualSprite = visualBuilding.GetComponent<SpriteRenderer>();
            visualDestructedSprite = visualDestructed.GetComponent<SpriteRenderer>();
            buildingStats = GetComponent<BuildingStats>();
            dynamicOrderInLayer = new();
        }
        private void Start()
        {
            PowerStatsAssign();
            dynamicOrderInLayer.OrderInLayerInitialize(OrderInLayerSpriteAnchor, visualSprite);
            dynamicOrderInLayer.OrderInLayerInitialize(OrderInLayerSpriteAnchor, visualDestructedSprite);
            SpriteAssingment();
        }
        void PowerStatsAssign()
        {
            healthController.health = buildingStats.health;
            healthController.regeneration = buildingStats.regrenation;
            healthController.regenerationAmount = buildingStats.regenerationAmount;
            healthController.regrenationPerTime = buildingStats.regrenationPerTime;
            healthController.regrenationAfterDamageTime = buildingStats.regrenationAfterDamageTime;
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

            if (interactableObjUIController != null)
                interactableObjUIController.interactablePanel.SetActive(false);

            if (destroyPermanent)
                Destroy(gameObject, destroyTime);
        }
        void Upgrade()
        {
            if (interactableObjUIController == null) return;
            if (interactableObjUIController.upgrade)
            {
                GameObject obj = Instantiate(interactableObjUIController.construct, transform.position, Quaternion.identity);
                obj.GetComponent<ConstructController>().constructing = interactableObjUIController.upgradedBuilding;
                //Destroy(gameObject);
                gameObject.SetActive(false);
            }
        }
        void SpriteAssingment()
        {
            // Wall Vertical
            if (gameObject.name == "WallVertical_Blue(Clone)" || gameObject.name == "WallVertical_Yellow(Clone)" || gameObject.name == "WallVertical_Red(Clone)" || gameObject.name == "WallVertical_Purple(Clone)")
                wallVerticalDownVisualSprite = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
            // Wall Door
            if (gameObject.name == "WallDoor_Blue(Clone)" || gameObject.name == "WallDoor_Yellow(Clone)" || gameObject.name == "WallDoor_Red(Clone)" || gameObject.name == "WallDoor_Purple(Clone)")
            {
                wallDoorCloseVisualSprite = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
                wallDoorOpen1VisualSprite = transform.GetChild(2).GetChild(1).GetChild(2).GetComponent<SpriteRenderer>();
                wallDoorOpen2VisualSprite = transform.GetChild(2).GetChild(1).GetChild(3).GetComponent<SpriteRenderer>();
            }
            if (gameObject.layer == 11)
                wallDestructed = transform.GetChild(1);


            if (wallVerticalDownVisualSprite != null)
                dynamicOrderInLayer.OrderInLayerInitialize(OrderInLayerSpriteAnchor, wallVerticalDownVisualSprite, -1);
            if (wallDoorCloseVisualSprite != null)
                dynamicOrderInLayer.OrderInLayerInitialize(OrderInLayerSpriteAnchor, wallDoorCloseVisualSprite, -1);
            if (wallDoorOpen1VisualSprite != null)
                dynamicOrderInLayer.OrderInLayerInitialize(OrderInLayerSpriteAnchor, wallDoorOpen1VisualSprite, -2);
            if (wallDoorOpen2VisualSprite != null)
                dynamicOrderInLayer.OrderInLayerInitialize(OrderInLayerSpriteAnchor, wallDoorOpen2VisualSprite, -2);
            if (visualBuilding != null && visualBuilding.transform.childCount > 0)
            {
                visualSprites = new SpriteRenderer[visualBuilding.transform.childCount];
                for (int i = 0; i < visualSprites.Length; i++)
                {
                    visualSprites[i] = visualBuilding.transform.GetChild(i).GetComponent<SpriteRenderer>();
                }
                if (gameObject.name.StartsWith("WallVertical"))
                    dynamicOrderInLayer.OrderInLayerInitialize(OrderInLayerSpriteAnchor, visualSprites, -1);
                else
                    dynamicOrderInLayer.OrderInLayerInitialize(OrderInLayerSpriteAnchor, visualSprites, 1);
            }
            if (wallDestructed != null)
            {
                destructedVisualSprites = new SpriteRenderer[wallDestructed.childCount];
                for (int i = 0; i < destructedVisualSprites.Length; i++)
                {
                    destructedVisualSprites[i] = wallDestructed.GetChild(i).GetComponent<SpriteRenderer>();
                }
                dynamicOrderInLayer.OrderInLayerInitialize(OrderInLayerSpriteAnchor, destructedVisualSprites);
            }

        }
    }
}