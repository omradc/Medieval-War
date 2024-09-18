using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Inputs;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class BarrackController : MonoBehaviour
    {
        [SerializeField] GameObject unit;
        public Vector3 trainedUnitPos;
        public Image timerFillImage;
        PanelController panelController;
        ButtonController buttonController;
        public float currentTime;
        public float trainingTime;
        Barrack barrack;
        IInput ıInput;
        
        private void Awake()
        {
            panelController = GetComponent<PanelController>();
            buttonController = GetComponent<ButtonController>();

        }
        private void Start()
        {
            ıInput = new PcInput();
            barrack = new Barrack(unit, transform.position + trainedUnitPos, panelController, buttonController, this);
        }
        private void Update()
        {           
            barrack.TrainUnit();
        }





    }
}
