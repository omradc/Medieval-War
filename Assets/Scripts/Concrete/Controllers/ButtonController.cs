using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class ButtonController : MonoBehaviour
    {

        [HideInInspector] public bool trainUnitButton;
        PanelController panelController;
        private void Start()
        {
            panelController = GetComponent<PanelController>();

        }
        public void TrainUnitButton()
        {
            trainUnitButton = true;
            panelController.TrainUnitVisibility(false);
            panelController.TrainTimeVisibility(true);
        }

        public void CloseButton()
        {
            panelController.TrainUnitVisibility(false);

        }



    }
}
