using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectFood
    {
        VillagerController vC;
        UnitPathFinding2D pF2D;
        public CollectFood(VillagerController collectResources, UnitPathFinding2D pathFinding2D)
        {
            vC = collectResources;
            pF2D = pathFinding2D;
            vC.animationEventController.CutSheepEvent += CutSheep;
        }

        public void GoToSheep()
        {
            if (vC.isSheep && vC.targetResource != null)
            {
                // Hedef varsa ona git
                float distance = Vector2.Distance(vC.transform.position, vC.targetResource.transform.GetChild(1).position);
                if (vC.targetResource != null && !vC.returnFences && !vC.returnHome)
                {
                    // Koyuna ulaşınca dur
                    if (distance > .1f)
                    {
                        pF2D.AIGetMoveCommand(vC.targetResource.transform.GetChild(1).position);
                        AnimationManager.Instance.RunAnim(vC.animator, 1);
                    }
                    // Koyuna ulaşıldı
                    if (distance < .1f)
                    {
                        AnimationManager.Instance.IdleAnim(vC.animator);
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
                            AnimationManager.Instance.ChopAnim(vC.animator, vC.chopSpeed);
                            vC.targetResource = null;
                            vC.tCollect = 0;
                        }
                    }
                }

            }
        }

        void CutSheep()
        {
            if (vC.sheepController != null)
                vC.sheepController.CutSheep();
        }
        public void GoToFences()
        {
            if (vC.returnFences)
            {
                // Çit kapısına git
                if (Vector2.Distance(vC.transform.position, vC.fenceObj.transform.GetChild(0).position) > .1)
                {
                    pF2D.AIGetMoveCommand(vC.fenceObj.transform.GetChild(0).position);
                    AnimationManager.Instance.RunAnim(vC.animator, 1);
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
