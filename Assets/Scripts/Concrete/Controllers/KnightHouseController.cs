using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.KnightBuildings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class KnightHouseController : MonoBehaviour
    {
        [SerializeField] GameObject unit;
        public Vector3 trainedUnitPos;
        public Image timerFillImage;
        public float currentTime;
        PanelController panelController;
        ButtonController buttonController;
        public float trainingTime;
        public float upgradingTime;
        KnightHouse knightHouse;
        IInput ıInput;
        
        private void Awake()
        {
            panelController = GetComponent<PanelController>();
            buttonController = GetComponent<ButtonController>();

        }
        private void Start()
        {
            ıInput = new PcInput();
            knightHouse = new KnightHouse(unit, transform.position + trainedUnitPos, panelController, buttonController, this);
        }
        private void Update()
        {           
            knightHouse.TrainUnit();
            knightHouse.UpgradeHouse();
        }





    }
}
