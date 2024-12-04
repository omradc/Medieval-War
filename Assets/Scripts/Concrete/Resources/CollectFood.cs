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
        }

        public void GoToSheep()
        {
            if (!vC.isSheep || vC.isTree || vC.isMine) return;
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
                    if (!vC.sheep.isDomestic)
                    {
                        if (vC.fenceObj != null)
                            vC.sheep.TameSheep(vC.gameObject, vC.fenceObj);
                    }
                    //Koyun evcilleştiyse, çite gönder
                    if (vC.sheep.isDomestic && !vC.sheep.inFence)
                        vC.returnFences = true;
                    //Koyun çitteyse ve kaynağı dolu ise, eve dönüşü bekle
                    if (vC.sheep.inFence && vC.sheep.giveMeat)
                    {
                        vC.workOnce = true;
                        vC.workOnce2 = true;
                        vC.returnHome = true;
                        vC.sheep.giveMeat = false;
                        vC.sheep.DropMeat(vC.meatCollectTime);
                        vC.tCollect = 0;
                    }
                }
            }
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
                    vC.sheep.CheckFences();
                    vC.sheep.goFence = true;
                    vC.returnFences = false;
                    vC.isSheep = false;
                }
            }
        }
    }
}
