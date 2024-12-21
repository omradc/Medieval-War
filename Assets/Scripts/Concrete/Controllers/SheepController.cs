using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class SheepController : MonoBehaviour
    {

        [Header("Movement")]
        [Range(0.1f, 2)] public float moveSpeed;
        public float currentSheepScale;
        public float followDistance;
        public GoblinBehaviorEnum goblinBehaviour;
        public float patrollingRadius;
        public float waitingTime;

        [Header("Feed")]
        public float maxSheepScale = 1.5f;
        public float growTime;
        public int maxMeatAmount;
        public float tameTime;

        [HideInInspector] public float currentGrowTime;
        [HideInInspector] public int currentMeatAmount;
        [HideInInspector] public float currentMoveSpeed;
        [HideInInspector] public float currentTameTime;
        [HideInInspector] public bool isDomestic;
        [HideInInspector] public bool goFence;
        [HideInInspector] public bool giveMeat;
        [HideInInspector] public bool inFence;
        [SerializeField] GameObject resourceMeat;
        GameObject fenceObj;
        Animator animator;
        GameObject villager;
        VillagerController vC;
        Transform[] sheepPoints;
        Transform sheepPoint;
        SheepPathFinding2D sPF2D;
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
            sPF2D = GetComponent<SheepPathFinding2D>();
        }
        private void Start()
        {
            animator = transform.GetChild(0).GetComponent<Animator>();
            InvokeRepeating(nameof(SetDirection), 0.1f, .5f);
            InvokeRepeating(nameof(CirclePatrollingAnchor), 0.1f, .5f);
            currentMeatAmount = ResourcesManager.Instance.collectMeatAmount;
            rightDirection = new Vector3(currentSheepScale, currentSheepScale, currentSheepScale);
            leftDirection = new Vector3(-currentSheepScale, currentSheepScale, currentSheepScale);
            scale = Vector3.one;
            firstPoint = transform.position;
            targetPoint = firstPoint;

        }

        private void Update()
        {
            currentMoveSpeed = moveSpeed * Time.deltaTime;
            FollowTheVillager();
            GoFence();
            GrowUp();
        }

        // Köylü koyunu bulunca onu evcilleştirir
        public void TameSheep(GameObject villager, GameObject fenceObj)
        {
            if (isDomestic) return;
            goblinBehaviour = GoblinBehaviorEnum.None;
            sPF2D.isPathEnd = true;

            currentTameTime += 1;
            if (currentTameTime > tameTime)
            {
                sPF2D.isPathEnd = false;
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

            // Eğer koyun; çitlere gitmiyorsa, evcilse ve köylü yanındaysa,
            if (!goFence && isDomestic && villager != null && !inFence)
            {
                // Köylüyü takip et
                if (Vector2.Distance(transform.position, villager.transform.position) > followDistance)
                {
                    AnimationManager.Instance.HappyAnim(animator);
                    sPF2D.AIGetMoveCommand(villager.transform.position);
                }
            }
        }
        void GoFence()
        {
            if (goFence)
            {
                // Çit dolu ise
                if (sheepPoint == null)
                {
                    goFence = false;
                    return;
                }
                // Çitlere git
                if (Vector2.Distance(transform.position, sheepPoint.position) > 0.1f)
                {
                    AnimationManager.Instance.HappyAnim(animator);
                    sPF2D.AIGetMoveCommand(sheepPoint.position);
                }

                // Çit içinde
                else
                {
                    goFence = false;
                    inFence = true;
                    AnimationManager.Instance.IdleAnim(animator);
                }
            }
        }
        //void ReadyToGiveMeat()
        //{
        //    if (inFence && !giveMeat)
        //    {
        //        AnimationManager.Instance.IdleAnim(animator);
        //        currentMeatTime += Time.deltaTime;
        //        if (currentMeatTime > meatTime)
        //        {
        //            giveMeat = true;
        //            AnimationManager.Instance.HappyAnim(animator);
        //            currentMeatTime = 0;
        //        }
        //    }
        //}

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
                    AnimationManager.Instance.HappyAnim(animator);
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

        void SetDirection()
        {

            if (inFence)
            {
                // Çitin içindeyse tüm koyunlar sola bakar
                if (!inFenceOnce)
                {
                    transform.localScale = leftDirection;
                    inFenceOnce = true;
                }
                return;
            }

            // Yola bak
            if (sPF2D.pathLeftToGo.Count > 0)
            {
                if (transform.position.x > sPF2D.pathLeftToGo[0].x)
                    transform.localScale = leftDirection;
                else
                    transform.localScale = rightDirection;

            }
        }

        void CirclePatrollingAnchor()
        {
            if (goblinBehaviour != GoblinBehaviorEnum.CirclePatrollingAnchor) return;
            if (patrol)
            {
                patrol = false;
                targetPoint = firstPoint;
                targetPoint += new Vector3(Random.Range(-patrollingRadius, patrollingRadius), Random.Range(-patrollingRadius, patrollingRadius));
                sPF2D.AIGetMoveCommand(targetPoint);
                AnimationManager.Instance.HappyAnim(animator);
            }

            if (sPF2D.pathLeftToGo.Count == 0)
            {
                time++;
                AnimationManager.Instance.IdleAnim(animator);
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
    }


}
