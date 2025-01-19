using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.AI;
using UnityEngine;
using Assets.Scripts.Concrete.Movements;

namespace Assets.Scripts.Concrete.Combats
{
    internal class UnitAttack
    {

        readonly KnightController kC;
        readonly KnightAI knightAI;
        readonly PathFindingController pF;

        public UnitAttack(KnightController kC, KnightAI knightAI, AnimationEventController animationEventController, PathFindingController pF)
        {
            this.kC = kC;
            this.knightAI = knightAI;
            this.pF = pF;

            if (kC.troopType == TroopTypeEnum.Worrior)
                animationEventController.AttackEvent += WorriorAttack;
            if (kC.troopType == TroopTypeEnum.Archer)
                animationEventController.AttackEvent += ArcherAttack;
            if (kC.troopType == TroopTypeEnum.Villager)
                animationEventController.AttackEvent += VillagerAttack;
        }

        void WorriorAttack()
        {
            kC.hitTargets = Physics2D.OverlapCircleAll(kC.transform.position, kC.attackRange, kC.enemy);
            for (int i = 0; i < kC.hitTargets.Length; i++)
            {
                if (kC.hitTargets != null)
                {
                    HealthController enemyHealth;
                    enemyHealth = kC.hitTargets[0].GetComponent<HealthController>();
                    enemyHealth.GetHit(kC.damage);
                    if (enemyHealth.isDead)
                        pF.agent.ResetPath();


                }
            }
        }
        void VillagerAttack()
        {
            kC.hitTargets = Physics2D.OverlapCircleAll(kC.transform.position, kC.attackRange, kC.enemy);
            for (int i = 0; i < kC.hitTargets.Length; i++)
            {
                //if (kC.hitTargets != null)
                kC.hitTargets?[i].GetComponent<HealthController>().GetHit(kC.damage);
            }
        }
        void ArcherAttack()
        {
            GameObject obj = Object.Instantiate(kC.arrow, kC.attackRangePosition, Quaternion.identity);
            Arrow arrow = obj.GetComponent<Arrow>();
            arrow.target = knightAI.nearestTarget;
            arrow.damage = kC.damage;
            arrow.arrowSpeed = kC.arrowSpeed;
        }

    }
}
