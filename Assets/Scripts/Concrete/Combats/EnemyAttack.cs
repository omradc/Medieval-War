﻿using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.AI;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Combats
{
    internal class EnemyAttack
    {
        GoblinController gC;
        EnemyAI enemyAI;
        PathFindingController pF;

        public EnemyAttack(GoblinController gC, EnemyAI enemyAI, AnimationEventController animationEventController, PathFindingController pF)
        {
            this.gC = gC;
            this.enemyAI = enemyAI;
            this.pF = pF;

            if (gC.troopType == TroopTypeEnum.Torch)
                animationEventController.AttackEvent += TorchAttack;
            if (gC.troopType == TroopTypeEnum.Tnt)
                animationEventController.AttackEvent += DynamiteAttack;
            if (gC.troopType == TroopTypeEnum.Barrel)
                animationEventController.AttackEvent += BarrelAttack;

        }



        // Saldırılar event ile tetiklenir
        void TorchAttack()
        {
            gC.hitTargets = Physics2D.OverlapCircleAll(gC.torchAttackPoint.position, gC.attackRange, gC.targetAll);
            for (int i = 0; i < gC.hitTargets.Length; i++)
            {
                if (gC.hitTargets != null)
                {
                    HealthController knightHealth = null;
                    knightHealth = gC.hitTargets[0].GetComponent<HealthController>();
                    knightHealth.GetHit(gC.damage);
                    if (knightHealth.isDead)
                        pF.agent.ResetPath();

                }
            }
        }
        void DynamiteAttack()
        {
            GameObject obj = Object.Instantiate(gC.dynamite, gC.attackRangePosition, Quaternion.identity);
            Dynamite dynamite = obj.GetComponent<Dynamite>();
            dynamite.targetLayer = gC.targetAll;
            dynamite.target = enemyAI.nearestTarget;
            dynamite.damage = gC.damage;
            dynamite.radius = gC.dynamiteExplosionRadius;
            dynamite.dynamiteSpeed = gC.dynamiteSpeed;
        }
        void BarrelAttack()
        {
            GameObject obj = Object.Instantiate(gC.explosion, gC.attackRangePosition, Quaternion.identity);
            Explosion explosion = obj.GetComponent<Explosion>();
            explosion.targetLayer = gC.targetAll;
            explosion.damage = gC.damage;
            explosion.radius = gC.barrelExplosionRadius;
            Object.Destroy(gC.gameObject);
        }
    }
}
