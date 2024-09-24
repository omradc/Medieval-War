using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectGoldOrRock 
    {
        CollectResourcesController cR;
        PathFinding2D pF2D;

        public CollectGoldOrRock(CollectResourcesController collectResources, PathFinding2D pathFinding2D)
        {
            cR = collectResources;
            pF2D = pathFinding2D;
        }
        public void GoToMine()
        {
            //ağaç seçliyse veya maden boşsa veya maden seçilmediyse, madene gitme
            if (cR.isTree || cR.isMineEmpty || !cR.isMine || cR.isSheep) return;
            // Hedef varsa ona git
            if (cR.targetResource != null && !cR.returnHome)
            {
                // Hedefe ulaşınca dur
                if (Vector2.Distance(cR.transform.position, cR.targetResource.transform.position) > .5f)
                {
                    pF2D.AIGetMoveCommand(cR.targetResource.transform.position);
                    AnimationManager.Instance.RunAnim(cR.animator, 1);
                }

                // Hedefe ulaşıldı
                else
                {
                    if (!cR.returnHome)
                        cR.villagerSpriteRenderer.enabled = false;

                    cR.tMining += Time.deltaTime;
                    if (cR.tMining > cR.miningTime)
                    {
                        // Madenden alınan kaynakları eksilt
                        Mine mine = cR.targetResource.GetComponent<Mine>();
                        if (mine.CompareTag("GoldMine"))
                        {
                            mine.currentMineAmount -= cR.collectGoldAmount;
                            mine.mineAmountFillValue.fillAmount = mine.currentMineAmount / mine.mineAmount;
                        }
                        if (mine.CompareTag("RockMine"))
                        {
                            mine.currentMineAmount -= cR.collectRockAmount;
                            mine.mineAmountFillValue.fillAmount = mine.currentMineAmount / mine.mineAmount;
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

