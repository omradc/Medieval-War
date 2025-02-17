using Assets.Scripts.Concrete.Controllers;
using UnityEngine;
namespace Assets.Scripts.Concrete.KnightBuildings
{
    class KnightHouse : Building
    {
        GameObject troop;
        Vector3 pos;
        KnightHouseController knightHouseController;
        InteractableObjUIController interactableObjUIController;
        float currentTime;



        public KnightHouse(GameObject troop, Vector3 pos , InteractableObjUIController interactableObjUIController, KnightHouseController knightHouseController)
        {
            this.troop = troop;
            this.pos = pos;
            this.interactableObjUIController = interactableObjUIController;
            this.knightHouseController = knightHouseController;
        }
        public void TrainUnit()
        {

            if (interactableObjUIController.trainUnitButton)
            {
                currentTime += Time.deltaTime;
                knightHouseController.timerFillImage.fillAmount = currentTime / knightHouseController.trainingTime;
                if (currentTime >= knightHouseController.trainingTime)
                {
                    GameObject trainedUnit = Object.Instantiate(troop, pos, Quaternion.identity);
                    currentTime = 0;
                    interactableObjUIController.trainUnitButton = false;
                    interactableObjUIController.TimerPanelVisibility(false);
                    if (trainedUnit.CompareTag("Villager"))
                        trainedUnit.GetComponent<VillagerController>().homePos = pos;
                }
            }

        }
    }
}

