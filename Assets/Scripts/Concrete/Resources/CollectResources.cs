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
        public int treeDamagePoint;
        public float chopTime;
        public float chopTreeSightRange;
        public LayerMask treeLayer;
        public Vector3 distanceToTreePos;
        public bool searchTree;
        public Collider2D[] trees;
        GameObject targetTree;

        [Header("Mine")]
        public float miningTime;
        public int collectGoldAmount;
        public int collectRockAmount;
        public int collectWoodAmount;
        public GameObject resourceGold;
        public GameObject resourceRock;
        public GameObject resourceWood;


        [HideInInspector] public Vector3 homePos;
        [HideInInspector] public bool isMine;
        float currentChopTreeSightRange;
        int currentTreeDamagePoint;
        float tMining;
        float tChop;
        float tReturnHome;
        bool returnHome;
        bool workOnce;
        bool workOnce2;
        public bool isMineEmpty;
        bool isTree;

        UnitController uC;
        PathFinding2D pF2D;
        SpriteRenderer villagerSpriteRenderer;
        Animator animator;
        GameObject goldIdle;
        GameObject rockIdle;
        GameObject woodIdle;
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
            woodIdle = transform.GetChild(3).gameObject;
            ıInput = new PcInput();
        }
        private void Start()
        {
            direction = uC.direction;
            currentTreeDamagePoint = treeDamagePoint;
            currentChopTreeSightRange = chopTreeSightRange;
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
                    Instantiate(resourceWood, transform.position, Quaternion.identity);
                    woodIdle.SetActive(false);
                }
            }

            SelectResourceType();
            GoToMine();
            GoToTree();
            GoToHome();
        }
        void SelectResourceType()
        {
            // Eğer köylü seçiliyse ve hedef kaynağa tıkladıysa, seçili köylünün hedefi seçili kaynaktır
            if (uC.isSeleceted)
            {
                targetResource = null;
                isMine = false;
                isTree = false;

                if (InteractManager.Instance.interactedMine != null)
                {
                    //isSheep = false;
                    isTree = false;
                    returnHome = false;
                    targetResource = InteractManager.Instance.interactedMine;
                    isMine = true;
                }
                if (InteractManager.Instance.interactedTree != null)
                {
                    //isSheep = false;
                    isMine = false;
                    returnHome = false;
                    targetResource = InteractManager.Instance.interactedTree;
                    RefreshTrees();
                    isTree = true;
                    searchTree = true;
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
            if (isTree) return;
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
            // Köylüyü madende çalışırken maden biterse, tekarar görünür olur
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
                        AnimationManager.Instance.RunCarryAnim(animator, 1);
                        workOnce = false;
                        CollectResource();
                    }
                }

                // Hedefe ulaşıldı
                else
                {
                    DropResource(isMine, isTree);
                    returnHome = false;
                }
            }
        }
        void GoToTree()
        {
            if (!isTree) return;
            // Hedef varsa ona git
            if (targetResource != null && !returnHome)
            {
                RefreshTrees();
                if (DetechNearestTree() == null)
                    return;
                targetTree = DetechNearestTree();
                // Hedefe ulaşınca dur
                if (Vector2.Distance(transform.position, CalculateNearestChopPos(targetTree)) > .1f)
                {
                    pF2D.AIGetMoveCommand(CalculateNearestChopPos(targetTree));
                    AnimationManager.Instance.RunAnim(animator, 1);
                }

                // Hedefe ulaşıldı
                else
                {
                    tChop += Time.deltaTime;
                    if (tChop > chopTime)
                    {
                        Tree tree = targetTree.GetComponent<Tree>();
                        tree.GetHit(currentTreeDamagePoint);

                        //Ağaç yıkıldıysa eve dön
                        if (tree.destruct)
                        {
                            returnHome = true;
                            targetTree.layer = default;
                        }
                        workOnce = true;
                        workOnce2 = true;
                        tChop = 0;
                    }

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
        void DropResource(bool _isMine, bool _isTree)
        {
            // Kaynakları eve bırak
            if (workOnce2)
            {
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


                if (_isTree && woodIdle.activeSelf)
                {
                    ResourcesManager.wood += collectWoodAmount;
                    for (int i = 0; i < 3; i++)
                    {
                        Instantiate(resourceWood, homePos + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0), Quaternion.identity);
                    }
                    woodIdle.SetActive(false);
                }
                workOnce2 = false;
            }
        }
        void OptimumTurn2Direction()
        {
            if (targetResource == null) return;
            if (!returnHome)
                direction.Turn2Direction(targetResource.transform.position.x);
            if (returnHome)
                direction.Turn2Direction(homePos.x);

        }
      
        GameObject DetechNearestTree()
        {
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
        // Hedefin kesilecek noktalarından en yakınını bulur
        Vector2 CalculateNearestChopPos(GameObject obj)
        {
            float distance1 = Vector2.Distance(obj.transform.GetChild(1).position, transform.position);
            float distance2 = Vector2.Distance(obj.transform.GetChild(2).position, transform.position);
            if (distance1 < distance2)
            {
                return obj.transform.GetChild(1).position;
            }

            else
                return obj.transform.GetChild(2).position;
        }
        private void OnDrawGizmos()
        {
            if (!isTree) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(targetResource.transform.position, currentChopTreeSightRange);
        }

    }
}
