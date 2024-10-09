using Assets.Scripts.Concrete.Managers;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class Tree : MonoBehaviour
    {
        public GameObject resourceWood;
        public GameObject sleepingTorch;
        GameObject torch;
        public int hitPoint;
        public int currentHitPoint;
        public bool destruct;
        public float growTime;
        public float currentTime;
        public bool isTreeAlreadyCutted;
        bool destructOnce;
        Animator animator;
        Vector3 normal = new Vector3(1, 1, 1);
        Vector3 reverse = new Vector3(-1, 1, 1);
        private void Start()
        {
            currentHitPoint = hitPoint;
            animator = transform.GetChild(0).GetComponent<Animator>();
        }
        void Update()
        {
            GrowUp();
        }
        public void GetHit(int treeDamagePoint, float collectTime) // Köylü Chop animasyoununda, tam ağaca vurduğu anda event ile tetikler
        {
            currentHitPoint -= treeDamagePoint;
            if (currentHitPoint <= 0)
                Destruct(collectTime);
        }
        public void GetHitTreeAnim(float chopSpeed) // Köylü Chop animasyoununda, tam ağaca vurduğu anda event ile tetikler
        {
            AnimationManager.Instance.GetHitTreeAnim(animator, chopSpeed);
        }
        public void IsTreeAlreadyCutted(bool value)
        {
            isTreeAlreadyCutted = value;
            gameObject.layer = default;
        }

        void Destruct(float collectTime)
        {
            if (!destructOnce)
            {
                for (int i = 0; i < 3; i++)
                {
                    GameObject wood = Instantiate(resourceWood, transform.position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0), Quaternion.identity);
                    Destroy(wood, collectTime);
                }

                destruct = true;
                gameObject.layer = default;
                AnimationManager.Instance.DestroyedTreeAnim(animator);
                destructOnce = true;
            }
        }

        void GrowUp()
        {
            if (destruct)
            {
                currentTime += Time.deltaTime;
                if (currentTime > growTime)
                {
                    currentTime = 0;
                    destruct = false;
                    isTreeAlreadyCutted = false;
                    destructOnce = false;
                    AnimationManager.Instance.IdleTreeAnim(animator);
                    gameObject.layer = 15;
                    currentHitPoint = hitPoint;
                }
            }
        }

    }
}
