using Assets.Scripts.Abstracts.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Concrete.Managers
{
    internal class AnimationManager : MonoBehaviour
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

        //Factions
        public void IdleAnim(Animator animator)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                animator.speed = 1f;
                animator.Play("Idle");
            }
        }
        public void RunAnim(Animator animator, float speed = 1)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                animator.speed = speed;
                animator.Play("Run");
            }
        }
        public void AttackUpAnim(Animator animator, float speed = 1)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Up"))
            {
                animator.speed = speed;
                animator.Play("Attack_Up");
            }
        }
        public void AttackUpFrontAnim(Animator animator, float speed = 1)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_UpFront"))
            {
                animator.speed = speed;
                animator.Play("Attack_UpFront");
            }
        }
        public void AttackFrontAnim(Animator animator, float speed = 1)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Front"))
            {
                animator.speed = speed;
                animator.Play("Attack_Front");
            }
        }
        public void AttackDownFrontAnim(Animator animator, float speed = 1)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_DownFront"))
            {
                animator.speed = speed;
                animator.Play("Attack_DownFront");
            }
        }
        public void AttackDownAnim(Animator animator, float speed = 1)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Down"))
            {
                animator.speed = speed;
                animator.Play("Attack_Down");
            }
        }

        //Tree
        public void DestroyedTreeAnim(Animator animator)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Destroyed"))
            {
                animator.speed = 1f;
                animator.Play("Destroyed");
            }
        }
        public void IdleTreeAnim(Animator animator)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                animator.speed = 1f;
                animator.Play("Idle");
            }
        }
        public void GetHitTreeAnim(Animator animator, float speed = 1)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit"))
            {
                animator.speed = speed;
                animator.Play("GetHit");
            }
        }

        //Pawn
        public void RunCarryAnim(Animator animator, float speed = 1)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run_0"))
            {
                animator.speed = speed;
                animator.Play("Run_0");
            }
        }
        public void ChopTreeAnim(Animator animator, float speed = 1)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Chop_Wood"))
            {
                animator.speed = speed;
                animator.Play("Chop_Wood");
            }
        }
        public void ChopSheepAnim(Animator animator, float speed = 1)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Chop_Sheep"))
            {
                animator.speed = speed;
                animator.Play("Chop_Sheep");
            }
        }
        public void BuildAnim(Animator animator, float speed = 1)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Build"))
            {
                animator.speed = speed;
                animator.Play("Build");
            }
        }
        public void IdleCarryAnim(Animator animator)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_0"))
            {
                animator.speed = 1;
                animator.Play("Idle_0");
            }
        }

        // Sheep
        public void HappyAnim(Animator animator)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Happy"))
            {
                animator.speed = 1f;
                animator.Play("Happy");
            }
        }
    }
}
