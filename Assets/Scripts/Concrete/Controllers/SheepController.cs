using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    [RequireComponent(typeof(PathFinding))]
    internal class SheepController : MonoBehaviour
    {

        [Header("Sheep")]
        [Range(0.1f, 1)] public float sheepAIPerTime = .5f;
        public float maxSheepScale = 1.5f;
        public float growTime;
        public int maxMeatAmount;
        public float tameTime;

        [Header("Movement")]
        [Range(0.1f, 2)] public float moveSpeed;
        [Range(0.1f, 2)] public float followSpeed;
        public float currentSheepScale;
        public float followDistance;
        public BehaviorEnum behavior;
        public float patrollingRadius;
        public float waitingTime;

        [Header("Setup")]
        [SerializeField] GameObject resourceMeat;
        [SerializeField] Transform orderInLayerSpriteAnchor;
        [SerializeField] SpriteRenderer visual;
        public Transform[] sheepPoints;
        public Transform sheepPoint;

        [HideInInspector] public float currentGrowTime;
        [HideInInspector] public int currentMeatAmount;
        [HideInInspector] public float currentTameTime;
        [HideInInspector] public bool isDomestic;
        [HideInInspector] public bool goFence;
        [HideInInspector] public bool inFence;
        [HideInInspector] public bool growed;
        Animator animator;
        GameObject villager;
        PawnController pawnController;
        [HideInInspector] public PathFinding pF;
        DynamicOrderInLayer dynamicOrderInLayer;
        Vector3 scale;
        Vector3 firstPoint;
        Vector3 targetPoint;
        public bool patrol;
        float time;
        int singleOrDouble;
        private void Awake()
        {
            pF = GetComponent<PathFinding>();
            dynamicOrderInLayer = new();
        }
        private void Start()
        {
            pF.agent.speed = moveSpeed;
            animator = transform.GetChild(0).GetComponent<Animator>();
            currentMeatAmount = ResourcesManager.Instance.collectMeatAmount;
            scale = Vector3.one;
            firstPoint = transform.position;
            targetPoint = firstPoint;
            InvokeRepeating(nameof(OptimumAI), .1f, sheepAIPerTime);
        }

        private void Update()
        {
            dynamicOrderInLayer.OrderInLayerInitialize(orderInLayerSpriteAnchor, visual);
            AnimationControl();
            GrowUp();
        }

        void OptimumAI()
        {
            CirclePatrollingAnchor();
            FollowTheVillager();
            GoFence();
        }

        // Köylü koyunu bulunca onu evcilleştirir
        public void TameSheep(GameObject villager, GameObject fenceObj)
        {
            if (isDomestic) return;
            behavior = BehaviorEnum.Default;
            currentTameTime += 1;
            if (currentTameTime > tameTime)
            {
                currentTameTime = 0;
                isDomestic = true;
                this.villager = villager;
                pawnController = villager.GetComponent<PawnController>();
                sheepPoints = new Transform[fenceObj.transform.GetChild(1).childCount];
                for (int i = 0; i < sheepPoints.Length; i++)
                {
                    sheepPoints[i] = fenceObj.transform.GetChild(1).GetChild(i);
                }
            }
        }
        void FollowTheVillager()
        {
            if (inFence || goFence) return;
            if (villager != null)
            {
                pF.agent.speed = followSpeed;
                if (pawnController.kC.isSeleceted)
                {
                    pF.agent.speed = moveSpeed;
                    isDomestic = false;
                }
            }

            // Eğer koyun; çitlere gitmiyorsa, evcilse ve köylüsü varsa, onu takip eder
            if (!goFence && isDomestic && villager != null && !inFence)
            {
                // Köylüyü takip et
                if (Vector2.Distance(transform.position, villager.transform.position) > followDistance)
                {
                    pF.agent.stoppingDistance = followDistance;
                    pF.MoveAI(villager.transform.position, followDistance);
                }
            }
        }
        void GoFence()
        {
            if (goFence)
            {
                // Çit dolu ise, gitme
                if (sheepPoint == null)
                {
                    goFence = false;
                    return;
                }
                // Çitlere git
                if (Vector2.Distance(transform.position, sheepPoint.position) >= 0.1f)
                {
                    pF.agent.stoppingDistance = 0;
                    pF.MoveAI(sheepPoint.position, 0);
                }
                // Çit içinde
                if (Vector2.Distance(transform.position, sheepPoint.position) < 0.1f && pF.isStopping)
                {
                    goFence = false;
                    inFence = true;
                    singleOrDouble = System.Convert.ToInt32(sheepPoint.name);
                }
            }
        }
        void GrowUp()
        {
            if (inFence)
            {
                // Büyüme süresi dolarsa büyümez
                if (currentGrowTime <= growTime)
                {
                    AnimationManager.Instance.IdleAnim(animator);
                    currentGrowTime += Time.deltaTime;

                    // Increase Scale
                    float scaleFactor = currentGrowTime / growTime * (maxSheepScale - currentSheepScale) + currentSheepScale;
                    if (singleOrDouble == 0) // Çift ise sola bakar
                        scale.x = scaleFactor;
                    else // Tek ise sağa bakar
                        scale.x = -scaleFactor;
                    scale.y = scaleFactor;
                    scale.z = scaleFactor;
                    transform.localScale = scale;

                    // Increase Meat
                    currentMeatAmount = Mathf.FloorToInt(currentGrowTime / growTime *
                        (maxMeatAmount - ResourcesManager.Instance.collectMeatAmount) + ResourcesManager.Instance.collectMeatAmount);


                }

                else
                {
                    growed = true;
                }
            }
        }
        public void CutSheep()
        {
            sheepPoint.gameObject.SetActive(true); // Koyun kesilirse, yerine yeni koyun gelebilmesi için
            GameObject meat = Instantiate(resourceMeat, transform.position, Quaternion.identity);
            Destroy(meat, pawnController.meatCollectTime - 1.5f); // Yere düşen et 1 saniye erken yok olur
            gameObject.SetActive(false);
            Destroy(gameObject, 1); // Koyunu hemen yok edersen, et toplamaz
        }
        public void CheckFences()
        {
            for (int i = 0; i < sheepPoints.Length; i++)
            {
                if (sheepPoints[i].transform.gameObject.activeSelf)
                {
                    sheepPoint = sheepPoints[i];
                    sheepPoints[i].gameObject.SetActive(false);
                    break;
                }
            }
        }

        void CirclePatrollingAnchor()
        {
            if (behavior != BehaviorEnum.CirclePatrollingAnchor) return;
            if (patrol)
            {
                patrol = false;
                targetPoint = firstPoint;
                targetPoint += new Vector3(Random.Range(-patrollingRadius, patrollingRadius), Random.Range(-patrollingRadius, patrollingRadius));
                pF.agent.stoppingDistance = 0;
                pF.MoveAI(targetPoint, 0);
            }

            if (pF.isStopping)
            {
                time++;
                if (time >= waitingTime)
                {
                    time = 0;
                    patrol = true;
                }

            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(firstPoint, patrollingRadius);

        }

        void AnimationControl()
        {
            if (growed) // Koyun büyüdüyse zıplar
            {
                AnimationManager.Instance.HappyAnim(animator);
                return;
            }
            if (pF.isStopping) // Koyun durur
                AnimationManager.Instance.IdleAnim(animator);
            else // Koyun hareket eder
                AnimationManager.Instance.HappyAnim(animator);

        }
    }


}
