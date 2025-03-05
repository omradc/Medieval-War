using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class KnightHouseController : MonoBehaviour
    {
        [Header("House")]
        [SerializeField] GameObject knight;
        public int maxKnightNumber = 1;

        [Header("Setup")]
        public Image timerFillImage;
        public float trainingTime;
        public float currentTrainedKnightNumber;
        [HideInInspector] public List<GameObject> knights;
        Transform trainedUnitPos;
        InteractableObjUIController interactableObjUIController;
        float currentTime;
        private void Awake()
        {
            interactableObjUIController = GetComponent<InteractableObjUIController>();
            trainedUnitPos = transform.GetChild(0);
            knights = new();
        }
        private void Start()
        {
            InitilizeKnights();
        }
        private void Update()
        {
            TrainUnit();
        }

        public void TrainUnit()
        {

            if (interactableObjUIController.trainUnitButton)
            {
                currentTime += Time.deltaTime;
                timerFillImage.fillAmount = currentTime / trainingTime;
                if (currentTime >= trainingTime)
                {
                    GameObject trainedUnit = Instantiate(knight, trainedUnitPos.position, Quaternion.identity);
                    trainedUnit.GetComponent<HealthController>().knightHouseController = this;
                    knights.Add(trainedUnit);
                    currentTrainedKnightNumber++;
                    currentTime = 0;
                    interactableObjUIController.trainUnitButton = false;
                    interactableObjUIController.TimerPanelVisibility(false);
                }
            }

        }
        void InitilizeKnights()
        {
            for (int i = 0; i < knights.Count; i++)
            {
                GameObject newKnight = Instantiate(knight, knights[i].transform.position, Quaternion.identity);
                currentTrainedKnightNumber++;
                Destroy(knights[i]);
                knights[i] = newKnight;
            }
        }


    }
}
