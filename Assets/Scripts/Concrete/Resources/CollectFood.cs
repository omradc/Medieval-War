using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectFood
    {
        CollectResourcesController cR;
        UnitPathFinding2D pF2D;
        public CollectFood(CollectResourcesController collectResources, UnitPathFinding2D pathFinding2D)
        {
            cR = collectResources;
            pF2D = pathFinding2D;
        }

        public void GoToSheep()
        {
            if (!cR.isSheep || cR.isTree || cR.isMine) return;
            // Hedef varsa ona git
            float distance = Vector2.Distance(cR.transform.position, cR.targetResource.transform.GetChild(1).position);
            if (cR.targetResource != null && !cR.returnFences && !cR.returnHome)
            {
                // Koyuna ulaşınca dur
                if (distance > .1f)
                {
                    pF2D.AIGetMoveCommand(cR.targetResource.transform.GetChild(1).position);
                    AnimationManager.Instance.RunAnim(cR.animator, 1);
                }
                // Koyuna ulaşıldı
                if (distance < .1f)
                {
                    AnimationManager.Instance.IdleAnim(cR.animator);
                    // Koyunu evcilleştir
                    if (!cR.sheep.isDomestic)
                    {
                        if (cR.fenceObj != null)
                            cR.sheep.TameSheep(cR.gameObject, cR.fenceObj);
                    }
                    //Koyun evcilleştiyse, çite gönder
                    if (cR.sheep.isDomestic && !cR.sheep.inFence)
                        cR.returnFences = true;
                    //Koyun çitteyse ve kaynağı dolu ise, eve dönüşü bekle
                    if (cR.sheep.inFence && cR.sheep.giveMeat)
                    {
                        cR.workOnce = true;
                        cR.workOnce2 = true;
                        cR.returnHome = true;
                        cR.sheep.giveMeat = false;
                        cR.sheep.DropMeat(cR.meatCollectTime);
                        cR.tCollect = 0;
                    }
                }
            }
        }
        public void GoToFences()
        {


            if (cR.returnFences)
            {
                // Çit kapısına git
                if (Vector2.Distance(cR.transform.position, cR.fenceObj.transform.GetChild(0).position) > .1)
                {
                    pF2D.AIGetMoveCommand(cR.fenceObj.transform.GetChild(0).position);
                    AnimationManager.Instance.RunAnim(cR.animator, 1);
                }

                // Çit kapısının önünde dur ve koyunu çite gönder
                if (Vector2.Distance(cR.transform.position, cR.fenceObj.transform.GetChild(0).position) < .1)
                {
                    cR.sheep.CheckFences();
                    cR.sheep.goFence = true;
                    cR.returnFences = false;
                    cR.isSheep = false;
                }
            }
        }
    }
}
