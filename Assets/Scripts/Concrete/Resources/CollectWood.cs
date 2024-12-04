using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectWood
    {
        VillagerController vC;
        UnitPathFinding2D pF2D;
        Trees tree;
        public CollectWood(VillagerController collectResources, UnitPathFinding2D pathFinding2D)
        {
            vC = collectResources;
            pF2D = pathFinding2D;
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
            if (!vC.isTree || vC.isMine || vC.isSheep) return;

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
                            tree = vC.targetResource.GetComponent<Trees>();
                        }

                        // Sonra en yakın ağaçlara gider
                        else
                        {
                            vC.treeChopPos = vC.nearestTree.transform.GetChild(1).position;
                            tree = vC.nearestTree.GetComponent<Trees>();

                        }

                    }
                    vC.workOnceForTree = false;
                }

                // Hedef boşsa çalışma
                if (vC.nearestTree == null) return;
                // Kullanıcı komutu bittiği zaman, kendi kendine ağaca doğru gider
                if (Vector2.Distance(vC.transform.position, vC.treeChopPos) > .1f)
                {
                    pF2D.AIGetMoveCommand(vC.treeChopPos);
                    AnimationManager.Instance.RunAnim(vC.animator, 1);

                    if (tree.isTreeAlreadyCutted)
                    {
                        vC.workOnceForTree = true;
                        return;
                    }
                }

                // Ağacı Kes
                if (Vector2.Distance(vC.transform.position, vC.treeChopPos) < .1f)
                {
                    AnimationManager.Instance.ChopAnim(vC.animator, vC.chopSpeed);
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
                    tree = vC.nearestTree.GetComponent<Trees>();
                }
                if (vC.isFirstTree)
                {
                    tree = vC.targetResource.GetComponent<Trees>();
                }

                tree.GetHit(vC.currentTreeDamage, vC.woodCollectTime);
                //Ağaç yıkıldıysa eve dön
                if (tree.destruct)
                {
                    vC.isFirstTree = false;
                    if (!tree.isTreeAlreadyCutted)
                    {
                        vC.returnHome = true;
                        vC.tCollect = 0;
                        AnimationManager.Instance.IdleAnim(vC.animator);
                        tree.IsTreeAlreadyCutted(true);
                    }

                    if (tree.isTreeAlreadyCutted)
                    {
                        vC.workOnceForTree = true;
                    }
                }

            }
            if (vC.nearestTree == null)
            {
                AnimationManager.Instance.IdleAnim(vC.animator);
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
