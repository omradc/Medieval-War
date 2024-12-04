using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectGoldOrRock
    {
        VillagerController vC;
        UnitPathFinding2D pF2D;

        public CollectGoldOrRock(VillagerController collectResources, UnitPathFinding2D pathFinding2D)
        {
            vC = collectResources;
            pF2D = pathFinding2D;
        }
        public void GoToMine()
        {
            if (vC.isTree || vC.isMineEmpty || !vC.isMine || vC.isSheep) return;
            // Hedef varsa ona git
            if (vC.targetResource != null && !vC.returnHome && vC.mine.currentMineAmount > 0)
            {
                // Hedefe ulaşınca dur
                if (Vector2.Distance(vC.transform.position, vC.targetResource.transform.position) > .1f)
                {
                    pF2D.AIGetMoveCommand(vC.targetResource.transform.position);
                    AnimationManager.Instance.RunAnim(vC.animator, 1);
                }

                // Hedefe ulaşıldı
                else
                {
                    if (vC.mine.currentMineAmount == 0) return;
                    vC.villagerSpriteRenderer.enabled = false;
                    vC.tMining += 1;
                    if (vC.tMining > vC.miningTime)
                    {
                        // Madenden alınan kaynakları eksilt
                        if (vC.mine.CompareTag("GoldMine"))
                        {
                            vC.mine.currentMineAmount -= vC.collectGoldAmount;
                            vC.mine.mineAmountFillValue.fillAmount = vC.mine.currentMineAmount / vC.mine.mineAmount;
                        }
                        if (vC.mine.CompareTag("RockMine"))
                        {
                            vC.mine.currentMineAmount -= vC.collectRockAmount;
                            vC.mine.mineAmountFillValue.fillAmount = vC.mine.currentMineAmount / vC.mine.mineAmount;
                        }
                        vC.villagerSpriteRenderer.enabled = true;
                        vC.returnHome = true;
                        vC.workOnce = true;
                        vC.workOnce2 = true;
                        vC.tMining = 0;
                    }

                }
            }
        }
    }
}

