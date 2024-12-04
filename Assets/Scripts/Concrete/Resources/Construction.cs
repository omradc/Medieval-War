using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class Construction
    {
        VillagerController vC;
        UnitPathFinding2D pF2D;

        public Construction(VillagerController vC, UnitPathFinding2D pF2D)
        {
            this.vC = vC;
            this.pF2D = pF2D;
        }

        public void GoConstruct()
        {
            if (vC.isTree || vC.isMine || vC.isSheep) return;
            // İnşaat varsa ve boşsa ona git
            if (vC.constructionObj != null && !vC.constructController.isFull)
            {
                Vector2 constructionPos = vC.constructionObj.transform.GetChild(1).position;
                float distance = Vector2.Distance(vC.transform.position, constructionPos);
                if (distance > .1f)
                {
                    pF2D.AIGetMoveCommand(constructionPos);
                    AnimationManager.Instance.RunAnim(vC.animator, 1);
                }

                if (distance <= .1f)
                {
                    AnimationManager.Instance.BuildAnim(vC.animator, vC.buildSpeed);
                }
            }
        }


        // Köylünün vuruş animasyonu ile tetiklenir
        public void Build()
        {
            vC.constructController.isFull = true;
            vC.constructController.currentHitNumber++;
            vC.constructController.UpdateConstructionTimerImage();
            // İnşaa Tamamlandı
            if (vC.constructController.currentHitNumber >= vC.constructController.hitNumber)
            {
                vC.constructionObj = null;
                AnimationManager.Instance.IdleAnim(vC.animator);
            }
        }

    }
}