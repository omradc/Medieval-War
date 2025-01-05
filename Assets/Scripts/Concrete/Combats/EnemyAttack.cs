using Assets.Scripts.Concrete.Controllers;
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
        EnemyPathFinding2D ePF2D;
        AnimationEventController animationEventController;
        public EnemyAttack(GoblinController gC, EnemyAI enemyAI, EnemyPathFinding2D ePF2D, AnimationEventController animationEventController)
        {
            this.gC = gC;
            this.enemyAI = enemyAI;
            this.ePF2D = ePF2D;
            this.animationEventController = animationEventController;


            if (gC.enemyTypeEnum == EnemyTypeEnum.Torch)
                animationEventController.AttackEvent += TorchAttack;
            if (gC.enemyTypeEnum == EnemyTypeEnum.Tnt)
                animationEventController.AttackEvent += DynamiteAttack;
            if (gC.enemyTypeEnum == EnemyTypeEnum.Barrel)
                animationEventController.AttackEvent += BarrelAttack;

        }
        public void Attack()
        {
            if (enemyAI.nearestTarget == null)
            {
                //Oyuncu birimi yoksa ve saldırı animasyonu oynarsa, idle oynar
                if (ePF2D.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Front") || ePF2D.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Up") || ePF2D.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Down"))
                    AnimationManager.Instance.IdleAnim(ePF2D.animator);
                return;
            }
            // Düşman saldırı menzilindeyse, yöne göre animasyonlar oynatılır. Animasyonlar, saldırıları event ile tetikler
            if (Vector2.Distance(gC.attackRangePosition, enemyAI.nearestAttackPoint.position) < gC.currentAttackRange)
            {
                if (ePF2D.right || ePF2D.left)
                    AnimationManager.Instance.AttackFrontAnim(ePF2D.animator, gC.currentAttackSpeed);
                if (ePF2D.up)
                    AnimationManager.Instance.AttackUpAnim(ePF2D.animator, gC.currentAttackSpeed);
                if (ePF2D.down)
                    AnimationManager.Instance.AttackDownAnim(ePF2D.animator, gC.currentAttackSpeed);
            }


        }


        // Saldırılar event ile tetiklenir
        void TorchAttack()
        {
            gC.hitTargets = Physics2D.OverlapCircleAll(gC.torchAttackPoint.position, gC.currentAttackRadius, gC.targetAll);
            for (int i = 0; i < gC.hitTargets.Length; i++)
            {
                if (gC.hitTargets != null)
                    gC.hitTargets[0].GetComponent<HealthController>().GetHit(gC.currentDamage);
            }
        }
        void DynamiteAttack()
        {
            GameObject obj = Object.Instantiate(gC.dynamite, gC.attackRangePosition, Quaternion.identity);
            Dynamite dynamite = obj.GetComponent<Dynamite>();
            dynamite.targetLayer = gC.targetAll;
            dynamite.target = enemyAI.nearestTarget;
            dynamite.damage = gC.currentDamage;
            dynamite.radius = gC.currentDynamiteExplosionRadius;
            dynamite.dynamiteSpeed = gC.currentDynamiteSpeed;
        }
        void BarrelAttack()
        {
            GameObject obj = Object.Instantiate(gC.explosion, gC.attackRangePosition, Quaternion.identity);
            Explosion explosion = obj.GetComponent<Explosion>();
            explosion.targetLayer = gC.targetAll;
            explosion.damage = gC.currentDamage;
            explosion.radius = gC.currentBarrelExplosionRadius;
            Object.Destroy(gC.gameObject);
        }
    }
}
