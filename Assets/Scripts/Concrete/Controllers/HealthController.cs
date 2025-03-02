using Assets.Scripts.Concrete.Combats;
using Assets.Scripts.Concrete.Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class HealthController : MonoBehaviour
    {
        [SerializeField] GameObject healthObj;
        [HideInInspector] public int health;
        [HideInInspector] public bool regeneration;
        [HideInInspector] public float regenerationAmount = 1;
        [HideInInspector] public float regrenationAfterDamageTime = 5;
        [HideInInspector] public float regrenationPerTime = 1;
        [HideInInspector] public float currentregrenationAfterDamageTime;
        [HideInInspector] public float currentRegrenationPerTime;
        [HideInInspector] public bool isTakeDamage;
        [HideInInspector] public bool isDead;
        [HideInInspector] public GoblinHouseController goblinHouseController;
        [HideInInspector] public KnightHouseController knightHouseController;
        [HideInInspector] public int elevationFloor;
        Image fillImage;
        List<GameObject> attackingPersons;
        TargetPriority targetPriority;
        public float currentHealth;
        RaycastHit2D hitElevation;
        private void Awake()
        {
            fillImage = healthObj.transform.GetChild(1).GetComponent<Image>();

        }
        private void Start()
        {
            currentHealth = health;
            attackingPersons = new();
            targetPriority = GetComponent<TargetPriority>();
            UpdateHealthBar();
            HealthBarVisibility(false);

            InvokeRepeating(nameof(Regeneration), 0, 1);
            InvokeRepeating(nameof(WhenHealthIsFullSetVisibility), 0, 5);
            InvokeRepeating(nameof(AttackingPersonsAssignment), 0, 0.5f);
            InvokeRepeating(nameof(ElevationControl), 0, 1f);
        }

        public void GetHit(int attackDamage, GameObject attacker) // Hasar al
        {
            AttackingPerson(attacker);
            currentregrenationAfterDamageTime = 0;
            isTakeDamage = true;
            HealthBarVisibility(true);
            currentHealth -= attackDamage;
            UpdateHealthBar();
            if (currentHealth <= 0)
            {
                // oyucu birimi ise, seçili birimlerden kaldır
                if (gameObject.layer == 6)
                    InteractManager.Instance.selectedUnits.Remove(gameObject);
                fillImage.fillAmount = 0;
                Dead();
            }
        }
        void Regeneration() // Yenilenme
        {
            if (!regeneration) return;
            currentregrenationAfterDamageTime += 1;
            UpdateHealthBar();
            if (currentregrenationAfterDamageTime >= regrenationAfterDamageTime)
            {
                isTakeDamage = false;
            }

            if (!isTakeDamage)
            {
                // Can dolu
                if (currentHealth >= health) return;

                currentRegrenationPerTime += 1;
                if (currentRegrenationPerTime >= regrenationPerTime)
                {
                    currentHealth += regenerationAmount;
                    currentRegrenationPerTime = 0;
                }
            }
        }
        void WhenHealthIsFullSetVisibility()
        {
            if (currentHealth >= health)
            {
                HealthBarVisibility(false);
            }
        }
        public void HealthBarVisibility(bool visibility)
        {
            healthObj.SetActive(visibility);
        }
        public void Dead()
        {
            isDead = true;

            // Oyuncu birimi
            if (gameObject.layer == 6)
            {
                if (knightHouseController != null)
                    knightHouseController.currentTrainedKnightNumber--;  // Ölmeden önce knight sayısını azaltır 
                Destroy(gameObject);
            }
            // Düşman birimi
            if (gameObject.layer == 13)
            {
                if (goblinHouseController != null)
                    goblinHouseController.currentGoblinNumber--; // Ölmeden önce goblin sayısını azaltır 
                Destroy(gameObject);
            }
            if(gameObject.CompareTag("Repo"))
            {
                ResourcesManager.Instance.RemoveRepo(gameObject);
            }
        }
        void UpdateHealthBar()
        {
            fillImage.fillAmount = currentHealth / health;
        }
        public void FillHealth()
        {
            currentHealth = health;
            isDead = false;
        }
        void AttackingPerson(GameObject attacker)
        {
            if (!attackingPersons.Contains(attacker) && attacker != null)
                attackingPersons.Add(attacker);
        }
        void AttackingPersonsAssignment()
        {


            if (attackingPersons.Count <= targetPriority.maxAttacker)
                targetPriority.attackingPersonNumber = 0;
            else
                targetPriority.attackingPersonNumber = attackingPersons.Count - targetPriority.maxAttacker;
            for (int i = 0; i < attackingPersons.Count; i++)
            {
                if (attackingPersons[i] == null)
                    attackingPersons.RemoveAt(i);
            }
        }
        void ElevationControl()
        {
            hitElevation = Physics2D.Raycast(transform.position, transform.forward, .05f, LayerMask.GetMask("GroundElevations1", "GroundElevations2", "GroundElevations3"));
            if (hitElevation.collider != null)
            {
                if (hitElevation.collider.gameObject.layer == 21) //1. yükselti katı
                    elevationFloor = 1;
                else if (hitElevation.collider.gameObject.layer == 22) //2. yükselti katı
                    elevationFloor = 2;
                else if (hitElevation.collider.gameObject.layer == 23) //3. yükselti katı
                    elevationFloor = 3;
            }
            else
                elevationFloor = 0;

        }
    }
}
