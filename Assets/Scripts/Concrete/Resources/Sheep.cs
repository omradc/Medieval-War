using Assets.Scripts.Concrete.Managers;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class Sheep : MonoBehaviour
    {
        public float speed = 2;
        public float tameTime;
        public float currentTameTime;
        public float meatTime;
        public float currentMeatTime;
        public bool isDomestic;
        public bool goFence;
        public bool giveMeat;
        public bool inFence;
        [SerializeField] GameObject resourceMeat;
        GameObject fence;
        Animator animator;
        GameObject villager;
        Vector3 rightDirection = new Vector3(1, 1, 1);
        Vector3 leftDirection = new Vector3(-1, 1, 1);
        Transform[] sheepPoints;
        Transform sheepPoint;
        private void Start()
        {
            animator = transform.GetChild(0).GetComponent<Animator>();
            InvokeRepeating(nameof(OptimumSetDirection), 0.1f, .5f);
        }

        // Köylü koyunu bulunca onu evcilleştirir
        public void TameSheep(GameObject villager, GameObject fence)
        {
            currentTameTime += Time.deltaTime;
            if (currentTameTime > tameTime)
            {
                AnimationManager.Instance.HappyAnim(animator);
                currentTameTime = 0;
                isDomestic = true;
                this.villager = villager;
                this.fence = fence;
                sheepPoints = fence.transform.GetChild(1).GetComponentsInChildren<Transform>();
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
        }
        private void Update()
        {
            FollowTheVillager();
            GoFence();
            ReadyToGiveMeat();
        }
        void FollowTheVillager()
        {
            // Eğer koyun; çitlere gitmiyorsa, evcilse ve köylü yanındaysa,
            if (!goFence && isDomestic && villager != null && !inFence)
            {
                // Köylüyü takip et
                if (Vector2.Distance(transform.position, villager.transform.position) > 0.3f)
                {
                    Vector3 dir = (villager.transform.position - transform.position).normalized;
                    transform.Translate(dir * speed * Time.deltaTime);
                }
            }

            if (!isDomestic || !giveMeat && inFence)
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
        void ReadyToGiveMeat()
        {
            if (inFence && !giveMeat)
            {
                currentMeatTime += Time.deltaTime;
                if (currentMeatTime > meatTime)
                {
                    giveMeat = true;
                    AnimationManager.Instance.HappyAnim(animator);
                    currentMeatTime = 0;
                }
            }
        }
        public void DropMeat(float lifeTime)
        {
            GameObject meat = Instantiate(resourceMeat, transform.position, Quaternion.identity);
            Destroy(meat, lifeTime);
        }

        void OptimumSetDirection()
        {
            if (isDomestic && villager != null)
            {
                if (transform.position.x > villager.transform.position.x)
                    transform.localScale = leftDirection;
                else
                    transform.localScale = rightDirection;

            }
        }
    }


}
