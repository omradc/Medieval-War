using Assets.Scripts.Concrete.Controllers;
using UnityEngine;
namespace Assets.Scripts.Concrete.KnightBuildings
{
    class KnightHouse : Building
    {
        GameObject troop;
        Vector3 pos;
        KnightHouseController knightHouseController;
        PanelController panelController;
        ButtonController buttonController;
        float currentTime;



        public KnightHouse(GameObject troop, Vector3 pos, PanelController panelController, ButtonController buttonController, KnightHouseController knightHouseController)
        {
            this.troop = troop;
            this.pos = pos;
            this.panelController = panelController;
            this.buttonController = buttonController;
            this.knightHouseController = knightHouseController;
        }
        public void TrainUnit()
        {

            if (buttonController.trainUnitButton)
            {

                currentTime += Time.deltaTime;
                knightHouseController.timerFillImage.fillAmount = currentTime / knightHouseController.trainingTime;
                if (currentTime >= knightHouseController.trainingTime)
                {
                    GameObject trainedUnit = Object.Instantiate(troop, pos, Quaternion.identity);
                    currentTime = 0;
                    buttonController.trainUnitButton = false;
                    panelController.TimerPanelVisibility(false);
                    if (trainedUnit.CompareTag("Villager"))
                        trainedUnit.GetComponent<VillagerController>().homePos = pos;
                }

            }

        }

        // Ev yükseltildiði anda yok edilir
        public void UpgradeHouse()
        {
            if (buttonController.upgrade)
            {
                Debug.Log("Upgrade");
                GameObject obj = Object.Instantiate(buttonController.upgrading, knightHouseController.transform.position, Quaternion.identity);
                obj.GetComponent<ConstructController>().building= buttonController.upgradeComplete;
                Object.Destroy(knightHouseController.gameObject);

            }
        }
    }
}

