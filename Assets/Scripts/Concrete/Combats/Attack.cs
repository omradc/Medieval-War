﻿using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using Assets.Scripts.Concrete.Orders;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Assets.Scripts.Concrete.Combats
{
    internal class Attack
    {

        float currentTime;
        UnitController uC;
        Order order;
        PathFinding2D pF2D;
        Direction direction;
        AnimationEventController animationEventController;
        bool workOnce;
        public Attack(UnitController uC, Order order, PathFinding2D pF2D, AnimationEventController animationEventController)
        {
            this.uC = uC;
            this.order = order;
            this.pF2D = pF2D;
            this.animationEventController = animationEventController;
            if (uC.unitTypeEnum == UnitTypeEnum.Worrior)
                animationEventController.AttackEvent += WorriorAttack;
            if (uC.unitTypeEnum == UnitTypeEnum.Archer)
                animationEventController.AttackEvent += ArcherAttack;
        }
        public void AttackOn()
        {
            if (order.DetechNearestTarget() == null)
            {
                //Düşman yoksa ve saldırı animasyonu oynarsa, idle oynar
                if (pF2D.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Front") || pF2D.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Up") || pF2D.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Down"))
                    AnimationManager.Instance.IdleAnim(pF2D.animator);
                return;
            }
            // Düşman saldırı menzilindeyse, yöne göre animasyonlar oynatılır. Animasyonlar saldırıları event ile tetikler
            if (Vector2.Distance(uC.attackRangePosition, order.DetechNearestTarget().transform.position) < uC.currentAttackRange)
            {
                if (pF2D.right || pF2D.left)
                    AnimationManager.Instance.AttackFrontAnim(pF2D.animator, uC.currentAttackSpeed);
                if (pF2D.up)
                    AnimationManager.Instance.AttackUpAnim(pF2D.animator, uC.currentAttackSpeed);
                if (pF2D.down)
                    AnimationManager.Instance.AttackDownAnim(pF2D.animator, uC.currentAttackSpeed);
                if (pF2D.upRight || pF2D.upLeft)
                    AnimationManager.Instance.AttackUpFrontAnim(pF2D.animator, uC.currentAttackSpeed);
                if (pF2D.downRight || pF2D.downLeft)
                    AnimationManager.Instance.AttackDownFrontAnim(pF2D.animator, uC.currentAttackSpeed);

            }


        }
        void WorriorAttack()
        {
            uC.hitTargets = Physics2D.OverlapCircleAll(uC.worriorAttackPoint.position, uC.currentAttackRadius, uC.targetLayer);
            for (int i = 0; i < uC.hitTargets.Length; i++)
            {
                if (uC.hitTargets != null)
                    uC.hitTargets[i].GetComponent<Health>().GetHit(uC.currentDamage);
            }
        }
        void ArcherAttack()
        {
            GameObject obj = Object.Instantiate(uC.arrow, uC.attackRangePosition, Quaternion.identity);
            Arrow arrow = obj.GetComponent<Arrow>();
            arrow.target = order.DetechNearestTarget();
            arrow.damage = uC.currentDamage;
            arrow.arrowSpeed = uC.currentArrowSpeed;
        }

    }
}