using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Names;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    class InteractableObjUIController : MonoBehaviour
    {

        public GameObject interactablePanel;
        public GameObject destructPanel;

        public ValueController trainButton;
        public ValueController upgradeButton;
        public ValueController rebuildButton;
        [HideInInspector] public bool trainUnitButton;
        [HideInInspector] public bool upgrade;
        [HideInInspector] public GameObject construct;
        [HideInInspector] public GameObject upgradedBuilding;
        BuildingController buildingController;
        [SerializeField] GameObject trainTimePanel;
        ObjNames names;
        private void Awake()
        {
            names = new(gameObject.name);
            buildingController = GetComponent<BuildingController>();
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && interactablePanel.activeSelf) // Etkileşim paneli açıksa ve ekrana tıklandıysa
                if (!InteractManager.Instance.CheckUIElements()) // uı elemanı yoksa 
                    interactablePanel.SetActive(false); // etkileşim panelini kapat

        }
        public void TrainUnitButton()
        {
            if (ResourcesManager.Instance.Buy(names.KnightName(),trainButton))
            {
                trainUnitButton = true;
                interactablePanel.SetActive(false);
                TimerPanelVisibility(true);
            }
        }
        public void Construct(GameObject construct) // Upgrade: yükselecek yapının inşaatı oluşturur
        {
            if (ResourcesManager.Instance.Buy(names.BuildingName(),upgradeButton))
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
        public void RebuildButton()
        {
            if (ResourcesManager.Instance.Buy(names.DestructedBuildingName(),rebuildButton))
            {
                Destroy(gameObject);
                Instantiate(buildingController.construction, transform.position, Quaternion.identity);
            }
        }
        public void DestroyButton()
        {
            Destroy(gameObject);
        }

        public void PanelVisibility()  //Event trigger bileşeni ile tetiklenir
        {
            if (!buildingController.destruct) // Bina yıkılmadıysa
            {
                if (buildingController.isFull || InteractManager.Instance.selectedUnits.Count > 0)  // üzerinde birim varsa, yıkıldıysa veya 1 birim seçili ise panelleri kapat
                    interactablePanel.SetActive(false);
                else
                    interactablePanel.SetActive(true);
            }

            if (buildingController.destruct) // Bina yıkıldıysa
            {
                if (InteractManager.Instance.selectedUnits.Count > 0) // 1 birim seçili ise panelleri kapat
                    destructPanel.SetActive(false);
                else
                    destructPanel.SetActive(true);
            }
        }
        public void TimerPanelVisibility(bool visibility)
        {
            trainTimePanel.SetActive(visibility);
        }
    }
}
