﻿using Assets.Scripts.Concrete.Combats;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using System;
using UnityEngine;

namespace Assets.Scripts.Concrete.AI
{
    internal class KnightAI
    {
        public GameObject target;
        public Transform nearestAttackPoint;
        readonly KnightController kC;
        readonly PathFinding pF;
        GameObject tower;
        SpriteRenderer towerVisual;
        BuildingController bC;
        Transform towerPos;
        Vector2 gatePos;
        Vector2 pos;
        bool workOnce;
        Transform obj;
        public KnightAI(KnightController kC, PathFinding pF)
        {
            this.kC = kC;
            this.pF = pF;
        }

        GameObject ChooseTarget()
        {
            GameObject bestTarget = null;
            float bestScore = Mathf.Infinity;

            for (int i = 0; i < kC.targetEnemies.Length; i++)
            {
                float distance = Vector2.Distance(kC.transform.position, kC.targetEnemies[i].transform.position);
                TargetPriority targetPriority = kC.targetEnemies[i].GetComponent<TargetPriority>();
                float currentScore = targetPriority.attackingPersonNumber + distance - targetPriority.currentPriority;

                if (bestScore > currentScore)
                {
                    bestScore = currentScore;
                    bestTarget = kC.targetEnemies[i].gameObject;
                }
            }
            return bestTarget;
        }
        public void CatchNeraestTarget()
        {
            target = ChooseTarget();

            if (target == null) return;

            CalculateNearestAttackPoint();

            if (Vector2.Distance(nearestAttackPoint.position, kC.sightRangePosition) < kC.currentSightRange)
            {
                // hedef, saldırı menziline girerse; yakalamayı bırak
                if (Vector2.Distance(nearestAttackPoint.position, kC.attackRangePosition) < kC.currentAttackRange) return;
                pF.MoveAI(nearestAttackPoint.position, kC.currentAttackRange);
            }

        }
        void CalculateNearestAttackPoint()
        {
            // Düşman goblin ise
            if (target.layer == 13)
            {
                nearestAttackPoint = target.transform;
                return;
            }

            for (int i = 0; i < target.transform.childCount; i++)
            {
                if (target.transform.GetChild(i).name == "AttackPoints")
                    obj = target.transform.GetChild(i);
            }
            if (obj == null)
                Debug.Log("AttackPoints Bulunanamadı");

            // Saldırı noktası sayısı 1 ise
            if (obj.transform.childCount == 0)
            {
                nearestAttackPoint = obj;
                return;
            }

            // Saldırı noktası sayısı 1 den büyük ise
            float shortestDistance = Mathf.Infinity;
            for (int i = 0; i < obj.childCount; i++)
            {
                float distanceToTarget = Vector2.Distance(kC.transform.position, obj.GetChild(i).position);
                if (shortestDistance > distanceToTarget)
                {
                    shortestDistance = distanceToTarget;
                    nearestAttackPoint = obj.GetChild(i);
                }
            }

        }
        public void UnitBehaviours()
        {
            if (kC.unitOrderEnum == KnightOrderEnum.AttackOrder)
                Attack();
            if (kC.unitOrderEnum == KnightOrderEnum.DefendOrder)
                Defend();
            if (kC.unitOrderEnum == KnightOrderEnum.FollowOrder)
                Follow();
            if (kC.unitOrderEnum == KnightOrderEnum.StayOrder)
                Stay();
        }
        void Attack()
        {
            kC.sightRangePosition = kC.transform.GetChild(0).position;
            kC.currentSightRange = kC.sightRange;
        }
        void Defend()
        {
            kC.sightRangePosition = pF.lastMousePos; // Seçili pozisyon merkezdir
            kC.currentSightRange = kC.sightRange;
            if (kC.targetEnemies.Length == 0) // Düşman yoksa merkeze dön
            {
                pF.MoveAI(kC.sightRangePosition, 0);
                kC.direction.Turn2DirectionWithPos(kC.sightRangePosition.x);
            }
        }
        void Follow()
        {
            kC.currentSightRange = kC.sightRange;
            // Hedef belirlendiyse menzilini ona ver ve takip et
            if (kC.followingObj != null)
                kC.sightRangePosition = kC.followingObj.transform.position;

            // Kendi saldırı menzilini geçmeyecek şekilde, görüş menzilinde düşman yoksa hedefi (kendi görüş menzilini) takip et
            if (kC.targetEnemies.Length == 0 && Vector2.Distance((Vector3)kC.sightRangePosition, kC.transform.position) > kC.currentAttackRange)
            {
                pF.MoveAI(kC.sightRangePosition, kC.currentAttackRange);
            }

            // Görüş menzilde düşman varsa, menzilden çıkmayacak şekilde düşmanı takip et
            if (kC.targetEnemies.Length > 0 && Vector2.Distance((Vector3)kC.sightRangePosition, kC.transform.position) < kC.currentSightRange)
            {
                if (target == null) return;
                pF.MoveAI(target.transform.position, kC.currentAttackRange);
            }

        }
        void Stay()
        {
            kC.currentSightRange = kC.currentAttackRange;
            kC.sightRangePosition = kC.transform.GetChild(0).position;
        }
        public void SelectTower()  // Update ile çalışır
        {

            if (kC.isSeleceted && !kC.onBuilding)
            {
                // Kuleye basılı tutulduğu sürece çalışır. Update.
                if (InteractManager.Instance.interactedTower != null)
                {
                    tower = InteractManager.Instance.interactedTower;
                    workOnce = true;
                    kC.aI = false;

                    // Etrafta düşman varken yapay zeka kapatıldığında düşmanın son konumuna gitmemesi için, yolları temizle
                    pF.agent.ResetPath();
                }
            }
        }
        public void GoTower()   // Optimum
        {
            if (tower != null)
            {
                // Kuleye git
                if (workOnce)
                {
                    Debug.Log("kuleye git");
                    gatePos = tower.transform.GetChild(0).position;
                    towerPos = tower.transform.GetChild(1);
                    bC = tower.GetComponent<BuildingController>();
                    towerVisual = tower.transform.GetChild(2).GetComponent<SpriteRenderer>();
                    if (bC.isFull)
                    {
                        tower = null; // eğer birim kuledeyken, kuleye tıklarsa; kodun devamlılığını sağlar
                        return;
                    }
                    pF.MoveAI(gatePos, 0);
                    kC.onBuildingStay = true;
                    kC.goBuilding = true;
                    workOnce = false;

                }

                // Kule dolu değilse, sıkışmaları engellemek için collider ları kapat
                if (!bC.isFull && Vector2.Distance(kC.transform.position, gatePos) <= 1f)
                {
                    kC.knightCollider.isTrigger = true;
                    pF.agent.radius = 0;

                }

                // Kuleye çık
                if (Vector2.Distance(kC.transform.position, gatePos) <= .1f)
                {
                    Debug.Log("Kuledeyim");
                    kC.goBuilding = false;
                    kC.currentSightRange = kC.currentAttackRange;
                    kC.unitOrderEnum = KnightOrderEnum.StayOrder;
                    bC.interactableObjUIController.interactablePanel.SetActive(false); // Etkileşim ekranını kapat

                    // Kulede birim varsa, çıkma
                    if (bC.isFull)
                    {
                        kC.aI = true;
                        pF.agent.ResetPath();
                        kC.onBuildingStay = false;
                        kC.unitOrderEnum = KnightOrderEnum.AttackOrder;
                        tower = null;
                        return;
                    }

                    // Kulede birim yoksa, çık
                    if (!bC.isFull)
                    {
                        Debug.Log("Kuleye çık");
                        kC.aI = true;
                        CalculateTowerPos();
                        pF.agent.ResetPath(); // kulede kal
                        kC.transform.position = pos; // Birimi kuleye ışınla
                        kC.onBuilding = true;
                        kC.gameObject.layer = 25; // ölümsüz ol
                        kC.isSeleceted = false;
                        kC.visual.sortingOrder = towerVisual.sortingOrder + 1;
                        InteractManager.Instance.selectedKnights.Remove(kC.gameObject); // Kuledeyse seçimi kaldır
                        InteractManager.Instance.SelectedObjColor(1, kC.gameObject);  // Kuledeyse seçimi kaldır

                        tower = null;
                    }
                }
            }

            // Kuleden in
            if (kC.onBuilding && kC.isSeleceted)
            {
                Debug.Log("Kuleden in");
                ActivateTowerPos();
                kC.gameObject.layer = 6; // ölümlü ol
                kC.onBuilding = false;
                kC.transform.position = gatePos; // kulenin kapısına git
                kC.onBuildingStay = false;
                kC.unitOrderEnum = KnightOrderEnum.AttackOrder;
                bC.isFull = false; // Kulede birim var
            }

            if (kC.onBuilding)
            {
                kC.transform.position = pos; // Birimi kuleye ışınla
            }

            if (tower == null)
            {
                kC.knightCollider.isTrigger = false; // colliderları aç
                pF.agent.radius = kC.knightCollider.radius; // colliderları aç
            }


        }
        public void DestructTower()
        {
            if (bC == null) return;
            if (bC.destruct && kC.onBuilding)
            {
                Debug.Log("Kuleden düş");
                ActivateTowerPos();
                kC.gameObject.layer = 6; // ölümlü ol
                kC.onBuilding = false;
                kC.transform.position = gatePos; // kulenin kapısına git
                kC.knightCollider.isTrigger = false; // collider ı aç
                pF.agent.radius = kC.knightCollider.radius; // collider ı aç
                kC.onBuildingStay = false;
                kC.unitOrderEnum = KnightOrderEnum.AttackOrder;
                bC.isFull = false; // Kulede birim var
            }
        }
        void CalculateTowerPos()
        {
            // Kale ise
            if (towerPos.childCount > 0)
            {
                for (int i = 0; i < towerPos.childCount; i++)
                {
                    if (towerPos.GetChild(i).transform.gameObject.activeSelf)
                    {
                        kC.towerPosIndex = i;
                        towerPos.GetChild(i).gameObject.SetActive(false);
                        pos = towerPos.GetChild(i).transform.position;
                        bC.unitValue++;
                        break;
                    }

                }
                if (bC.unitValue == towerPos.childCount)
                    bC.isFull = true; // Kulede birim var
            }

            // Kule ise
            else
            {
                bC.isFull = true; // Kulede birim var
                pos = towerPos.position;
            }
        }
        void ActivateTowerPos()
        {
            if (towerPos.childCount > 0)
            {
                towerPos.GetChild(kC.towerPosIndex).gameObject.SetActive(true);
                bC.unitValue--;
            }

            bC.isFull = false;
        }
    }
}

