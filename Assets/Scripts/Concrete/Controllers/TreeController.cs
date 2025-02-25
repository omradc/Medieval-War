using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class TreeController : MonoBehaviour
    {
        public SpriteRenderer visual;
        public Transform orderInLayerSpriteAnchor;
        public GameObject resourceWood;
        public int health;
        public float growTime;
        bool destructOnce;
        float currentTime;
        int currentHealth;
        [HideInInspector] public bool destruct;
        [HideInInspector] public bool isTreeAlreadyCutted;
        Animator animator;
        DynamicOrderInLayer dynamicOrderInLayer;

        private void Awake()
        {
            animator = transform.GetChild(0).GetComponent<Animator>();
            dynamicOrderInLayer = new();
            
        }
        private void Start()
        {
            animator.enabled = false;
            currentHealth = health;
            dynamicOrderInLayer.OrderInLayerInitialize(orderInLayerSpriteAnchor, visual);
            InvokeRepeating(nameof(GrowUp), 0, 1);
            Invoke(nameof(RandomStartAnimation), Random.Range(0f, .5f));
        }

        void RandomStartAnimation()
        {
            animator.enabled = true;
        }
        public void GetHit(int treeDamagePoint, float collectTime) // Köylü Chop animasyoununda, tam ağaca vurduğu anda event ile tetikler
        {
            currentHealth -= treeDamagePoint;
            if (currentHealth <= 0)
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
                    Destroy(wood, collectTime - 0.5f); // 0.5 saniye erken yok olur
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
                currentTime += 1;
                if (currentTime > growTime)
                {
                    currentTime = 0;
                    destruct = false;
                    isTreeAlreadyCutted = false;
                    destructOnce = false;
                    AnimationManager.Instance.IdleTreeAnim(animator);
                    gameObject.layer = 15;
                    currentHealth = health;
                }
            }
        }

    }
}
