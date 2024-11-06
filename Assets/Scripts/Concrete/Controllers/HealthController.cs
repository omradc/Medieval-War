using Assets.Scripts.Concrete.Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class HealthController : MonoBehaviour
    {
        [SerializeField] GameObject healthObj;
        [SerializeField] float health;
        [SerializeField] float currentHealth;
        [SerializeField] float regeneration = 1;
        public float takeDamageTime = 5;
        public float regrenationPerTime = 1;
        [HideInInspector] public bool isTakeDamage;
        [HideInInspector] public float currentTakeDamageTime;
        [HideInInspector] public float currentRegrenationPerTime;
        Image fillImage;
        private void Awake()
        {
            currentHealth = health;
            fillImage = healthObj.transform.GetChild(1).GetComponent<Image>();

        }
        private void Start()
        {
            DisplayHealthBar();
            Visibility(false);

            InvokeRepeating(nameof(Regeneration), 0, 1);
            InvokeRepeating(nameof(WhenHealthIsFullSetVisibility), 0, 5);
        }
        //private void Update()
        //{
        //    Regeneration();
        //}
        public void GetHit(int attackDamage)
        {
            currentTakeDamageTime = 0;
            isTakeDamage = true;
            Visibility(true);
            currentHealth -= attackDamage;
            DisplayHealthBar();
            if (currentHealth <= 0)
            {
                // oyucu birimi ise, seçili birimlerden kaldır
                if (gameObject.layer == 6)
                    InteractManager.Instance.selectedUnits.Remove(gameObject);
                fillImage.fillAmount = 0;
                Dead();
            }
        }

        void Regeneration()
        {
            currentTakeDamageTime += 1;
            DisplayHealthBar();
            if (currentTakeDamageTime >= takeDamageTime)
            {
                isTakeDamage = false;
            }

            if (!isTakeDamage)
            {
                // Can dolu
                if (currentHealth >= health)
                {
                    return;
                }
                currentRegrenationPerTime += 1;
                if (currentRegrenationPerTime >= regrenationPerTime)
                {
                    currentHealth += regeneration;
                    currentRegrenationPerTime = 0;
                }
            }
        }

        void WhenHealthIsFullSetVisibility()
        {
            if (currentHealth >= health)
                Visibility(false);
        }
        void Visibility(bool visibility)
        {
            healthObj.SetActive(visibility);
        }

        public void Dead()
        {
            Destroy(gameObject);
        }
        void DisplayHealthBar()
        {
            fillImage.fillAmount = currentHealth / health;
        }
    }
}
