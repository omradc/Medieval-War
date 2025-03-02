using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class Construction
    {
       readonly PawnController pawnController;
        readonly PathFinding pF;

        public Construction(PawnController pawnController, PathFinding pF)
        {
            this.pawnController = pawnController;
            this.pF = pF;
        }

        public void GoConstruct()
        {
            if (pawnController.isTree || pawnController.isMine || pawnController.isSheep) return;
            // İnşaat varsa ve boşsa ona git
            if (pawnController.constructionObj != null && !pawnController.constructController.isFull)
            {
                Vector2 constructionPos = pawnController.constructionObj.transform.GetChild(1).position;
                float distance = Vector2.Distance(pawnController.transform.position, constructionPos);
                if (distance > .1f)
                {
                    pF.MoveAI(constructionPos, 0);
                }

                if (distance <= .1f)
                {
                    AnimationManager.Instance.BuildAnim(pawnController.animator, pawnController.buildSpeed);
                }
            }
        }


        // Köylünün vuruş animasyonu ile tetiklenir
        public void Build()
        {
            pawnController.constructController.isFull = true;
            pawnController.constructController.currentHitNumber++;
            pawnController.constructController.UpdateConstructionTimerImage();
            // İnşaa Tamamlandı
            if (pawnController.constructController.currentHitNumber >= pawnController.constructController.hitNumber)
            {
                pawnController.constructionObj = null;
                AnimationManager.Instance.IdleAnim(pawnController.animator);
            }
        }

    }
}