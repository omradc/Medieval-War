using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;
namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectFood
    {
        readonly PawnController pawnController;
        readonly PathFinding pF;
        public CollectFood(PawnController pawnController, PathFinding pF)
        {
            this.pawnController = pawnController;
            this.pF = pF;
        }
        public void GoToSheep()
        {
            if (pawnController.isSheep && pawnController.targetResource != null && pawnController.fenceObj != null)
            {
                // Hedef varsa ona git
                float distance = Vector2.Distance(pawnController.transform.position, pawnController.targetResource.transform.GetChild(1).position);
                if (pawnController.targetResource != null && !pawnController.returnFences && !pawnController.returnHome)
                {
                    // Koyuna ulaşınca dur
                    if (distance > .1f)
                    {
                        pF.MoveAI(pawnController.targetResource.transform.GetChild(1).position, 0f);
                    }

                    //  // Koyun evcil değilse, durdur
                    if (distance <= 1 && !pawnController.sheepController.isDomestic)
                    {
                        pawnController.sheepController.pF.agent.ResetPath();
                    }

                    // Koyuna ulaşıldı
                    if (distance <= .1f)
                    {
                        // Koyunu evcilleştir
                        if (!pawnController.sheepController.isDomestic)
                        {
                            if (pawnController.fenceObj != null)
                                pawnController.sheepController.TameSheep(pawnController.gameObject, pawnController.fenceObj);
                        }
                        //Koyun evcilleştiyse, çite gönder
                        if (pawnController.sheepController.isDomestic && !pawnController.sheepController.inFence)
                        {
                            pawnController.returnFences = true;
                            pawnController.targetResource = null;
                            return;
                        }
                        //Koyun çitteyse, tekrar koyunu seçtiğinde kesebilir
                        if (pawnController.sheepController.inFence)
                        {
                            pawnController.workOnce = true;
                            pawnController.workOnce2 = true;
                            pawnController.returnHome = true;
                            //Koyunu kes, animasyon ile
                            AnimationManager.Instance.ChopSheepAnim(pawnController.animator, pawnController.chopSpeed);
                            // vC.targetResource = null; // kesme animasyonunun bitiminde null a döner. Aksi takdirde koyunu kesmeden önce yanlış yöne bakar
                            pawnController.tCollect = 0;

                        }
                    }
                }

            }
        }
        public void GetHitSheep() // Kesme animasyonu ile tetiklenir
        {
            if (pawnController.sheepController != null)
            {
                pawnController.sheepController.CutSheep();
            }
        }
        public void GoToFences()
        {
            if (pawnController.returnFences)
            {
                // Çit kapısına git
                if (Vector2.Distance(pawnController.transform.position, pawnController.fenceObj.transform.GetChild(0).position) > .1)
                {
                    pF.MoveAI(pawnController.fenceObj.transform.GetChild(0).position, 0);
                }

                // Çit kapısının önünde dur ve koyunu çite gönder
                if (Vector2.Distance(pawnController.transform.position, pawnController.fenceObj.transform.GetChild(0).position) < .1)
                {
                    pawnController.sheepController.CheckFences();
                    pawnController.sheepController.goFence = true;
                    pawnController.returnFences = false;
                    pawnController.isSheep = false;
                }
            }
        }
    }
}
