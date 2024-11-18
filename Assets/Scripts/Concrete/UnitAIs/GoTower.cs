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
                if (InteractManager.Instance.interactedTower != null)
                {
                    tower = InteractManager.Instance.interactedTower;
                    workOnce = true;
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
                    Debug.Log("Kuleye git");
                    uC.aI = false;
                    uC.unitOrderEnum = UnitOrderEnum.StayOrder;
                    towerController = tower.GetComponent<TowerController>();

                    if (towerController.hasUnit)
                    {
                        tower = null; // eğer birim kuledeyken, kuleye tıklarsa; kodun devamlılığını sağlar
                        return;
                    }
                    pF2D.AIGetMoveCommand(tower.transform.GetChild(0).position);
                    AnimationManager.Instance.RunAnim(uC.animator, 1);
                    uC.stayBuilding = true;
                    uC.goBuilding = true;
                    workOnce = false;

                }

                // Kuleye çık
                if (Vector2.Distance(uC.transform.position, tower.transform.GetChild(0).position) < .3f)
                {
                    uC.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                    time++;
                    if (time > timeToGetOffTower)
                    {

                        Debug.Log("Kuleye çık");
                        uC.aI = true;
                        uC.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                        AnimationManager.Instance.IdleAnim(uC.animator);
                        towerController.hasUnit = true; // Kulede birim var
                        pF2D.isPathEnd = true; // Dur
                        uC.transform.position = tower.transform.GetChild(1).position;
                        uC.onBuilding = true;
                        uC.circleCollider.isTrigger = true; // kulenin çarpıştırıcısı ile etkileşime girmesin
                        uC.goBuilding = false;
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
                if (time > timeToGetOffTower && towerController != null)
                {
                    uC.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                    Debug.Log("Kuleden in");
                    uC.gameObject.layer = 6; // ölümlü ol
                    uC.onBuilding = false;
                    uC.transform.position = towerController.transform.GetChild(0).position; // kulenin kapısına git
                    uC.circleCollider.isTrigger = false;
                    uC.stayBuilding = false;
                    towerController.hasUnit = false;
                    towerController = null;
                    time = 0;
                }
            }
        }


    }
}