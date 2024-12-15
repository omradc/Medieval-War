using Assets.Scripts.Concrete.Managers;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class SheepController : MonoBehaviour
    {
        public float speed;
        public float currentSheepScale;
        [Range(1, 2)] public float maxSheepScale;
        public float growTime;
        public float currentGrowTime;
        public int currentMeatAmount;
        public int maxMeatAmount;
        public float tameTime;
        public float followDistance;

        [HideInInspector] public float currentTameTime;
        [HideInInspector] public bool isDomestic;
        [HideInInspector] public bool goFence;
        [HideInInspector] public bool giveMeat;
        public bool inFence;
        [SerializeField] GameObject resourceMeat;
        GameObject fenceObj;
        Animator animator;
        GameObject villager;
        VillagerController vC;
        Vector3 rightDirection;
        Vector3 leftDirection;
        Transform[] sheepPoints;
        Transform sheepPoint;
        Vector3 scale;
        bool inFenceOnce;

        private void Start()
        {
            animator = transform.GetChild(0).GetComponent<Animator>();
            InvokeRepeating(nameof(OptimumSetDirection), 0.1f, .5f);
            currentMeatAmount = ResourcesManager.Instance.collectMeatAmount;
            rightDirection = new Vector3(currentSheepScale, currentSheepScale, currentSheepScale);
            leftDirection = new Vector3(-currentSheepScale, currentSheepScale, currentSheepScale);
            scale = Vector3.one;
        }

        private void Update()
        {
            FollowTheVillager();
            GoFence();
            GrowUp();
        }

        // Köylü koyunu bulunca onu evcilleştirir
        public void TameSheep(GameObject villager, GameObject fenceObj)
        {
            if (isDomestic) return;

            currentTameTime += 1;
            if (currentTameTime > tameTime)
            {
                AnimationManager.Instance.HappyAnim(animator);
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
                    Vector3 dir = (villager.transform.position - transform.position).normalized;
                    transform.Translate(dir * speed * Time.deltaTime);
                }
            }

            if (!isDomestic)
                AnimationManager.Instance.IdleAnim(animator);
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
                    Vector3 dir = (sheepPoint.position - transform.position).normalized;
                    transform.Translate(dir * speed * Time.deltaTime);
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
        void OptimumSetDirection()
        {
            if (isDomestic && villager != null)
            {
                if (inFence)
                {
                    if (!inFenceOnce)
                    {
                        transform.localScale = leftDirection;
                        inFenceOnce = true;
                    }
                    return;
                }
                if (transform.position.x > villager.transform.position.x)
                    transform.localScale = leftDirection;
                else
                    transform.localScale = rightDirection;

            }
        }
    }


}
