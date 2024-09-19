using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using System.Security.Cryptography;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectResources : MonoBehaviour
    {
        public GameObject targetResource;
        public Vector3 homePos;
        public GameObject resourceGold;
        public GameObject resourceRock;


        public float miningTime;
        public float returnHomeTime;
        public int collectGoldAmount;
        public int collectRockAmount;
        float tMining;
        float tReturnHome;
        bool returnHome;
        bool workOnce;
        bool workOnce2;
        public bool isMineEmpty;
        bool isMine;
        bool isTree;
        bool isSheep;

        UnitController uC;
        PathFinding2D pF2D;
        SpriteRenderer villagerSpriteRenderer;
        Animator animator;
        GameObject goldIdle;
        GameObject rockIdle;
        Direction direction;
        IInput ıInput;



        private void Awake()
        {
            uC = GetComponent<UnitController>();
            pF2D = GetComponent<PathFinding2D>();
            villagerSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            animator = transform.GetChild(0).GetComponent<Animator>();
            goldIdle = transform.GetChild(1).gameObject;
            rockIdle = transform.GetChild(2).gameObject;
            ıInput = new PcInput();
        }
        private void Start()
        {
            direction = uC.direction;
            InvokeRepeating(nameof(OptimumTurn2Direction),.1f,.5f);

        }
        private void Update()
        {
            // Eğer elinde kaynak varken seçip, başka bir yere gönderirsen. Kaynak yere düşer.
            if (ıInput.GetButtonDown0 && uC.isSeleceted)
            {
                if (goldIdle.activeSelf)
                {
                    Instantiate(resourceGold, transform.position, Quaternion.identity);
                    goldIdle.SetActive(false);
                }
                if (rockIdle.activeSelf)
                {
                    Instantiate(resourceRock, transform.position, Quaternion.identity);
                    rockIdle.SetActive(false);
                }
            }

            SelectResourceType();
            GoToMine();
            // GoToTree();
            GoToHome();
        }
        void SelectResourceType()
        {
            // Eğer köylü seçiliyse ve hedef kaynağa tıkladıysa, seçili köylünün hedefi seçili kaynaktır
            if (uC.isSeleceted)
            {
                targetResource = null;


                if (InteractManager.Instance.interactedMine != null)
                {
                    isTree = false;
                    isSheep = false;
                    returnHome = false;
                    targetResource = InteractManager.Instance.interactedMine;
                    isMine = true;
                }
                if (InteractManager.Instance.interactedTree != null)
                {
                    isMine = false;
                    isSheep = false;
                    returnHome = false;
                    targetResource = InteractManager.Instance.interactedTree;
                    isTree = true;
                }
            }
        }
        void GoToMine()
        {
            if (uC.isSeleceted)
            {
                villagerSpriteRenderer.enabled = true;
                isMineEmpty = false;
                if (!isMine) return;
            }

            // Eğer Maden bittiyse
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
                            mine.mineAmountFillValue.fillAmount = mine.currentMineAmount / mine.mineAmount;
                        }
                        if (mine.CompareTag("RockMine"))
                        {
                            mine.currentMineAmount -= collectRockAmount;
                            mine.mineAmountFillValue.fillAmount = mine.currentMineAmount / mine.mineAmount;
                        }
                        villagerSpriteRenderer.enabled = true;
                        returnHome = true;
                        workOnce = true;
                        workOnce2 = true;
                        tMining = 0;
                    }

                }
            }
        }
        void GoToHome()
        {
            // Köylüyü madende çalışırken çağrılırsa, tekarar görünür olur
            if (isMineEmpty)
                villagerSpriteRenderer.enabled = true;

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
                        CollectResource();
                    }
                }

                // Hedefe ulaşıldı
                else
                {
                    DropResource(isMine);
                    returnHome = false;
                }
            }
        }
        void CollectResource()
        {
            if (isMine)
            {
                Mine mine = targetResource.GetComponent<Mine>();
                if (mine.CompareTag("GoldMine"))
                    goldIdle.SetActive(true);
                if (mine.CompareTag("RockMine"))
                    rockIdle.SetActive(true);
            }
        }

        void DropResource(bool _isMine)
        {
            // Kaynakları eve bırak
            if (_isMine && workOnce2)
            {
                workOnce2 = false;
                print("Dropped");

                if (goldIdle.activeSelf)
                {
                    ResourcesManager.gold += collectGoldAmount;
                    Instantiate(resourceGold, homePos, Quaternion.identity);
                    goldIdle.SetActive(false);
                }
                if (rockIdle.activeSelf)
                {
                    ResourcesManager.rock += collectRockAmount;
                    Instantiate(resourceRock, homePos, Quaternion.identity);
                    rockIdle.SetActive(false);
                }
                villagerSpriteRenderer.enabled = true;
            }
        }

        void OptimumTurn2Direction()
        {
            if (targetResource != null && !returnHome)
                direction.Turn2Direction(targetResource.transform.position.x);
            if (returnHome)
                direction.Turn2Direction(homePos.x);

        }

    }
}
