using Assets.Scripts.Concrete.KnightBuildings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class KnightHouseController : MonoBehaviour
    {
        [SerializeField] GameObject unit;
        public Image timerFillImage;
        public float trainingTime;

        Transform trainedUnitPos;
        BuildingPanelController panelController;
        ButtonController buttonController;
        KnightHouse knightHouse;
        
        private void Awake()
        {
            panelController = GetComponent<BuildingPanelController>();
            buttonController = GetComponent<ButtonController>();
            trainedUnitPos = transform.GetChild(0);

        }
        private void Start()
        {
            knightHouse = new KnightHouse(unit, trainedUnitPos.position, panelController, buttonController, this);
        }
        private void Update()
        {           
            knightHouse.TrainUnit();
        }





    }
}
