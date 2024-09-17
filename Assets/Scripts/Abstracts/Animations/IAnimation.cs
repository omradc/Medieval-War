using UnityEngine;

namespace Assets.Scripts.Abstracts.Animations
{
    internal interface IAnimation
    {
        void IdleAnim(Animator animator);
        void RunAnim(Animator animator, float speed);
        void AttackFrontAnim(Animator animator, float speed);
        void AttackUpAnim(Animator animator, float speed);
        void AttackDownAnim(Animator animator, float speed);
        void AttackUpFrontAnim(Animator animator, float speed);
        void AttackDownFrontAnim(Animator animator, float speed);

    }
}
