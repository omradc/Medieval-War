using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Concrete.UnitAIs
{
    internal class GoTower
    {
        GameObject tower;
        UnitController uC;
        PathFinding2D pF2D;
        TowerController towerController;
        Vector2 gatePos;
        Vector2 towerPos;
        bool workOnce;
        float time;
        float timeToGetOffTower = 1;
        public GoTower(UnitController uC, PathFinding2D pF2D)
        {
            this.uC = uC;
            this.pF2D = pF2D;
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
                }
            }

        }

        // Optimum
        public void GoUpToTower()
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
                    uC.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                   towerController.hasUnit = true; // Kulede birim var
                    time++;
                    if (time > timeToGetOffTower)
                    {
                        Debug.Log("Kuleye çık");
                        uC.aI = true;
                        uC.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                        AnimationManager.Instance.IdleAnim(uC.animator);
                        pF2D.isPathEnd = true; // Dur
                        uC.transform.position = towerPos; // Birimi kuleye ışınla
                        uC.onBuilding = true;
                        uC.circleCollider.isTrigger = true; // kulenin çarpıştırıcısı ile etkileşime girmesin
                        uC.gameObject.layer = 25; // ölümsüz ol
                        tower = null;
                        time = 0;
                    }
                }
            }

            // Kuleden in
            if (uC.onBuilding && uC.isSeleceted && !pF2D.isPathEnd)
            {
                uC.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                time++;
                if (time > timeToGetOffTower)
                {
                    Debug.Log("Kuleden in");
                    towerController.hasUnit = false; // Kulede birim var
                    uC.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                    uC.gameObject.layer = 6; // ölümlü ol
                    uC.onBuilding = false;
                    uC.transform.position = gatePos; // kulenin kapısına git
                    uC.circleCollider.isTrigger = false;
                    uC.stayBuilding = false;
                    uC.goBuilding = false;
                    time = 0;
                }

            }


        }
    }
}