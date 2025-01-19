using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;


namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectWood
    {
       readonly VillagerController vC;
       readonly PathFindingController pF;
       TreeController tree;


        public CollectWood(VillagerController vC, PathFindingController pF)
        {
            this.vC = vC;
            this.pF = pF;
        }
        void RefreshTrees()
        {
            vC.trees = Physics2D.OverlapCircleAll(vC.targetResource.transform.position, vC.currentChopTreeSightRange, vC.treeLayer);
            vC.nearestTree = DetechNearestTree();
            if (vC.trees.Length == 0)
                vC.isTree = false;
        }
        public void GoToTree()
        {

            // Ağaç seçilmediyse veya maden seçildiyse ağaç kesmeye gitme.
            if (!vC.isTree) return;

            // Hedef varsa ona git
            if (vC.targetResource != null && !vC.returnHome)
            {
                // İlk ağacı bulur
                if (vC.workOnceForTree)
                {
                    RefreshTrees();

                    if (vC.nearestTree != null)
                    {
                        // Seçilen ilk ağaca gider
                        if (vC.isFirstTree)
                        {
                            vC.treeChopPos = vC.targetResource.transform.GetChild(1).position;
                            tree = vC.targetResource.GetComponent<TreeController>();
                        }

                        // Sonra en yakın ağaçlara gider
                        else
                        {
                            vC.treeChopPos = vC.nearestTree.transform.GetChild(1).position;
                            tree = vC.nearestTree.GetComponent<TreeController>();

                        }

                    }
                    vC.workOnceForTree = false;
                }

                // Hedef boşsa çalışma
                if (vC.nearestTree == null) return;
                // Ağaca doğru gider
                if (Vector2.Distance(vC.transform.position, vC.treeChopPos) > .1f)
                {
                    pF.MoveAI(vC.treeChopPos, 0);

                    if (tree.isTreeAlreadyCutted)
                    {
                        vC.workOnceForTree = true;
                        return;
                    }
                }

                // Ağacı Kes
                if (Vector2.Distance(vC.transform.position, vC.treeChopPos) < .1f && pF.isStoping)
                {
                    AnimationManager.Instance.ChopTreeAnim(vC.animator, vC.chopSpeed);
                }
            }
        }
        public void Chop()  // Ağaç kesme animasyonu oynadığında; Chop event i ile tetiklenir
        {
            // Ağacı kes
            if (vC.nearestTree != null)
            {
                if (!vC.isFirstTree)
                {
                    tree = vC.nearestTree.GetComponent<TreeController>();
                }
                if (vC.isFirstTree)
                {
                    tree = vC.targetResource.GetComponent<TreeController>();
                }

                tree.GetHit(vC.currentTreeDamage, vC.woodCollectTime);

                //Ağaç yıkıldıysa eve dön
                if (tree.destruct)
                {
                    AnimationManager.Instance.IdleAnim(vC.animator);
                    vC.isFirstTree = false;
                    if (!tree.isTreeAlreadyCutted)
                    {
                        vC.returnHome = true;
                        vC.tCollect = 0;
                        tree.IsTreeAlreadyCutted(true);
                    }

                    if (tree.isTreeAlreadyCutted)
                    {
                        vC.workOnceForTree = true;
                    }
                }

            }

            vC.workOnce = true;
            vC.workOnce2 = true;
        }
        public void GetHitTree() // Ağaç kesme animasyonu oynadığında; GetHitTree event i ile tetiklenir
        {
            if (vC.nearestTree != null && !tree.isTreeAlreadyCutted)
            {
                tree.GetHitTreeAnim(vC.chopSpeed);
            }
        }
        GameObject DetechNearestTree()
        {
            GameObject nearestTarget = null;
            float shortestDistance = Mathf.Infinity;

            for (int i = 0; i < vC.trees.Length; i++)
            {
                if (vC.trees[i] != null)
                {
                    float distanceToEnemy = Vector2.Distance(vC.transform.position, vC.trees[i].transform.position);

                    if (shortestDistance > distanceToEnemy)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestTarget = vC.trees[i].gameObject;
                    }
                }
            }

            return nearestTarget;
        }
    }
}
