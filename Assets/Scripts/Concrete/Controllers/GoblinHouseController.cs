using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Concrete.Enums;

namespace Assets.Scripts.Concrete.Controllers
{
    public class GoblinHouseController : MonoBehaviour
    {
        [Header("HOUSE")]
        [SerializeField] bool attackOrder;
        [SerializeField] int spawnTime;
        [SerializeField] int maxGoblin;


        [Header("SETUP")]
        [SerializeField] GameObject spawnedGoblin;
        [SerializeField] Transform spawnPoint;
        [SerializeField] GameObject timerPanel;

        public List<GameObject> goblins;
        Transform allGoblins;
        Image timerFillImage;
        HealthController healthController;
        float time;
        public int currentGoblinNumber;

        private void Awake()
        {
            allGoblins = GameObject.FindWithTag("Goblins").transform;
            goblins = new List<GameObject>();
            timerFillImage = timerPanel.transform.GetChild(1).GetComponent<Image>();
            healthController = GetComponent<HealthController>();

        }
        void Update()
        {
            if (healthController.isDead) // Ev yıkıldıysa işlevsizdir
            {
                timerPanel.SetActive(false);
                return;
            }
            GoblinSpawner();
            AttackOrder();
        }


        public void GoblinSpawner()
        {
            if (currentGoblinNumber == maxGoblin) return;
            timerPanel.SetActive(true);
            time += Time.deltaTime;
            timerFillImage.fillAmount = time / spawnTime;
            if (time > spawnTime && currentGoblinNumber < maxGoblin)
            {
                GameObject createdGoblin = Instantiate(spawnedGoblin, spawnPoint.position, transform.rotation, allGoblins);
                goblins.Add(createdGoblin);
                createdGoblin.GetComponent<HealthController>().goblinHouseController = this;
                currentGoblinNumber++;
                timerPanel.SetActive(false);
                time = 0;
            }
        }

        void AttackOrder()
        {
            if (attackOrder)
            {
                for (int i = 0; i < goblins.Count; ++i)
                {
                    goblins[i].GetComponent<GoblinController>().behavior = BehaviorEnum.FindNearestPlayerUnit;
                }
                attackOrder = false;
            }
        }
    }
}