using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    class BuildingController : MonoBehaviour
    {
        public GameObject construction;
        public Collider2D physicalCollider;
        bool workOnce = true;
        [SerializeField] bool destroyPermanent;
        [SerializeField] int destroyTime;

        // Kule yapay zekası için gerekli değerler
        [HideInInspector] public bool isFull;
        [HideInInspector] public bool destruct;
        [HideInInspector] public int unitValue;

        GameObject visualBuilding;
        GameObject visualDestructed;
        ButtonController buttonController;
        [HideInInspector] public BuildingPanelController buildingPanelController;
        HealthController healthController;
        private void Awake()
        {
            healthController = GetComponent<HealthController>();
            visualBuilding = transform.GetChild(2).gameObject;
            visualDestructed = transform.GetChild(3).gameObject;
            buttonController = GetComponent<ButtonController>();
            buildingPanelController = GetComponent<BuildingPanelController>();
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
            Debug.Log("Destruct");
            destruct = true;
            physicalCollider.enabled = false;
            healthController.enabled = false;
            healthController.HealthBarVisibility(false);
            visualBuilding.SetActive(false);
            visualDestructed.SetActive(true);
            gameObject.layer = 26; // Katman = Destructed
            if (buildingPanelController != null)
                buildingPanelController.InteractablePanelVisibility(false);
            workOnce = false;
            if (destroyPermanent)
                Destroy(gameObject, destroyTime);
        }

        void Upgrade()
        {
            if (buttonController == null) return;
            if (buttonController.upgrade)
            {
                Debug.Log("Upgrade");
                GameObject obj = Instantiate(buttonController.construct, transform.position, Quaternion.identity);
                obj.GetComponent<ConstructController>().constructing = buttonController.upgradedBuilding;
                Destroy(gameObject, destroyTime);

            }
        }
    }
}