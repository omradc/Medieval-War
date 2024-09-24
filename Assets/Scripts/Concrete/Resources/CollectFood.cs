using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectFood 
    {
        CollectResourcesController cR;
        PathFinding2D pF2D;
        public CollectFood(CollectResourcesController collectResources, PathFinding2D pathFinding2D)
        {
            cR = collectResources;
            pF2D = pathFinding2D;
        }
        public void GoToSheep()
        {
            if (!cR.isSheep || cR.isTree || cR.isMine) return;
            // Hedef varsa ona git
            if (cR.targetResource != null && !cR.returnFences && !cR.returnHome)
            {

                // Koyuna ulaşınca dur
                if (Vector2.Distance(cR.transform.position, cR.targetResource.transform.GetChild(1).position) > .1f)
                {
                    pF2D.AIGetMoveCommand(cR.targetResource.transform.GetChild(1).position);
                    AnimationManager.Instance.RunAnim(cR.animator, 1);
                }

                // Koyuna ulaşıldı
                else
                {
                    AnimationManager.Instance.IdleAnim(cR.animator);
                    // Koyunu evcilleştir
                    if (!cR.sheep.isDomestic)
                        cR.sheep.TameSheep(cR.gameObject, cR.fence);
                    //Koyun evcilleştiyse, çite gönder
                    if (cR.sheep.isDomestic && !cR.sheep.inFence)
                        cR.returnFences = true;
                    //Koyun çitteyse ve kaynağı dolu ise, eve dnüşü bekle
                    if (cR.sheep.inFence && cR.sheep.giveMeat)
                    {
                        cR.workOnce = true;
                        cR.workOnce2 = true;
                        cR.returnHome = true;
                        cR.tCollect = 0;
                        cR.sheep.giveMeat = false;
                        cR.sheep.DropMeat(cR.collectTime);
                    }


                }
            }
        }
        public void GoToFences()
        {
            if (!cR.returnFences) return;

            // Çit kapısına git
            if (Vector2.Distance(cR.transform.position, cR.fence.transform.GetChild(0).position) > .1)
            {
                pF2D.AIGetMoveCommand(cR.fence.transform.GetChild(0).position);
                AnimationManager.Instance.RunAnim(cR.animator, 1);
            }

            // Çit kapısının önünde dur ve koyunu çite gönder
            else
            {
                cR.isSheep = false;
                cR.sheep.goFence = true;
                cR.returnFences = false;
            }

        }
    }
}
