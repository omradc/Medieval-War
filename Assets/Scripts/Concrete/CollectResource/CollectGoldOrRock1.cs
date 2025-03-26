
using Assets.Scripts.Concrete.Movements;

namespace Assets.Scripts.Concrete.CollectResource
{
    class CollectGoldOrRock1
    {
        readonly CollectResourceController cRC;
        readonly PathFinding pF;

        public CollectGoldOrRock1(CollectResourceController cRC, PathFinding pF)
        {
            this.cRC = cRC;
            this.pF = pF;
        }

        //public void GoToMine()
        //{
        //    if (cRC.isMineEmpty || !cRC.isMine) return;
        //    // Hedef varsa ona git
        //    if (cRC.targetResource != null && !cRC.returnHome && cRC.mineControler.currentMineAmount > 0)
        //    {
        //        // Hedefe ulaşınca dur
        //        if (Vector2.Distance(cRC.transform.position, cRC.targetResource.transform.position) > .1f)
        //        {
        //            pF.MoveAI(cRC.targetResource.transform.position, 0);
        //        }
        //        // Hedefe ulaşıldı
        //        else
        //        {
        //            AnimationManager.Instance.RunCarryAnim(cRC.animator, 1);
        //            if (cRC.mineControler.currentMineAmount == 0) return;
        //            cRC.pawnSpriteRenderer.enabled = false;
        //            cRC.tMining += 1;
        //            if (cRC.tMining > cRC.miningTime)
        //            {
        //                // Madenden alınan kaynakları eksilt
        //                if (cRC.targetResource.CompareTag("GoldMine"))
        //                {
        //                    cRC.mineControler.currentMineAmount -= ResourcesManager.Instance.collectGoldAmount;
        //                    cRC.mineControler.mineAmountFillValue.fillAmount = cRC.mineControler.currentMineAmount / cRC.mineControler.mineAmount;
        //                }
        //                if (cRC.targetResource.CompareTag("RockMine"))
        //                {
        //                    cRC.mineControler.currentMineAmount -= ResourcesManager.Instance.collectRockAmount;
        //                    cRC.mineControler.mineAmountFillValue.fillAmount = cRC.mineControler.currentMineAmount / cRC.mineControler.mineAmount;
        //                }
        //                cRC.pawnSpriteRenderer.enabled = true;
        //                cRC.returnHome = true;
        //                cRC.workOnce = true;
        //                cRC.workOnce2 = true;
        //                cRC.tMining = 0;
        //            }
        //        }
        //    }
        //}
    }
}