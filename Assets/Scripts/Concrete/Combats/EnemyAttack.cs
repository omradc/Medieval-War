using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.EnemyAIs;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Combats
{
    internal class EnemyAttack
    {
        float currentTime;
        EnemyController eC;
        EnemyAI enemyAI;
        EnemyPathFinding2D ePF2D;
        UnitDirection direction;
        AnimationEventController animationEventController;
        bool workOnce;
        public EnemyAttack(EnemyController eC, EnemyAI enemyAI, EnemyPathFinding2D ePF2D, AnimationEventController animationEventController)
        {
            this.eC = eC;
            this.enemyAI = enemyAI;
            this.ePF2D = ePF2D;
            this.animationEventController = animationEventController;


            if (eC.enemyTypeEnum == EnemyTypeEnum.Torch)
                animationEventController.AttackEvent += TorchAttack;
            if (eC.enemyTypeEnum == EnemyTypeEnum.Tnt)
                animationEventController.AttackEvent += DynamiteAttack;
            if (eC.enemyTypeEnum == EnemyTypeEnum.Barrel)
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
            if (Vector2.Distance(eC.attackRangePosition, enemyAI.nearestAttackPoint.position) < eC.currentAttackRange)
            {
                if (ePF2D.right || ePF2D.left)
                    AnimationManager.Instance.AttackFrontAnim(ePF2D.animator, eC.currentAttackSpeed);
                if (ePF2D.up)
                    AnimationManager.Instance.AttackUpAnim(ePF2D.animator, eC.currentAttackSpeed);
                if (ePF2D.down)
                    AnimationManager.Instance.AttackDownAnim(ePF2D.animator, eC.currentAttackSpeed);
            }


        }


        // Saldırılar event ile tetiklenir
        void TorchAttack()
        {
            eC.hitTargets = Physics2D.OverlapCircleAll(eC.torchAttackPoint.position, eC.currentAttackRadius, eC.targetAll);
            for (int i = 0; i < eC.hitTargets.Length; i++)
            {
                if (eC.hitTargets != null)
                    eC.hitTargets[0].GetComponent<HealthController>().GetHit(eC.currentDamage);
            }
        }
        void DynamiteAttack()
        {
            GameObject obj = Object.Instantiate(eC.dynamite, eC.attackRangePosition, Quaternion.identity);
            Dynamite dynamite = obj.GetComponent<Dynamite>();
            dynamite.targetLayer = eC.targetAll;
            dynamite.target = enemyAI.nearestTarget;
            dynamite.damage = eC.currentDamage;
            dynamite.radius = eC.currentDynamiteExplosionRadius;
            dynamite.dynamiteSpeed = eC.currentDynamiteSpeed;
        }
        void BarrelAttack()
        {
            GameObject obj = Object.Instantiate(eC.explosion, eC.attackRangePosition, Quaternion.identity);
            Explosion explosion = obj.GetComponent<Explosion>();
            explosion.targetLayer = eC.targetAll;
            explosion.damage = eC.currentDamage;
            explosion.radius = eC.currentBarrelExplosionRadius;
            Object.Destroy(eC.gameObject);
        }
    }
}
