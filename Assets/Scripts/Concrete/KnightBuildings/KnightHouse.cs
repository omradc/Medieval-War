using Assets.Scripts.Concrete.Controllers;
using UnityEngine;
namespace Assets.Scripts.Concrete.KnightBuildings
{
    class KnightHouse : Building
    {
        GameObject troop;
        Vector3 pos;
        KnightHouseController knightHouseController;
        BuildingPanelController panelController;
        ButtonController buttonController;
        float currentTime;



        public KnightHouse(GameObject troop, Vector3 pos, BuildingPanelController panelController, ButtonController buttonController, KnightHouseController knightHouseController)
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
    }
}

