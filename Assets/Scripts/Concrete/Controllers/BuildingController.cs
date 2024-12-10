using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    public class BuildingController : MonoBehaviour
    {
        [HideInInspector] public bool isFull;
        [HideInInspector] public GameObject thisObj;
        [HideInInspector] public GameObject construct;
        [HideInInspector] public bool destruct;
        public Collider2D physicalCollider;
        GameObject visualTower;
        [HideInInspector] public GameObject destructed;
        bool workOnce = true;
        [HideInInspector] public int unitValue;
        ButtonController buttonController;

        HealthController healthController;
        private void Awake()
        {
            healthController = GetComponent<HealthController>();
            visualTower = transform.GetChild(2).gameObject;
            destructed = transform.GetChild(3).gameObject;
            buttonController = GetComponent<ButtonController>();
            thisObj = gameObject;
            construct = transform.GetChild(4).gameObject;
        }

        private void Update()
        {
            Upgrade();
            Destruct();
        }

        void Destruct()
        {
            if (healthController.isDead)
            {
                if (!workOnce) return;
                Debug.Log("Destruct");
                destruct = true;
                physicalCollider.enabled = false;
                healthController.enabled = false;
                healthController.HealthBarVisibility(false);
                visualTower.SetActive(false);
                destructed.SetActive(true);
                gameObject.layer = 26; // Katman = Destructed
                workOnce = false;

            }
        }

        void Upgrade()
        {
            if (buttonController.upgrade)
            {
                Debug.Log("Upgrade");
                GameObject obj = Instantiate(buttonController.upgrading, transform.position, Quaternion.identity);
                obj.GetComponent<ConstructController>().building = buttonController.upgradeComplete;
                Destroy(gameObject);

            }
        }
    }
}