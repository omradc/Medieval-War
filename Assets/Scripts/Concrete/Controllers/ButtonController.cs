using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class ButtonController : MonoBehaviour
    {

        [HideInInspector] public bool trainUnitButton;
        [HideInInspector] public bool upgrade;
        [HideInInspector] public GameObject upgrading;
        [HideInInspector] public GameObject upgradeComplete;
        PanelController panelController;
        private void Start()
        {
            panelController = GetComponent<PanelController>();

        }
        public void TrainUnitButton()
        {
            trainUnitButton = true;
            panelController.InteractablePanelVisibility(false);
            panelController.TimerPanelVisibility(true);
        }

        public void ConstructingBuildingButton(GameObject upgrading)
        {
            // Son Seviye
            if (upgrading == null) return;
            // Ev yükseltildiği anda yok edilir
            upgrade = true;
            this.upgrading = upgrading;
        }
        public void ConstructedBuilding(GameObject upgradeComplete)
        {
            this.upgradeComplete = upgradeComplete;
        }
        public void CloseButton()
        {
            panelController.InteractablePanelVisibility(false);
        }



    }
}
