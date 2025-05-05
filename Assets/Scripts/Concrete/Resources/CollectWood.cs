//using Assets.Scripts.Concrete.Controllers;
//using Assets.Scripts.Concrete.Managers;
//using Assets.Scripts.Concrete.Movements;
//using UnityEngine;


//namespace Assets.Scripts.Concrete.Resources
//{
//    internal class CollectWood
//    {
//       readonly PawnController pawnController;
//       readonly PathFinding pF;
//       TreeController tree;


//        public CollectWood(PawnController pawnController, PathFinding pF)
//        {
//            this.pawnController = pawnController;
//            this.pF = pF;
//        }
//        void RefreshTrees()
//        {
//            pawnController.trees = Physics2D.OverlapCircleAll(pawnController.targetResource.transform.position, pawnController.currentChopTreeSightRange, pawnController.treeLayer);
//            pawnController.nearestTree = DetechNearestTree();
//            if (pawnController.trees.Length == 0)
//                pawnController.isTree = false;
//        }
//        public void GoToTree()
//        {

//            // Ağaç seçilmediyse veya maden seçildiyse ağaç kesmeye gitme.
//            if (!pawnController.isTree) return;

//            // Hedef varsa ona git
//            if (pawnController.targetResource != null && !pawnController.returnHome)
//            {
//                // İlk ağacı bulur
//                if (pawnController.workOnceForTree)
//                {
//                    RefreshTrees();

//                    if (pawnController.nearestTree != null)
//                    {
//                        // Seçilen ilk ağaca gider
//                        if (pawnController.isFirstTree)
//                        {
//                            pawnController.treeChopPos = pawnController.targetResource.transform.GetChild(1).position;
//                            tree = pawnController.targetResource.GetComponent<TreeController>();
//                        }

//                        // Sonra en yakın ağaçlara gider
//                        else
//                        {
//                            pawnController.treeChopPos = pawnController.nearestTree.transform.GetChild(1).position;
//                            tree = pawnController.nearestTree.GetComponent<TreeController>();
//                            pawnController.targetResource = pawnController.nearestTree;

//                        }

//                    }
//                    pawnController.workOnceForTree = false;
//                }

//                // Hedef boşsa çalışma
//                if (pawnController.nearestTree == null) return;
//                // Ağaca doğru gider
//                if (Vector2.Distance(pawnController.transform.position, pawnController.treeChopPos) > .1f)
//                {
//                    pF.MoveAI(pawnController.treeChopPos, 0);

//                    if (tree.isTreeAlreadyCutted)
//                    {
//                        pawnController.workOnceForTree = true;
//                        return;
//                    }
//                }

//                // Ağacı Kes
//                if (Vector2.Distance(pawnController.transform.position, pawnController.treeChopPos) < .1f && pF.isStopping)
//                {
//                    AnimationManager.Instance.ChopTreeAnim(pawnController.animator, pawnController.chopSpeed);
//                }
//            }
//        }
//        public void Chop()  // Ağaç kesme animasyonu oynadığında; Chop event i ile tetiklenir
//        {
//            // Ağacı kes
//            if (pawnController.nearestTree != null)
//            {
//                if (!pawnController.isFirstTree)
//                {
//                    tree = pawnController.nearestTree.GetComponent<TreeController>();
//                }
//                if (pawnController.isFirstTree)
//                {
//                    tree = pawnController.targetResource.GetComponent<TreeController>();
//                }

//                tree.GetHit(pawnController.currentTreeDamage, pawnController.woodCollectTime);

//                //Ağaç yıkıldıysa eve dön
//                if (tree.destruct)
//                {
//                    AnimationManager.Instance.IdleAnim(pawnController.animator);
//                    pawnController.isFirstTree = false;
//                    if (!tree.isTreeAlreadyCutted)
//                    {
//                        pawnController.returnHome = true;
//                        pawnController.tCollect = 0;
//                        tree.IsTreeAlreadyCutted(true);
//                    }

//                    if (tree.isTreeAlreadyCutted)
//                    {
//                        pawnController.workOnceForTree = true;
//                    }
//                }

//            }

//            pawnController.workOnce = true;
//            pawnController.workOnce2 = true;
//        }
//        public void GetHitTree() // Ağaç kesme animasyonu oynadığında; GetHitTree event i ile tetiklenir
//        {
//            if (pawnController.nearestTree != null && !tree.isTreeAlreadyCutted)
//            {
//                tree.GetHitTreeAnim(pawnController.chopSpeed);
//            }
//        }
//        GameObject DetechNearestTree()
//        {
//            GameObject nearestTarget = null;
//            float shortestDistance = Mathf.Infinity;

//            for (int i = 0; i < pawnController.trees.Length; i++)
//            {
//                if (pawnController.trees[i] != null)
//                {
//                    float distanceToEnemy = Vector2.Distance(pawnController.transform.position, pawnController.trees[i].transform.position);

//                    if (shortestDistance > distanceToEnemy)
//                    {
//                        shortestDistance = distanceToEnemy;
//                        nearestTarget = pawnController.trees[i].gameObject;
//                    }
//                }
//            }

//            return nearestTarget;
//        }
//    }
//}
