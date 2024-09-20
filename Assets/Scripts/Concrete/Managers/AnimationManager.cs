using Assets.Scripts.Abstracts.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Concrete.Managers
{
    internal class AnimationManager : MonoBehaviour, IAnimation
    {
        public static AnimationManager Instance { get; private set; }
        private void Awake()
        {
            Singelton();
        }
        void Singelton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
                Destroy(this);
        }

        public void IdleAnim(Animator animator)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                animator.speed = 1f;
                animator.Play("Idle");
            }

        }
        public void RunAnim(Animator animator, float speed)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                animator.speed = speed;
                animator.Play("Run");
            }

        }
        public void AttackUpAnim(Animator animator, float speed)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Up"))
            {
                animator.speed = speed;
                animator.Play("Attack_Up");
            }
        }
        public void AttackUpFrontAnim(Animator animator, float speed)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_UpFront"))
            {
                animator.speed = speed;
                animator.Play("Attack_UpFront");
            }
        }
        public void AttackFrontAnim(Animator animator, float speed)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Front"))
            {
                animator.speed = speed;
                animator.Play("Attack_Front");
            }
        }
        public void AttackDownFrontAnim(Animator animator, float speed)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_DownFront"))
            {
                animator.speed = speed;
                animator.Play("Attack_DownFront");
            }
        }
        public void AttackDownAnim(Animator animator, float speed)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Down"))
            {
                animator.speed = speed;
                animator.Play("Attack_Down");
            }
        }

        // Tree
        public void DestroyedTreeAnim(Animator animator)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Destroyed"))
            {
                animator.Play("Destroyed");
            }
        }

        public void IdleTreeAnim(Animator animator)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                animator.Play("Idle");
            }
        }

        public void GetHitTreeAnim(Animator animator, float speed)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit"))
            {
                animator.speed = speed;
                animator.Play("GetHit");
            }
        }

        //Villager
        public void RunCarryAnim(Animator animator, float speed)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run_0"))
            {
                animator.speed = speed;
                animator.Play("Run_0");
            }
        }
        public void ChopAnim(Animator animator, float speed)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Chop"))
            {
                animator.speed = speed;
                animator.Play("Chop");
            }
        }


    }
}
