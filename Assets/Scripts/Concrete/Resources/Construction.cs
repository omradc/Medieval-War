using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class Construction
    {
       readonly VillagerController vC;
        readonly PathFindingController pF;

        public Construction(VillagerController vC, PathFindingController pF)
        {
            this.vC = vC;
            this.pF = pF;
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
                    pF.MoveAI(constructionPos, 0);
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