using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    public class TowerController : MonoBehaviour
    {
        public bool isFull;
        public bool destruct;
        public bool rebuild;
        public Collider2D col1;
        public Collider2D col2;
        GameObject visualTower;
        GameObject visualDestructed;
        bool workOnce = true;
        [HideInInspector] public int unitValue;
        ButtonController buttonController;

        HealthController healthController;
        private void Awake()
        {
            healthController = GetComponent<HealthController>();
            visualTower = transform.GetChild(2).gameObject;
            visualDestructed = transform.GetChild(3).gameObject;
            buttonController = GetComponent<ButtonController>();
        }

        private void Update()
        {
            if (healthController.isDead)
                destruct = true;
            if (destruct)
                Destruct();
            if (rebuild)
                ReBuild();

            Upgrade();

        }

        void Destruct()
        {
            if (!workOnce) return;
            Debug.Log("Destruct");
            col1.enabled = false;
            col2.enabled = false;
            healthController.enabled = false;
            healthController.HealthBarVisibility(false);
            visualTower.SetActive(false);
            visualDestructed.SetActive(true);
            gameObject.layer = 26; // Katman = Destructed
            workOnce = false;
        }

        void ReBuild()
        {
            Debug.Log("ReBuild");
            destruct = false;
            workOnce = true;
            col1.enabled = true;
            col2.enabled = true;
            healthController.enabled = true;
            visualTower.SetActive(true);
            visualDestructed.SetActive(false);
            gameObject.layer = 9; // Katman = Tower
            healthController.FillHealth();
            rebuild = false;
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