using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    public class BuildingController : MonoBehaviour
    {
        public GameObject construction;
        public Collider2D physicalCollider;
        bool workOnce = true;

        // Kule yapay zekası için gerekli değerler
        [HideInInspector] public bool isFull;
        public bool destruct;
        [HideInInspector] public int unitValue;

        GameObject visualTower;
        GameObject visualDestructed;
        ButtonController buttonController;
        BuildingPanelController bC;
        HealthController healthController;
        private void Awake()
        {
            healthController = GetComponent<HealthController>();
            visualTower = transform.GetChild(2).gameObject;
            visualDestructed = transform.GetChild(3).gameObject;
            buttonController = GetComponent<ButtonController>();
            bC = GetComponent<BuildingPanelController>();
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
            visualTower.SetActive(false);
            visualDestructed.SetActive(true);
            gameObject.layer = 26; // Katman = Destructed
            bC.InteractablePanelVisibility(false);
            workOnce = false;
        }

        void Upgrade()
        {
            if (buttonController.upgrade)
            {
                Debug.Log("Upgrade");
                GameObject obj = Instantiate(buttonController.construct, transform.position, Quaternion.identity);
                obj.GetComponent<ConstructController>().constructing = buttonController.upgradedBuilding;
                Destroy(gameObject);

            }
        }
    }
}