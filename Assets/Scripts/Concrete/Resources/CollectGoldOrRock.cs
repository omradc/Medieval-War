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
            if (pawnController.isMineEmpty || pawnController.mineController==null) return;
            Debug.Log("GoToMine2");

            if (!pawnController.returnHome && pawnController.mineController.currentMineAmount > 0)
            {
                Debug.Log("GoToMine3");
                // Hedefe ulaşınca dur
                if (Vector2.Distance(pawnController.transform.position, pawnController.mineController.transform.position) > .1f)
                {
                    Debug.Log("GoToMine4");
                    pF.MoveAI(pawnController.mineController.transform.position, 0);
                }

                // Hedefe ulaşıldı
                else
                {
                    Debug.Log("GoToMine5");
                    AnimationManager.Instance.RunCarryAnim(pawnController.animator, 1);
                    if (pawnController.mineController.currentMineAmount == 0) return;
                    pawnController.villagerSpriteRenderer.enabled = false;
                    pawnController.tMining += 1;
                    if (pawnController.tMining > pawnController.miningTime)
                    {
                        Debug.Log("GoToMine6");
                        // Madenden alınan kaynakları eksilt
                        if (pawnController.mineController.CompareTag("GoldMine"))
                        {
                            pawnController.mineController.currentMineAmount -= ResourcesManager.Instance.collectGoldAmount;
                            pawnController.mineController.mineAmountFillValue.fillAmount = pawnController.mineController.currentMineAmount / pawnController.mineController.mineAmount;
                        }
                        if (pawnController.mineController.CompareTag("RockMine"))
                        {
                            pawnController.mineController.currentMineAmount -= ResourcesManager.Instance.collectRockAmount;
                            pawnController.mineController.mineAmountFillValue.fillAmount = pawnController.mineController.currentMineAmount / pawnController.mineController.mineAmount;
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

