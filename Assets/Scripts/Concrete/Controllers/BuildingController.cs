using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    class BuildingController : MonoBehaviour
    {
        public GameObject construction;
        [SerializeField] GameObject visualBuilding;
        [SerializeField] GameObject visualDestructed;
        [SerializeField] bool destroyPermanent;
        [SerializeField] int destroyTime;
        [SerializeField] Transform OrderInLayerSpriteAnchor;

        [Header("Other Sprites")]
        [SerializeField] SpriteRenderer wallVerticalDownVisualSprite;
        [SerializeField] SpriteRenderer wallDoorCloseVisualSprite;
        [SerializeField] SpriteRenderer wallDoorOpen1VisualSprite;
        [SerializeField] SpriteRenderer wallDoorOpen2VisualSprite;
        [SerializeField] Transform spriteParent;
        SpriteRenderer[] visualSprites;
        SpriteRenderer visualSprite;
        SpriteRenderer visualDestructedSprite;

        // Kule yapay zekası için gerekli değerler
        [HideInInspector] public bool isFull;
        [HideInInspector] public bool destruct;
        [HideInInspector] public int unitValue;

        [HideInInspector] public InteractableObjUIController interactableObjUIController;
        HealthController healthController;
        DynamicOrderInLayer dynamicOrderInLayer;
        bool workOnce = true;

        private void Awake()
        {
            healthController = GetComponent<HealthController>();
            interactableObjUIController = GetComponent<InteractableObjUIController>();
            dynamicOrderInLayer = new();
            visualSprite = visualBuilding.GetComponent<SpriteRenderer>();
            visualDestructedSprite = visualDestructed.GetComponent<SpriteRenderer>();
        }
        private void Start()
        {
            dynamicOrderInLayer.OrderInLayerInitialize(OrderInLayerSpriteAnchor, visualSprite);
            dynamicOrderInLayer.OrderInLayerInitialize(OrderInLayerSpriteAnchor, visualDestructedSprite);

            SpriteAssingment();

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
                Destroy(gameObject, 0);

            }
        }

        void SpriteAssingment()
        {
            if (wallVerticalDownVisualSprite != null)
                dynamicOrderInLayer.OrderInLayerInitialize(OrderInLayerSpriteAnchor, wallVerticalDownVisualSprite, -1);
            if (wallDoorCloseVisualSprite != null)
                dynamicOrderInLayer.OrderInLayerInitialize(OrderInLayerSpriteAnchor, wallDoorCloseVisualSprite, -1);
            if (wallDoorOpen1VisualSprite != null)
                dynamicOrderInLayer.OrderInLayerInitialize(OrderInLayerSpriteAnchor, wallDoorOpen1VisualSprite, -2);
            if (wallDoorOpen2VisualSprite != null)
                dynamicOrderInLayer.OrderInLayerInitialize(OrderInLayerSpriteAnchor, wallDoorOpen2VisualSprite, -2);
            if (spriteParent != null)
            {
                visualSprites = new SpriteRenderer[spriteParent.childCount];
                for (int i = 0; i < spriteParent.childCount; i++)
                {
                    visualSprites[i] = spriteParent.GetChild(i).GetComponent<SpriteRenderer>();
                }
                dynamicOrderInLayer.OrderInLayerInitialize(OrderInLayerSpriteAnchor, visualSprites, 1);
            }
        }
    }
}