using Assets.Scripts.Concrete.Buildings;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Resources;
using UnityEngine;

class Barrack : Building
{
    GameObject troop;
    Vector3 pos;
    BarrackController barrackController;
    public bool timeToTraining;

    PanelController panelController;
    ButtonController buttonController;
    float currentTime;


    public Barrack(GameObject troop, Vector3 pos, PanelController panelController, ButtonController buttonController, BarrackController barrackController)
    {
        this.troop = troop;
        this.pos = pos;
        this.panelController = panelController;
        this.buttonController = buttonController;
        this.barrackController = barrackController;
    }
    public void TrainUnit()
    {

        if (buttonController.trainUnitButton)
        {

            currentTime += Time.deltaTime;
            barrackController.timerFillImage.fillAmount = currentTime / barrackController.trainingTime;
            if (currentTime >= barrackController.trainingTime)
            {
                GameObject trainedUnit = Object.Instantiate(troop, pos, Quaternion.identity);
                currentTime = 0;
                timeToTraining = false;
                buttonController.trainUnitButton = false;
                panelController.TrainTimeVisibility(false);
                if (trainedUnit.CompareTag("Villager"))
                    trainedUnit.GetComponent<CollectResourcesController>().homePos = pos;
            }

        }

    }
}
