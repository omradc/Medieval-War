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
        public Collider2D col3;
        GameObject visualTower;
        GameObject visualDestructed;
        bool workOnce = true;
        [HideInInspector] public int unitValue;

        HealthController healthController;
        private void Awake()
        {
            healthController = GetComponent<HealthController>();
            visualTower = transform.GetChild(2).gameObject;
            visualDestructed = transform.GetChild(3).gameObject;
        }

        private void Update()
        {
            if (healthController.isDead)
            {
                destruct = true;
            }

            if (destruct)
            {
                Destruct();
            }
            if (rebuild)
            {
                ReBuild();
            }
        }

        void Destruct()
        {
            if (!workOnce) return;
            Debug.Log("Destruct");
            col1.enabled = false;
            col2.enabled = false;
            col3.enabled = true;
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
            col3.enabled = false;
            healthController.enabled = true;
            visualTower.SetActive(true);
            visualDestructed.SetActive(false);
            gameObject.layer = 9; // Katman = Tower
            healthController.FillHealth();
            rebuild = false;
        }
    }
}