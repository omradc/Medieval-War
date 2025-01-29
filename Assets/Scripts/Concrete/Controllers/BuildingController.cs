using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    class BuildingController : MonoBehaviour
    {
        public GameObject construction;
        bool workOnce = true;
        [SerializeField] bool destroyPermanent;
        [SerializeField] int destroyTime;

        // Kule yapay zekası için gerekli değerler
        public bool isFull;
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
            destruct = true;
            healthController.enabled = false;
            healthController.HealthBarVisibility(false);
            visualBuilding.SetActive(false);
            visualDestructed.SetActive(true);
            gameObject.layer = 26; // Katman = Destructed
            if (buildingPanelController != null)
                buildingPanelController.interactablePanel.SetActive(false);
            workOnce = false;
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
                Destroy(gameObject, destroyTime);

            }
        }
    }
}