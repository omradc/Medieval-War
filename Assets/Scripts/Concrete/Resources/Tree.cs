using Assets.Scripts.Concrete.Managers;
using System;
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
        private void Start()
        {
            currentHitPoint = hitPoint;
            animator = transform.GetChild(0).GetComponent<Animator>();
        }

        void Update()
        {
            GrowUp();
        }
        public void GetHit(int treeDamagePoint)
        {
            currentHitPoint -= treeDamagePoint;
            if (currentHitPoint <= 0)
                Destruct();
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
                }
            }
        }

    }
}
