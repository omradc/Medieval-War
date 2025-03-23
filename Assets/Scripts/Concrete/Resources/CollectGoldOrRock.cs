using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectGoldOrRock
    {
        readonly PawnController pawnController;
        readonly PathFinding pF;

        public CollectGoldOrRock(PawnController pawnController, PathFinding pF)
        {
            this.pawnController = pawnController;
            this.pF = pF;
        }
        public void GoToMine()
        {
            Debug.Log("GoToMine");
            if (!pawnController.isMineEmpty && pawnController.mine != null) // Maden seçilmediyse ve boş değilse
            {
                Debug.Log("GoToMine2");
                // Hedef varsa ona git
                if (!pawnController.returnHome && pawnController.mine.currentMineAmount > 0)
                {
                    Debug.Log("GoToMine3");
                    // Hedefe ulaşınca dur
                    if (Vector2.Distance(pawnController.transform.position, pawnController.targetResource.transform.position) > .1f)
                    {
                        Debug.Log("GoToMine4");
                        pF.MoveAI(pawnController.targetResource.transform.position, 0);
                    }

                    // Hedefe ulaşıldı
                    else
                    {
                        Debug.Log("GoToMine5");
                        AnimationManager.Instance.RunCarryAnim(pawnController.animator, 1);
                        if (pawnController.mine.currentMineAmount == 0) return;
                        pawnController.villagerSpriteRenderer.enabled = false;
                        pawnController.tMining += 1;
                        if (pawnController.tMining > pawnController.miningTime)
                        {
                            Debug.Log("GoToMine6");
                            // Madenden alınan kaynakları eksilt
                            if (pawnController.mine.CompareTag("GoldMine"))
                            {
                                pawnController.mine.currentMineAmount -= ResourcesManager.Instance.collectGoldAmount;
                                pawnController.mine.mineAmountFillValue.fillAmount = pawnController.mine.currentMineAmount / pawnController.mine.mineAmount;
                            }
                            if (pawnController.mine.CompareTag("RockMine"))
                            {
                                pawnController.mine.currentMineAmount -= ResourcesManager.Instance.collectRockAmount;
                                pawnController.mine.mineAmountFillValue.fillAmount = pawnController.mine.currentMineAmount / pawnController.mine.mineAmount;
                            }
                            pawnController.villagerSpriteRenderer.enabled = true;
                            pawnController.returnHome = true;
                            pawnController.workOnce = true;
                            pawnController.workOnce2 = true;
                            pawnController.tMining = 0;
                        }

                    }
                }

            }
        }
    }
}

