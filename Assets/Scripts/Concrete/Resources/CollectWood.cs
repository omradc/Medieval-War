﻿using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;


namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectWood
    {
        CollectResourcesController cR;
        PathFinding2D pF2D;
        Tree tree;
        public CollectWood(CollectResourcesController collectResources, PathFinding2D pathFinding2D)
        {
            cR = collectResources;
            pF2D = pathFinding2D;
        }
        void RefreshTrees()
        {
            cR.trees = Physics2D.OverlapCircleAll(cR.targetResource.transform.position, cR.currentChopTreeSightRange, cR.treeLayer);
            cR.nearestTree = DetechNearestTree();
            if (cR.trees.Length == 0)
                cR.isTree = false;
        }
        public void GoToTree()
        {

            // Ağaç seçilmediyse veya maden seçildiyse ağaç kesmeye gitme.
            if (!cR.isTree || cR.isMine || cR.isSheep) return;

            // Hedef varsa ona git
            if (cR.targetResource != null && !cR.returnHome)
            {
                // İlk ağacı bulur
                if (cR.workOnceForTree)
                {
                    RefreshTrees();

                    if (cR.nearestTree != null)
                    {
                        cR.nearestTreeChopPos = CalculateNearestChopPos(cR.nearestTree);
                        tree = cR.nearestTree.GetComponent<Tree>();
                    }
                    cR.workOnceForTree = false;
                }

                // Hedef boşsa çalışma
                if (cR.nearestTree == null) return;
                // Kullanıcı komutu bittiği zaman, kendi kendine ağaca doğru gider
                if (Vector2.Distance(cR.transform.position, cR.nearestTreeChopPos) > .1f)
                {
                    pF2D.AIGetMoveCommand(cR.nearestTreeChopPos);
                    AnimationManager.Instance.RunAnim(cR.animator, 1);

                    if (tree.isTreeAlreadyCutted)
                    {
                        cR.workOnceForTree = true;
                        return;
                    }
                }

                // Ağacı Kes
                if (Vector2.Distance(cR.transform.position, cR.nearestTreeChopPos) < .1f)
                {
                    AnimationManager.Instance.ChopAnim(cR.animator, cR.chopSpeed);
                }
            }
        }
        public void Chop()  // Ağaç kesme animasyonu oynadığında; Chop event i ile tetiklenir
        {
            // Ağacı kes
            if (cR.nearestTree != null)
            {
                tree = cR.nearestTree.GetComponent<Tree>();
                tree.GetHit(cR.currentTreeDamagePoint, cR.woodCollectTime);
                //Ağaç yıkıldıysa eve dön
                if (tree.destruct)
                {
                    if (!tree.isTreeAlreadyCutted)
                    {
                        cR.returnHome = true;
                        cR.tCollect = 0;
                        AnimationManager.Instance.IdleAnim(cR.animator);
                        tree.IsTreeAlreadyCutted(true);
                    }

                    if (tree.isTreeAlreadyCutted)
                    {
                        cR.workOnceForTree = true;
                    }
                }

            }
            if (cR.nearestTree == null)
            {
                AnimationManager.Instance.IdleAnim(cR.animator);
            }

            cR.workOnce = true;
            cR.workOnce2 = true;
        }
        public void GetHitTree() // Ağaç kesme animasyonu oynadığında; GetHitTree event i ile tetiklenir
        {
            if (cR.nearestTree != null && !tree.isTreeAlreadyCutted)
            {
                tree = cR.nearestTree.GetComponent<Tree>();
                tree.GetHitTreeAnim(cR.direction.Turn2Direction(cR.nearestTree.transform.position.x), cR.chopSpeed);

            }
        }
        GameObject DetechNearestTree()
        {
            GameObject nearestTarget = null;
            float shortestDistance = Mathf.Infinity;

            for (int i = 0; i < cR.trees.Length; i++)
            {
                if (cR.trees[i] != null)
                {
                    float distanceToEnemy = Vector2.Distance(cR.transform.position, cR.trees[i].transform.position);

                    if (shortestDistance > distanceToEnemy)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestTarget = cR.trees[i].gameObject;
                    }
                }
            }

            return nearestTarget;
        }
        Vector2 CalculateNearestChopPos(GameObject obj) // Hedefin kesilecek noktalarından en yakınını bulur
        {
            float distance1 = Vector2.Distance(obj.transform.GetChild(1).position, cR.transform.position);
            float distance2 = Vector2.Distance(obj.transform.GetChild(2).position, cR.transform.position);
            if (distance1 < distance2)
            {
                return obj.transform.GetChild(1).position;
            }

            else
                return obj.transform.GetChild(2).position;
        }
    }
}