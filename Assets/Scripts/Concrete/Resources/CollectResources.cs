using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectResources : MonoBehaviour
    {
        public GameObject targetResource;
        [Header("Tree")]
        public GameObject nearestTree;
        Vector2 nearestTreeChopPos;
        public int treeDamagePoint;
        public float chopSpeed;
        public float collectTime;
        public float chopTreeSightRange;
        public LayerMask treeLayer;
        public Vector3 distanceToTreePos;
        public Collider2D[] trees;

        [Header("Mine")]
        public float miningTime;
        public int collectGoldAmount;
        public int collectRockAmount;
        public int collectWoodAmount;
        public GameObject resourceGold;
        public GameObject resourceRock;
        public GameObject resourceWood;


        [HideInInspector] public Vector3 homePos;
        float currentChopTreeSightRange;
        int currentTreeDamagePoint;
        float tMining;
        float tChop;
        float tCollect;
        bool returnHome;
        bool workOnce;
        bool workOnce2;
        bool workOnce3 = true;
        public bool isMineEmpty;
        public bool isTree;
        public bool isMine;

        UnitController uC;
        PathFinding2D pF2D;
        SpriteRenderer villagerSpriteRenderer;
        Animator animator;
        GameObject goldIdle;
        GameObject rockIdle;
        GameObject woodIdle;
        Direction direction;
        AnimationEventController animationEventController;
        Tree tree;
        IInput ıInput;



        private void Awake()
        {
            uC = GetComponent<UnitController>();
            pF2D = GetComponent<PathFinding2D>();
            villagerSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            animator = transform.GetChild(0).GetComponent<Animator>();
            goldIdle = transform.GetChild(1).gameObject;
            rockIdle = transform.GetChild(2).gameObject;
            woodIdle = transform.GetChild(3).gameObject;
            ıInput = new PcInput();
            animationEventController = transform.GetChild(0).GetComponent<AnimationEventController>();
        }
        private void Start()
        {
            direction = uC.direction;
            currentTreeDamagePoint = treeDamagePoint;
            currentChopTreeSightRange = chopTreeSightRange;

            //  Events
            animationEventController.ChopEvent += Chop;
            animationEventController.GetHitTreeEvent += GetHitTree;
            // animationEventController.IsTreeBeingCutAlreadyEvent += IsTreeBeingCutAlready;

            InvokeRepeating(nameof(OptimumTurn2Direction), .1f, .5f);

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
                if (woodIdle.activeSelf)
                {
                    DropWood(transform.position);
                    woodIdle.SetActive(false);
                }
                // Köylü seçili iken, etkileşimli olmayan bir nesne seçildiğinde seçim kalkar 
                if (InteractManager.Instance.interactedObj == null)
                {
                    uC.isSeleceted = false;
                }
            }
            SelectResourceType();
            GoToMine();
            GoToTree();
            GoToHome();
        }
        void SelectResourceType()
        {
            // UPDATE İLE ÇALIŞIR
            // Eğer köylü seçiliyse ve hedef kaynağa tıkladıysa, seçili köylünün hedefi seçili kaynaktır
            if (uC.isSeleceted)
            {
                targetResource = null;
                nearestTree = null;
                villagerSpriteRenderer.enabled = true;
                isMine = false;
                isMineEmpty = false;
                isTree = false;
                // Maden
                if (InteractManager.Instance.interactedMine != null)
                {
                    // SADECE 1 KEZ ÇALIŞIR
                    isTree = false;
                    returnHome = false;
                    targetResource = InteractManager.Instance.interactedMine;
                    isMine = true;
                    uC.isSeleceted = false;

                }
                // Ağaç
                if (InteractManager.Instance.interactedTree != null)
                {
                    // SADECE 1 KEZ ÇALIŞIR
                    isMine = false;
                    returnHome = false;
                    targetResource = InteractManager.Instance.interactedTree;
                    isTree = true;
                    workOnce3 = true;
                    uC.isSeleceted = false;
                }
            }
        }
        void RefreshTrees()
        {
            trees = Physics2D.OverlapCircleAll(targetResource.transform.position, currentChopTreeSightRange, treeLayer);
            if (trees.Length == 0)
                isTree = false;
        }
        void GoToMine()
        {
            //ağaç seçliyse veya maden boşsa veya maden seçilmediyse, madene gitme
            if (isTree || isMineEmpty || !isMine) return;
            // Hedef varsa ona git
            if (targetResource != null && !returnHome)
            {
                // Hedefe ulaşınca dur
                if (Vector2.Distance(transform.position, targetResource.transform.position) > .5f)
                {
                    pF2D.AIGetMoveCommand(targetResource.transform.position);
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
        void GoToTree()
        {
            // Ağaç seçilmediyse veya maden seçildiyse ağaç kesmeye gitme.
            if (!isTree || isMine) return;

            // Hedef varsa ona git
            if (targetResource != null && !returnHome)
            {
                // İlk ağacı bulur
                if (workOnce3)
                {
                    RefreshTrees();
                    nearestTree = DetechNearestTree();
                    if (nearestTree != null)
                    {
                        nearestTreeChopPos = CalculateNearestChopPos(nearestTree);
                        tree = nearestTree.GetComponent<Tree>();
                    }
                    workOnce3 = false;
                }

                // Hedef boşsa çalışma
                if (nearestTree == null) return;
                // Kullanıcı komutu bittiği zaman, kendi kendine ağaca doğru gider
                if (Vector2.Distance(transform.position, nearestTreeChopPos) > .1f)
                {
                    pF2D.AIGetMoveCommand(nearestTreeChopPos);
                    AnimationManager.Instance.RunAnim(animator, 1);

                    if (tree.isTreeAlreadyCutted)
                    {
                        workOnce3 = true;
                        return;
                    }
                }

                // Ağacı Kes
                if (Vector2.Distance(transform.position, nearestTreeChopPos) < .1f)
                {
                    AnimationManager.Instance.ChopAnim(animator, chopSpeed);
                }
            }
        }
        void Chop()  // Ağaç kesme animasyonu oynadığında; Chop event i ile tetiklenir
        {
            // Ağacı kes
            if (nearestTree != null)
            {
                tree = nearestTree.GetComponent<Tree>();
                tree.GetHit(currentTreeDamagePoint, collectTime);
                //Ağaç yıkıldıysa eve dön
                if (tree.destruct)
                {
                    if (!tree.isTreeAlreadyCutted)
                    {
                        returnHome = true;
                        tCollect = 0;
                        print("work");
                        AnimationManager.Instance.IdleAnim(animator);
                        tree.IsTreeAlreadyCutted(true);
                    }

                    if (tree.isTreeAlreadyCutted)
                    {
                        workOnce3 = true;
                    }
                }

            }

            workOnce = true;
            workOnce2 = true;
        }
        void GetHitTree() // Ağaç kesme animasyonu oynadığında; GetHitTree event i ile tetiklenir
        {
            if (nearestTree != null)
            {
                tree = nearestTree.GetComponent<Tree>();
                tree.GetHitTreeAnim(direction.Turn2Direction(nearestTree.transform.position.x), chopSpeed);

            }
        }
        void GoToHome()
        {
            // Köylüyü madende çalışırken maden biterse, tekarar görünür olur
            if (isMineEmpty)
                villagerSpriteRenderer.enabled = true;

            if (returnHome)
            {
                // Hedefe ulaşınca dur
                if (Vector2.Distance(transform.position, homePos) > .5f)
                {
                    tCollect += Time.deltaTime;
                    if (workOnce && tCollect > collectTime && isTree || isMine)
                    {
                        pF2D.AIGetMoveCommand(homePos);
                        AnimationManager.Instance.RunCarryAnim(animator, 1);
                        CollectResource();
                        workOnce = false;
                        tCollect = 0;
                    }
                }

                // Hedefe ulaşıldı
                else
                {
                    DropResource();
                    returnHome = false;
                    workOnce3 = true;
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
            if (isTree)
                woodIdle.SetActive(true);
        }
        void DropResource()
        {
            // Kaynakları eve bırak
            if (workOnce2)
            {
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
                if (woodIdle.activeSelf)
                {
                    ResourcesManager.wood += collectWoodAmount;
                    DropWood(homePos);
                    woodIdle.SetActive(false);
                }
                workOnce2 = false;
            }
        }
        void OptimumTurn2Direction()
        {
            if (targetResource == null) return;
            if (!returnHome)
            {
                if (isMine)
                    direction.Turn2Direction(targetResource.transform.position.x);
                if (isTree && nearestTree != null)
                    direction.Turn2Direction(nearestTree.transform.position.x);
            }
            if (returnHome)
                direction.Turn2Direction(homePos.x);

        }
        GameObject DetechNearestTree()
        {
            print("DetechNearestTree");
            GameObject nearestTarget = null;
            float shortestDistance = Mathf.Infinity;

            for (int i = 0; i < trees.Length; i++)
            {
                if (trees[i] != null)
                {
                    float distanceToEnemy = Vector2.Distance(transform.position, trees[i].transform.position);

                    if (shortestDistance > distanceToEnemy)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestTarget = trees[i].gameObject;
                    }
                }
            }

            return nearestTarget;
        }
        Vector2 CalculateNearestChopPos(GameObject obj) // Hedefin kesilecek noktalarından en yakınını bulur
        {
            print("CalculateNearestChopPos");
            float distance1 = Vector2.Distance(obj.transform.GetChild(1).position, transform.position);
            float distance2 = Vector2.Distance(obj.transform.GetChild(2).position, transform.position);
            if (distance1 < distance2)
            {
                return obj.transform.GetChild(1).position;
            }

            else
                return obj.transform.GetChild(2).position;
        }
        void DropWood(Vector3 pos)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject wood = Instantiate(resourceWood, pos + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0), Quaternion.identity);
                Destroy(wood, 3);
            }
        }
        private void OnDrawGizmos()
        {
            if (!isTree) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(targetResource.transform.position, currentChopTreeSightRange);
        }


    }
}
