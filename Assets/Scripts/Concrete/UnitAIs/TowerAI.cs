﻿using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Concrete.UnitAIs
{
    internal class TowerAI
    {
        GameObject tower;
        UnitController uC;
        PathFinding2D pF2D;
        TowerController towerController;
        SpriteRenderer unitSpriteRenderer;
        Vector2 gatePos;
        Vector2 towerPos;
        bool workOnce;
        float time;
        float timeToGetOffTower = 1;

        public TowerAI(UnitController uC, PathFinding2D pF2D)
        {
            this.uC = uC;
            this.pF2D = pF2D;
            unitSpriteRenderer = uC.transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        // Update ile çalışır
        public void SelectTower()
        {
            if (uC.isSeleceted)
            {
                // Kuleye basılı tutulduğu sürece çalışır. Update.
                if (InteractManager.Instance.interactedTower != null)
                {
                    tower = InteractManager.Instance.interactedTower;
                    workOnce = true;
                    uC.aI = false;

                    // Etrafta düşman varken yapay zeka kapatıldığında düşmanın son konumuna gitmemesi için, yolları temizle
                    pF2D.path.Clear();
                    pF2D.pathLeftToGo.Clear();
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
                    uC.unitOrderEnum = UnitOrderEnum.StayOrder;
                    gatePos = tower.transform.GetChild(0).position;
                    towerPos = tower.transform.GetChild(1).position;
                    towerController = tower.GetComponent<TowerController>();
                    if (towerController.hasUnit)
                    {
                        tower = null; // eğer birim kuledeyken, kuleye tıklarsa; kodun devamlılığını sağlar
                        return;
                    }

                    pF2D.AIGetMoveCommand(gatePos);
                    AnimationManager.Instance.RunAnim(uC.animator, 1);
                    uC.stayBuilding = true;
                    uC.goBuilding = true;
                    workOnce = false;


                }

                // Kuleye çık
                if (Vector2.Distance(uC.transform.position, gatePos) < .3f)
                {
                    unitSpriteRenderer.enabled = false;
                    if (towerController.hasUnit)
                    {
                        unitSpriteRenderer.enabled = true;
                        time = 0;
                        return;
                    }

                    time++;
                    // Kulede birim yoksa, çık
                    if (time > timeToGetOffTower && !towerController.hasUnit)
                    {
                        Debug.Log("Kuleye çık");
                        towerController.hasUnit = true; // Kulede birim var
                        unitSpriteRenderer.enabled = true;
                        uC.aI = true;
                        pF2D.isPathEnd = true; // Dur
                        uC.transform.position = towerPos; // Birimi kuleye ışınla
                        uC.onBuilding = true;
                        uC.circleCollider.isTrigger = true; // kulenin çarpıştırıcısı ile etkileşime girmesin
                        uC.gameObject.layer = 25; // ölümsüz ol
                        AnimationManager.Instance.IdleAnim(uC.animator);
                        tower = null;
                        time = 0;
                    }
                }
            }

            // Kuleden in
            if (uC.onBuilding && uC.isSeleceted && !pF2D.isPathEnd)
            {
                unitSpriteRenderer.enabled = false;
                time++;
                if (time > timeToGetOffTower)
                {
                    Debug.Log("Kuleden in");
                    unitSpriteRenderer.enabled = true;
                    uC.gameObject.layer = 6; // ölümlü ol
                    uC.onBuilding = false;
                    uC.transform.position = gatePos; // kulenin kapısına git
                    uC.circleCollider.isTrigger = false;
                    uC.stayBuilding = false;
                    uC.goBuilding = false;
                    towerController.hasUnit = false; // Kulede birim var
                    time = 0;
                }
            }
        }

        public void DestructTower()
        {
            if (towerController == null) return;
            if (towerController.destruct && uC.onBuilding)
            {
                Debug.Log("Kuleden in");
                unitSpriteRenderer.enabled = true;
                uC.gameObject.layer = 6; // ölümlü ol
                uC.onBuilding = false;
                uC.transform.position = gatePos; // kulenin kapısına git
                uC.circleCollider.isTrigger = false;
                uC.stayBuilding = false;
                uC.goBuilding = false;
                towerController.hasUnit = false; // Kulede birim var

            }
        }
    }
}