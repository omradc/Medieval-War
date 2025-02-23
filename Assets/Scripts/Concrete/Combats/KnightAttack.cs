using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.AI;
using UnityEngine;
using Assets.Scripts.Concrete.Movements;

namespace Assets.Scripts.Concrete.Combats
{
    internal class KnightAttack
    {

        readonly KnightController kC;
        readonly KnightAI knightAI;
        readonly PathFindingController pF;
        public KnightAttack(KnightController kC, KnightAI knightAI, AnimationEventController animationEventController, PathFindingController pF)
        {
            this.kC = kC;
            this.knightAI = knightAI;
            this.pF = pF;

            if (kC.factionType == FactionTypeEnum.Warrior || kC.factionType == FactionTypeEnum.Villager)
                animationEventController.AttackEvent += WorriorOrVillagerAttack;
            if (kC.factionType == FactionTypeEnum.Archer)
                animationEventController.AttackEvent += ArcherAttack;
        }

        void WorriorOrVillagerAttack()
        {
            HealthController enemyHealth;
            enemyHealth = knightAI.target.GetComponent<HealthController>();
            enemyHealth.GetHit(kC.damage, kC.gameObject);
            if (enemyHealth.isDead)
                pF.agent.ResetPath();
        }

        void ArcherAttack()
        {
            GameObject obj = Object.Instantiate(kC.arrow, kC.attackRangePosition, Quaternion.identity);
            Arrow arrow = obj.GetComponent<Arrow>();
            arrow.target = knightAI.target;
            arrow.damage = kC.damage;
            arrow.archer = kC.gameObject;
            arrow.arrowSpeed = kC.arrowSpeed;
            arrow.arrowDestroyTime = kC.arrowDestroyTime;
        }

    }
}
