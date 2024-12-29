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
        AnimationEventController animationEventController;

        public UnitAttack(KnightController kC, UnitAI unitAI, AnimationEventController animationEventController)
        {
            this.kC = kC;
            this.unitAI = unitAI;
            this.animationEventController = animationEventController;
            if (kC.unitTypeEnum == UnitTypeEnum.Worrior)
                animationEventController.AttackEvent += WorriorAttack;
            if (kC.unitTypeEnum == UnitTypeEnum.Archer)
                animationEventController.AttackEvent += ArcherAttack;
            if (kC.unitTypeEnum == UnitTypeEnum.Villager)
                animationEventController.AttackEvent += VillagerAttack;
        }
     
        void WorriorAttack()
        {
            kC.hitTargets = Physics2D.OverlapCircleAll(kC.transform.position, kC.currentAttackRange, kC.enemy);
            for (int i = 0; i < kC.hitTargets.Length; i++)
            {
                if (kC.hitTargets != null)
                    kC.hitTargets[0].GetComponent<HealthController>().GetHit(kC.currentDamage);
            }
        }
        void VillagerAttack()
        {
            kC.hitTargets = Physics2D.OverlapCircleAll(kC.transform.position, kC.currentAttackRange, kC.enemy);
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
            arrow.target = unitAI.nearestTarget;
            arrow.damage = kC.currentDamage;
            arrow.arrowSpeed = kC.currentArrowSpeed;
        }

    }
}
