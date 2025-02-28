using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;
namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectFood
    {
        readonly VillagerController vC;
        readonly PathFinding pF;
        public CollectFood(VillagerController vC, PathFinding pF)
        {
            this.vC = vC;
            this.pF = pF;
        }
        public void GoToSheep()
        {
            if (vC.isSheep && vC.targetResource != null && vC.fenceObj != null)
            {
                // Hedef varsa ona git
                float distance = Vector2.Distance(vC.transform.position, vC.targetResource.transform.GetChild(1).position);
                if (vC.targetResource != null && !vC.returnFences && !vC.returnHome)
                {
                    // Koyuna ulaşınca dur
                    if (distance > .1f)
                    {
                        pF.MoveAI(vC.targetResource.transform.GetChild(1).position, 0f);
                    }

                    //  // Koyun evcil değilse, durdur
                    if (distance <= 1 && !vC.sheepController.isDomestic)
                    {
                        vC.sheepController.pF.agent.ResetPath();
                    }

                    // Koyuna ulaşıldı
                    if (distance <= .1f)
                    {
                        // Koyunu evcilleştir
                        if (!vC.sheepController.isDomestic)
                        {
                            if (vC.fenceObj != null)
                                vC.sheepController.TameSheep(vC.gameObject, vC.fenceObj);
                        }
                        //Koyun evcilleştiyse, çite gönder
                        if (vC.sheepController.isDomestic && !vC.sheepController.inFence)
                        {
                            vC.returnFences = true;
                            vC.targetResource = null;
                            return;
                        }
                        //Koyun çitteyse, tekrar koyunu seçtiğinde kesebilir
                        if (vC.sheepController.inFence)
                        {
                            vC.workOnce = true;
                            vC.workOnce2 = true;
                            vC.returnHome = true;
                            //Koyunu kes, animasyon ile
                            AnimationManager.Instance.ChopSheepAnim(vC.animator, vC.chopSpeed);
                            // vC.targetResource = null; // kesme animasyonunun bitiminde null a döner. Aksi takdirde koyunu kesmeden önce yanlış yöne bakar
                            vC.tCollect = 0;

                        }
                    }
                }

            }
        }
        public void GetHitSheep() // Kesme animasyonu ile tetiklenir
        {
            if (vC.sheepController != null)
            {
                vC.sheepController.CutSheep();
            }
        }
        public void GoToFences()
        {
            if (vC.returnFences)
            {
                // Çit kapısına git
                if (Vector2.Distance(vC.transform.position, vC.fenceObj.transform.GetChild(0).position) > .1)
                {
                    pF.MoveAI(vC.fenceObj.transform.GetChild(0).position, 0);
                }

                // Çit kapısının önünde dur ve koyunu çite gönder
                if (Vector2.Distance(vC.transform.position, vC.fenceObj.transform.GetChild(0).position) < .1)
                {
                    vC.sheepController.CheckFences();
                    vC.sheepController.goFence = true;
                    vC.returnFences = false;
                    vC.isSheep = false;
                }
            }
        }
    }
}
