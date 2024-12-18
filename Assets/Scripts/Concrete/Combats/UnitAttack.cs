using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using Assets.Scripts.Concrete.Orders;
using UnityEngine;

namespace Assets.Scripts.Concrete.Combats
{
    internal class UnitAttack
    {

        KnightController kC;
        UnitAI unitAI;
        UnitPathFinding2D pF2D;
        AnimationEventController animationEventController;

        public UnitAttack(KnightController kC, UnitAI unitAI, UnitPathFinding2D pF2D, AnimationEventController animationEventController)
        {
            this.kC = kC;
            this.unitAI = unitAI;
            this.pF2D = pF2D;
            this.animationEventController = animationEventController;
            if (kC.unitTypeEnum == UnitTypeEnum.Worrior)
                animationEventController.AttackEvent += WorriorAttack;
            if (kC.unitTypeEnum == UnitTypeEnum.Archer)
                animationEventController.AttackEvent += ArcherAttack;
            if (kC.unitTypeEnum == UnitTypeEnum.Villager)
                animationEventController.AttackEvent += VillagerAttack;
        }
        public void AttackOn()
        {
            if (unitAI.DetechNearestTarget() == null)
            {
                //Düşman yoksa ve saldırı animasyonu oynarsa, idle oynar
                if (pF2D.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Front") || pF2D.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Up") || pF2D.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Down")
                    || pF2D.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_UpFront") || pF2D.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_DownFront"))
                    AnimationManager.Instance.IdleAnim(pF2D.animator);
                return;
            }
            // Düşman saldırı menzilindeyse, yöne göre animasyonlar oynatılır. Animasyonlar, saldırıları event ile tetikler
            if (Vector2.Distance(kC.attackRangePosition, unitAI.DetechNearestTarget().transform.position) < kC.currentAttackRange)
            {
                if (pF2D.right || pF2D.left)
                    AnimationManager.Instance.AttackFrontAnim(pF2D.animator, kC.currentAttackSpeed);
                if (pF2D.up)
                    AnimationManager.Instance.AttackUpAnim(pF2D.animator, kC.currentAttackSpeed);
                if (pF2D.down)
                    AnimationManager.Instance.AttackDownAnim(pF2D.animator, kC.currentAttackSpeed);
                if (pF2D.upRight || pF2D.upLeft)
                    AnimationManager.Instance.AttackUpFrontAnim(pF2D.animator, kC.currentAttackSpeed);
                if (pF2D.downRight || pF2D.downLeft)
                    AnimationManager.Instance.AttackDownFrontAnim(pF2D.animator, kC.currentAttackSpeed);

            }


        }
        void WorriorAttack()
        {
            kC.hitTargets = Physics2D.OverlapCircleAll(kC.attackPoint.position, kC.currentAttackRadius, kC.enemy);
            for (int i = 0; i < kC.hitTargets.Length; i++)
            {
                if (kC.hitTargets != null)
                    kC.hitTargets[0].GetComponent<HealthController>().GetHit(kC.currentDamage);
            }
        }
        void VillagerAttack()
        {
            kC.hitTargets = Physics2D.OverlapCircleAll(kC.attackPoint.position, kC.currentAttackRadius, kC.enemy);
            for (int i = 0; i < kC.hitTargets.Length; i++)
            {
                if (kC.hitTargets != null)
                    kC.hitTargets[i].GetComponent<HealthController>().GetHit(kC.currentDamage);
            }
        }
        void ArcherAttack()
        {
            GameObject obj = Object.Instantiate(kC.arrow, kC.attackRangePosition, Quaternion.identity);
            Arrow arrow = obj.GetComponent<Arrow>();
            arrow.target = unitAI.DetechNearestTarget();
            arrow.damage = kC.currentDamage;
            arrow.arrowSpeed = kC.currentArrowSpeed;
        }

    }
}
