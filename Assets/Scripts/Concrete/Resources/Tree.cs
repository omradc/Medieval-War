using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class Tree : MonoBehaviour
    {
        public int hitPoint;
        public int currentHitPoint;
        public bool destruct;
        public float growTime;
        public float currentTime;
        Animator animator;
        Vector3 normal = new Vector3(1, 1, 1);
        Vector3 reverse = new Vector3(-1, 1, 1);
        private void Start()
        {
            currentHitPoint = hitPoint;
            animator = transform.GetChild(0).GetComponent<Animator>();
        }

        private void GetHitAnim()
        {
            print("GetHitAnim");
            AnimationManager.Instance.GetHitTreeAnim(animator, 1);
        }

        void Update()
        {
            GrowUp();

        }
        public void GetHit(int treeDamagePoint/*, int direction*/)
        {
            //if (direction == 1)
            //    transform.localScale = normal;
            //else
            //    transform.localScale = reverse;
            //GetHitAnim();
            currentHitPoint -= treeDamagePoint;
            if (currentHitPoint <= 0)
                Destruct();
        }
        public void GetHitTreeAnim( int direction)
        {
            if (direction == 1)
                transform.localScale = normal;
            else
                transform.localScale = reverse;
            GetHitAnim();

        }
        void Destruct()
        {
            destruct = true;
            AnimationManager.Instance.DestroyedTreeAnim(animator);
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
                    AnimationManager.Instance.IdleTreeAnim(animator);
                    gameObject.layer = 15;
                    currentHitPoint = hitPoint;
                }
            }
        }

    }
}
