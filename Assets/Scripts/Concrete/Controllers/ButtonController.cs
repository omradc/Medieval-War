using Assets.Scripts.Concrete.Managers;
using System.Resources;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class ButtonController : MonoBehaviour
    {

        [HideInInspector] public bool trainUnitButton;
        [HideInInspector] public bool upgrade;
        [HideInInspector] public GameObject construct;
        [HideInInspector] public GameObject upgradedBuilding;
        BuildingPanelController panelController;
        BuildingController buildingController;
        private void Start()
        {
            panelController = GetComponent<BuildingPanelController>();
            buildingController = GetComponent<BuildingController>();
        }
        public void TrainUnitButton()
        {
            if (ResourcesManager.Instance.Buy(KnightName()))
            {
                trainUnitButton = true;
                panelController.interactablePanel.SetActive(false);
                panelController.TimerPanelVisibility(true);
            }
        }

        //*********************************************************
        public void Construct(GameObject construct) // Upgrade: yükselecek yapının inşaatı oluşturur
        {
            if (ResourcesManager.Instance.Buy(BuildingName()))
            {
                // Son Seviye
                if (construct == null) return;
                // Ev yükseltildiği anda yok edilir
                upgrade = true;
                this.construct = construct;
            }
        }
        public void Upgrade(GameObject upgradedBuilding) // Upgrade: yükseltilen yapıyı oluşturur
        {
            this.upgradedBuilding = upgradedBuilding;
        }
        //*********************************************************
        public void CloseButton()
        {
            panelController.interactablePanel.SetActive(false);
            panelController.destructPanel.SetActive(false);
        }

        public void RebuildButton()
        {
            if (ResourcesManager.Instance.Buy(BuildingName()))
            {
                Destroy(gameObject);
                Instantiate(buildingController.construction, transform.position, Quaternion.identity);
            }
        }

        public void DestroyButton()
        {
            Destroy(gameObject);
        }
        string BuildingName()
        {
            if (gameObject.name == "PawnHouse_Blue(Clone)")
                return "pawnHouseLvl2";
            if (gameObject.name == "PawnHouse_Yellow(Clone)")
                return "pawnHouseLvl3";
            if (gameObject.name == "PawnHouse_Red(Clone)")
                return "pawnHouseLvl4";

            if (gameObject.name == "WarriorHouse_Blue(Clone)")
                return "warriorHouseLvl2";
            if (gameObject.name == "WarriorHouse_Yellow(Clone)")
                return "warriorHouseLvl3";
            if (gameObject.name == "WarriorHouse_Red(Clone)")
                return "warriorHouseLvl4";

            if (gameObject.name == "ArcherHouse_Blue(Clone)")
                return "archerHouseLvl2";
            if (gameObject.name == "ArcherHouse_Yellow(Clone)")
                return "archerHouseLvl3";
            if (gameObject.name == "ArcherHouse_Red(Clone)")
                return "archerHouseLvl4";

            if (gameObject.name == "Tower_Blue(Clone)")
                return "towerLvl2";
            if (gameObject.name == "Tower_Yellow(Clone)")
                return "towerLvl3";
            if (gameObject.name == "Tower_Red(Clone)")
                return "towerLvl4";

            if (gameObject.name == "Castle_Blue(Clone)")
                return "castleLvl2";
            if (gameObject.name == "Castle_Yellow(Clone)")
                return "castleLvl3";
            if (gameObject.name == "Castle_Red(Clone)")
                return "castleLvl4";

            if (gameObject.name == "Fence4x4(Clone)")
                return "fence4x4";

            else
                return "";
        }
        string KnightName()
        {
            if (gameObject.name == "PawnHouse_Blue(Clone)")
                return "pawnLvl1";
            if (gameObject.name == "PawnHouse_Yellow(Clone)")
                return "pawnLvl2";
            if (gameObject.name == "PawnHouse_Red(Clone)")
                return "pawnLvl3";
            if (gameObject.name == "PawnHouse_Purple(Clone)")
                return "pawnLvl4";

            if (gameObject.name == "WarriorHouse_Blue(Clone)")
                return "warriorLvl1";
            if (gameObject.name == "WarriorHouse_Yellow(Clone)")
                return "warriorLvl2";
            if (gameObject.name == "WarriorHouse_Red(Clone)")
                return "warriorLvl3";
            if (gameObject.name == "WarriorHouse_Purple(Clone)")
                return "warriorLvl4";

            if (gameObject.name == "ArcherHouse_Blue(Clone)")
                return "archerLvl1";
            if (gameObject.name == "ArcherHouse_Yellow(Clone)")
                return "archerLvl2";
            if (gameObject.name == "ArcherHouse_Red(Clone)")
                return "archerLvl3";
            if (gameObject.name == "ArcherHouse_Purple(Clone)")
                return "archerLvl4";

            else
                return "";
        }
    }
}
