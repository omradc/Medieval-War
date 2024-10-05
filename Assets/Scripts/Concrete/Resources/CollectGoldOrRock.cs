using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectGoldOrRock
    {
        CollectResourcesController cR;
        UnitPathFinding2D pF2D;

        public CollectGoldOrRock(CollectResourcesController collectResources, UnitPathFinding2D pathFinding2D)
        {
            cR = collectResources;
            pF2D = pathFinding2D;
        }
        public void GoToMine()
        {
            if (cR.isTree || cR.isMineEmpty || !cR.isMine || cR.isSheep) return;
            // Hedef varsa ona git
            if (cR.targetResource != null && !cR.returnHome && cR.mine.currentMineAmount > 0)
            {
                // Hedefe ulaşınca dur
                if (Vector2.Distance(cR.transform.position, cR.targetResource.transform.position) > .1f)
                {
                    pF2D.AIGetMoveCommand(cR.targetResource.transform.position);
                    AnimationManager.Instance.RunAnim(cR.animator, 1);
                }

                // Hedefe ulaşıldı
                else
                {
                    if (cR.mine.currentMineAmount == 0) return;
                    cR.villagerSpriteRenderer.enabled = false;

                    //cR.tMining += Time.deltaTime;
                    cR.tMining += 1;
                    if (cR.tMining > cR.miningTime)
                    {
                        // Madenden alınan kaynakları eksilt
                        if (cR.mine.CompareTag("GoldMine"))
                        {
                            cR.mine.currentMineAmount -= cR.collectGoldAmount;
                            cR.mine.mineAmountFillValue.fillAmount = cR.mine.currentMineAmount / cR.mine.mineAmount;
                        }
                        if (cR.mine.CompareTag("RockMine"))
                        {
                            cR.mine.currentMineAmount -= cR.collectRockAmount;
                            cR.mine.mineAmountFillValue.fillAmount = cR.mine.currentMineAmount / cR.mine.mineAmount;
                        }
                        cR.villagerSpriteRenderer.enabled = true;
                        cR.returnHome = true;
                        cR.workOnce = true;
                        cR.workOnce2 = true;
                        cR.tMining = 0;
                    }

                }
            }
        }
    }
}

