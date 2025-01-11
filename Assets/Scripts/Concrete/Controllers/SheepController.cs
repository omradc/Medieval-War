using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
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
        public float currentSheepScale;
        public float followDistance;
        public BehaviorEnum behavior;
        public float patrollingRadius;
        public float waitingTime;


        public float currentGrowTime;
        public int currentMeatAmount;
        public float currentTameTime;
        public bool isDomestic;
        public bool goFence;
        public bool inFence;
        public bool growed;
        [SerializeField] GameObject resourceMeat;
        GameObject fenceObj;
        Animator animator;
        GameObject villager;
        VillagerController vC;
        public Transform[] sheepPoints;
        public Transform sheepPoint;
        PathFindingController pF;
        Vector3 rightDirection;
        Vector3 leftDirection;
        Vector3 scale;
        Vector3 firstPoint;
        Vector3 targetPoint;
        bool inFenceOnce;
        bool patrol;
        float time;

        private void Awake()
        {
            pF = GetComponent<PathFindingController>();
        }
        private void Start()
        {
            pF.agent.speed = moveSpeed;
            animator = transform.GetChild(0).GetComponent<Animator>();
            currentMeatAmount = ResourcesManager.Instance.collectMeatAmount;
            rightDirection = new Vector3(currentSheepScale, currentSheepScale, currentSheepScale);
            leftDirection = new Vector3(-currentSheepScale, currentSheepScale, currentSheepScale);
            scale = Vector3.one;
            firstPoint = transform.position;
            targetPoint = firstPoint;
            InvokeRepeating(nameof(OptimumAI), .1f, sheepAIPerTime);
        }

        private void Update()
        {
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
                vC = villager.GetComponent<VillagerController>();
                this.fenceObj = fenceObj;
                sheepPoints = fenceObj.transform.GetChild(1).GetComponentsInChildren<Transform>();
            }
        }
        void FollowTheVillager()
        {
            if (inFence || goFence) return;
            if (villager != null)
                if (vC.kC.isSeleceted)
                    isDomestic = false;

            // Eğer koyun; çitlere gitmiyorsa, evcilse ve köylüsü varsa, onu takip eder
            if (!goFence && isDomestic && villager != null && !inFence)
            {
                // Köylüyü takip et
                if (Vector2.Distance(transform.position, villager.transform.position) > followDistance)
                {
                    pF.agent.stoppingDistance = followDistance;
                    pF.MoveAI(villager.transform.position);
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
                    pF.MoveAI(sheepPoint.position);
                }
                // Çit içinde
                if (Vector2.Distance(transform.position, sheepPoint.position) < 0.1f && pF.isStopped)
                {
                    goFence = false;
                    inFence = true;
                    transform.localScale = leftDirection; // Koyun çitin içindeyse sola bakar
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
            GameObject meat = Instantiate(resourceMeat, transform.position, Quaternion.identity);
            Destroy(meat, vC.meatCollectTime - 1.5f); // Yere düşen et 1 saniye erken yok olur
            gameObject.SetActive(false);
            Destroy(gameObject, 1); // Koyunu hemen yok edersen, et toplamaz
        }
        public void CheckFences()
        {
            for (int i = 1; i < sheepPoints.Length; i++)
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
                pF.agent.stoppingDistance = followDistance;
                pF.MoveAI(targetPoint);
            }

            if (pF.agent.isStopped)
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
            if (pF.isStopped) // Koyun durur
                AnimationManager.Instance.IdleAnim(animator);
            else // Koyun hareket eder
                AnimationManager.Instance.HappyAnim(animator);

        }
    }


}
