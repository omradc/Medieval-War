using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Concrete.UnitAIs
{
    internal class TowerAI
    {
        GameObject tower;
        KnightController kC;
        //PathFinding2D pF2D;
        PathFindingController pF;
        BuildingController bC;
        SpriteRenderer unitSpriteRenderer;
        Transform towerPos;
        Vector2 gatePos;
        Vector2 pos;
        bool workOnce;
        float time;
        float timeToGetOffTower = 1;
        public TowerAI(KnightController kC, PathFindingController pF)
        {
            this.kC = kC;
            this.pF = pF;
            unitSpriteRenderer = kC.transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        // Update ile çalışır
        public void SelectTower()
        {
            if (kC.isSeleceted && !kC.onBuilding)
            {
                // Kuleye basılı tutulduğu sürece çalışır. Update.
                if (InteractManager.Instance.interactedTower != null)
                {
                    tower = InteractManager.Instance.interactedTower;
                    workOnce = true;
                    kC.aI = false;

                    //// Etrafta düşman varken yapay zeka kapatıldığında düşmanın son konumuna gitmemesi için, yolları temizle
                    //pF2D.path.Clear();
                    //pF2D.pathLeftToGo.Clear();

                }
            }
        }

        // Optimum
        public void GoTower()
        {

            if (tower != null)
            {
                // Kuleye git
                if (workOnce)
                {
                    Debug.Log("kuleye git");
                    kC.unitOrderEnum = UnitOrderEnum.StayOrder;

                    gatePos = tower.transform.GetChild(0).position;
                    towerPos = tower.transform.GetChild(1);
                    bC = tower.GetComponent<BuildingController>();
                    if (bC.isFull)
                    {
                        tower = null; // eğer birim kuledeyken, kuleye tıklarsa; kodun devamlılığını sağlar
                        return;
                    }

                    //pF2D.AIGetMoveCommand(gatePos);
                    pF.MoveAI(gatePos);
                    AnimationManager.Instance.RunAnim(kC.animator, 1);
                    kC.stayBuilding = true;
                    kC.goBuilding = true;
                    workOnce = false;


                }

                // Kuleye çık
                if (Vector2.Distance(kC.transform.position, gatePos) <= pF.agent.stoppingDistance)
                {
                    unitSpriteRenderer.enabled = false;
                    bC.buildingPanelController.InteractablePanelVisibility(false); // Etkileşim ekranını kapat
                    if (bC.isFull)
                    {
                        unitSpriteRenderer.enabled = true;
                        
                        time = 0;
                        return;
                    }

                    time++;
                    // Kulede birim yoksa, çık
                    if (time > timeToGetOffTower && !bC.isFull)
                    {
                        Debug.Log("Kuleye çık");
                        unitSpriteRenderer.enabled = true;
                        unitSpriteRenderer.sortingOrder = 12;
                        kC.aI = true;
                        //pF2D.isPathEnd = true; // Dur
                        CalculateTowerPos();
                        kC.transform.position = pos; // Birimi kuleye ışınla
                        kC.onBuilding = true;
                        kC.circleCollider.isTrigger = true; // kulenin çarpıştırıcısı ile etkileşime girmesin
                        kC.gameObject.layer = 25; // ölümsüz ol
                        AnimationManager.Instance.IdleAnim(kC.animator);
                        tower = null;
                        time = 0;
                    }
                }
            }

            // Kuleden in
            if (kC.onBuilding && kC.isSeleceted /*&& !pF2D.isPathEnd*/)
            {
                unitSpriteRenderer.enabled = false;
                time++;
                if (time > timeToGetOffTower)
                {
                    Debug.Log("Kuleden in");
                    ActivateTowerPos();
                    unitSpriteRenderer.enabled = true;
                    unitSpriteRenderer.sortingOrder = 10;
                    kC.gameObject.layer = 6; // ölümlü ol
                    kC.onBuilding = false;
                    kC.transform.position = gatePos; // kulenin kapısına git
                    kC.circleCollider.isTrigger = false;
                    kC.stayBuilding = false;
                    kC.goBuilding = false;
                    bC.isFull = false; // Kulede birim var
                    time = 0;
                }
            }
        }

        public void DestructTower()
        {
            if (bC == null) return;
            if (bC.destruct && kC.onBuilding)
            {
                Debug.Log("Kuleden düş");
                ActivateTowerPos();
                unitSpriteRenderer.enabled = true;
                kC.gameObject.layer = 6; // ölümlü ol
                kC.onBuilding = false;
                kC.transform.position = gatePos; // kulenin kapısına git
                kC.circleCollider.isTrigger = false;
                kC.stayBuilding = false;
                kC.goBuilding = false;
                bC.isFull = false; // Kulede birim var
                time = 0;
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