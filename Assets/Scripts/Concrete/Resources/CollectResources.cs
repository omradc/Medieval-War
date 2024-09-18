using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectResources : MonoBehaviour
    {
        public GameObject targetResource;
        public Vector3 homePos;
        public GameObject goldBag;
        public GameObject rockBag;

        UnitController uC;
        PathFinding2D pF2D;
        SpriteRenderer villagerSpriteRenderer;
        Animator animator;
        GameObject goldIdle;
        GameObject rockIdle;
        public float miningTime;
        public float returnHomeTime;
        public int collectGoldAmount;
        public int collectRockAmount;
        float tMining;
        float tReturnHome;
        bool returnHome;
        bool workOnce;
        public bool isMineEmpty;
        private void Awake()
        {
            uC = GetComponent<UnitController>();
            pF2D = GetComponent<PathFinding2D>();
            villagerSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            animator = transform.GetChild(0).GetComponent<Animator>();
            goldIdle = transform.GetChild(1).gameObject;
            rockIdle = transform.GetChild(2).gameObject;
        }
        private void Start()
        {

        }
        private void Update()
        {
            GoToResource();
            GoToHome();
        }

        void GoToResource()
        {

            // Eğer köylü seçiliyse ve hedef kaynağa tıkladıysa, seçili köylünün hedefi seçili kaynaktır
            if (uC.isSeleceted)
            {
                isMineEmpty = false;
                returnHome = false;
                villagerSpriteRenderer.enabled = true;
                targetResource = null;
                if (InteractManager.Instance.interactedMine != null)
                {
                    targetResource = InteractManager.Instance.interactedMine;
                }
            }
            if (isMineEmpty) return;
            // Hedef varsa ona git
            if (targetResource != null && !returnHome)
            {
                // Hedefe ulaşınca dur
                if (Vector2.Distance(transform.position, targetResource.transform.position) > .5f)
                {
                    pF2D.AIGetMoveCommand(targetResource.transform.position);
                    goldIdle.SetActive(false);
                    AnimationManager.Instance.RunAnim(animator, 1);
                }

                // Hedefe ulaşıldı
                else
                {
                    if (!returnHome)
                        villagerSpriteRenderer.enabled = false;

                    tMining += Time.deltaTime;
                    if (tMining > miningTime)
                    {
                        // Madenden alınan kaynakları eksilt
                        Mine mine = targetResource.GetComponent<Mine>();
                        if (mine.CompareTag("GoldMine"))
                        {
                            mine.currentMineAmount -= collectGoldAmount;
                            mine.mineAmountFillValue.fillAmount=mine.currentMineAmount/mine.mineAmount;
                        }
                        if (mine.CompareTag("RockMine"))
                        {
                            mine.currentMineAmount -= collectRockAmount;
                            mine.mineAmountFillValue.fillAmount = mine.currentMineAmount / mine.mineAmount;
                        }
                        villagerSpriteRenderer.enabled = true;
                        returnHome = true;
                        workOnce = true;
                        tMining = 0;
                    }

                }
            }


        }
        void GoToHome()
        {

            if (uC.isSeleceted) return;
            if (isMineEmpty)
            {
                villagerSpriteRenderer.enabled = true;
            }
            if (returnHome)
            {
                // Hedefe ulaşınca dur
                if (Vector2.Distance(transform.position, homePos) > .5f)
                {
                    if (workOnce)
                    {
                        pF2D.AIGetMoveCommand(homePos);
                        AnimationManager.Instance.RunCarry(animator, 1);
                        workOnce = false;
                        
                        Mine mine = targetResource.GetComponent<Mine>();
                        if (mine.CompareTag("GoldMine"))
                            goldIdle.SetActive(true);
                        if (mine.CompareTag("RockMine"))
                            rockIdle.SetActive(true);

                    }
                }

                // Hedefe ulaşıldı
                else
                {
                    // Kaynakları eve bırak
                    Mine mine = targetResource.GetComponent<Mine>();
                    if (mine.CompareTag("GoldMine"))
                    {
                        ResourcesManager.gold += collectGoldAmount;
                        Instantiate(goldBag, homePos, Quaternion.identity);
                        goldIdle.SetActive(false);
                    }
                    if (mine.CompareTag("RockMine"))
                    {
                        ResourcesManager.rock += collectRockAmount;
                        Instantiate(rockBag, homePos, Quaternion.identity);
                        rockIdle.SetActive(false);
                    }
                    villagerSpriteRenderer.enabled = true;
                    returnHome = false;

                }
            }
        }
    }
}
