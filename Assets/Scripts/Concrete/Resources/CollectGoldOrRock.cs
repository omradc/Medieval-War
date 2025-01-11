using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectGoldOrRock
    {
        VillagerController vC;
        PathFindingController pF;

        public CollectGoldOrRock(VillagerController vC, PathFindingController pF)
        {
            this.vC = vC;
            this.pF = pF;
        }
        public void GoToMine()
        {
            if (vC.isMineEmpty || !vC.isMine) return;
            // Hedef varsa ona git
            if (vC.targetResource != null && !vC.returnHome && vC.mine.currentMineAmount > 0)
            {
                // Hedefe ulaşınca dur
                if (Vector2.Distance(vC.transform.position, vC.targetResource.transform.position) > .1f)
                {
                    pF.MoveAI(vC.targetResource.transform.position);
                }

                // Hedefe ulaşıldı
                else
                {
                    AnimationManager.Instance.RunCarryAnim(vC.animator, 1);
                    if (vC.mine.currentMineAmount == 0) return;
                    vC.villagerSpriteRenderer.enabled = false;
                    vC.tMining += 1;
                    if (vC.tMining > vC.miningTime)
                    {
                        // Madenden alınan kaynakları eksilt
                        if (vC.mine.CompareTag("GoldMine"))
                        {
                            vC.mine.currentMineAmount -= ResourcesManager.Instance.collectGoldAmount;
                            vC.mine.mineAmountFillValue.fillAmount = vC.mine.currentMineAmount / vC.mine.mineAmount;
                        }
                        if (vC.mine.CompareTag("RockMine"))
                        {
                            vC.mine.currentMineAmount -= ResourcesManager.Instance.collectRockAmount;
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

