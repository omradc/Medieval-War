﻿using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.AI;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Combats
{
    internal class EnemyAttack
    {
       readonly GoblinController gC;
       readonly GoblinAI enemyAI;
       readonly PathFinding pF;

        public EnemyAttack(GoblinController gC, GoblinAI enemyAI, AnimationEventController animationEventController, PathFinding pF)
        {
            this.gC = gC;
            this.enemyAI = enemyAI;
            this.pF = pF;

            if (gC.factionType == FactionTypeEnum.Torch)
                animationEventController.AttackEvent += TorchAttack;
            if (gC.factionType == FactionTypeEnum.Tnt)
                animationEventController.AttackEvent += DynamiteAttack;
            if (gC.factionType == FactionTypeEnum.Barrel)
                animationEventController.AttackEvent += BarrelAttack;

        }



        // Saldırılar event ile tetiklenir
        void TorchAttack()
        {
            HealthController targetHealth;
            targetHealth =  enemyAI.target.GetComponent<HealthController>();
            targetHealth.GetHit(gC.damage,gC.gameObject);
            if (targetHealth.isDead)
            {
                gC.nonRangeDetechEnemy = null;
                pF.agent.ResetPath();
            }

        }
        void DynamiteAttack()
        {
            GameObject obj = Object.Instantiate(gC.dynamite, gC.transform.position, Quaternion.identity);
            Dynamite dynamite = obj.GetComponent<Dynamite>();
            dynamite.targetLayer = gC.enemiesLayer;
            dynamite.target = enemyAI.target;
            dynamite.dynamite = gC.gameObject; // dinamit atan asker
            dynamite.damage = gC.damage;
            dynamite.radius = gC.dynamiteExplosionRadius;
            dynamite.dynamiteSpeed = gC.dynamiteSpeed;
        }
        void BarrelAttack()
        {
            GameObject obj = Object.Instantiate(gC.explosion, gC.transform.position, Quaternion.identity);
            Explosion explosion = obj.GetComponent<Explosion>();
            explosion.targetLayer = gC.enemiesLayer;
            explosion.damage = gC.damage;
            explosion.radius = gC.barrelExplosionRadius;
            Object.Destroy(gC.gameObject);
        }
    }
}
