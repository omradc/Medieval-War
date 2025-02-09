using Assets.Scripts.Concrete.Combats;
using Assets.Scripts.Concrete.Managers;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class HealthController : MonoBehaviour
    {
        [SerializeField] GameObject healthObj;
        [SerializeField] float health;
        [SerializeField] float currentHealth;
        [SerializeField] bool regeneration;
        [SerializeField] float regenerationValue = 1;

        public float takeDamageTime = 5;
        public float regrenationPerTime = 1;
        [HideInInspector] public bool isTakeDamage;
        [HideInInspector] public float currentTakeDamageTime;
        [HideInInspector] public float currentRegrenationPerTime;
        [HideInInspector] public bool isDead;
        [HideInInspector] public GoblinHouseController goblinHouseController;
        Image fillImage;
        public List<GameObject> attackingPersons;
        TargetPriority targetPriority;
        private void Awake()
        {
            currentHealth = health;
            fillImage = healthObj.transform.GetChild(1).GetComponent<Image>();

        }
        private void Start()
        {
            targetPriority = GetComponent<TargetPriority>();
            UpdateHealthBar();
            HealthBarVisibility(false);

            InvokeRepeating(nameof(Regeneration), 0, 1);
            InvokeRepeating(nameof(WhenHealthIsFullSetVisibility), 0, 5);
            InvokeRepeating(nameof(AttackingPersonsAssignment), 0, 0.5f);
        }

        public void GetHit(int attackDamage, GameObject attacker) // Hasar al
        {
            AttackingPerson(attacker);
            currentTakeDamageTime = 0;
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
            currentTakeDamageTime += 1;
            UpdateHealthBar();
            if (currentTakeDamageTime >= takeDamageTime)
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
                    currentHealth += regenerationValue;
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
                Destroy(gameObject);
            // Düşman birimi
            if (gameObject.layer == 13)
            {
                if (goblinHouseController != null)
                    goblinHouseController.currentGoblinNumber--; // Ölmeden önce goblin sayısını azaltır 
                Destroy(gameObject);
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
    }
}
