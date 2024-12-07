using Assets.Scripts.Concrete.Managers;
using System.Resources;
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


        //Upgrade Butonu
        public void ConstructingBuildingButton(GameObject upgrading)
        {
            if (ResourcesManager.Instance.Buy(BuildingName()))
            {
                // Son Seviye
                if (upgrading == null) return;
                // Ev yükseltildiği anda yok edilir
                upgrade = true;
                this.upgrading = upgrading;
            }

            else
            {
                Debug.Log("CANT UPGRADİNG");
            }
        }
        public void ConstructedBuilding(GameObject upgradeComplete)
        {
            this.upgradeComplete = upgradeComplete;
        }
        public void CloseButton()
        {
            panelController.InteractablePanelVisibility(false);
        }
        string BuildingName()
        {
            if (gameObject.name == "PawnHouse_Blue(Clone)")
                return "pawnHouseLvl2";
            if (gameObject.name == "PawnHouse_Yellow(Clone)")
                return "pawnHouseLvl3";
            if (gameObject.name == "PawnHouse_Red(Clone)")
                return "pawnHouseLvl4";

            if (gameObject.name == "WorriorHouse_Blue(Clone)")
                return "worriorHouseLvl2";
            if (gameObject.name == "WorriorHouse_Yellow(Clone)")
                return "worriorHouseLvl3";
            if (gameObject.name == "WorriorHouse_Red(Clone)")
                return "worriorHouseLvl4";

            if (gameObject.name == "ArcherHouse_Blue(Clone)")
                return "archerHouseLvl2";
            if (gameObject.name == "ArcherHouse_Yellow(Clone)")
                return "archerHouseLvl3";
            if (gameObject.name == "ArcherHouse_Red(Clone)")
                return "archerHouseLvl4";

            else
                return "";
        }


    }
}
