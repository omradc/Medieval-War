using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Names;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    class InteractableObjUIController : MonoBehaviour
    {

        public GameObject interactablePanel;
        public GameObject destructPanel;
        public float holdTreshold = .2f;

        public ValueController trainButton;
        public ValueController upgradeButton;
        public ValueController rebuildButton;
        [HideInInspector] public bool trainUnitButton;
        [HideInInspector] public bool upgrade;
        [HideInInspector] public GameObject construct;
        [HideInInspector] public GameObject upgradedBuilding;
        GameObject currentBuilding;
        BuildingController buildingController;
        [SerializeField] GameObject trainTimePanel;
        ObjNames names;
        float lastClickTime = 0f;
        float doubleClickThreshold = 0.3f;
        IInput ınput;
        float time;
        bool hold;
        private void Awake()
        {
            names = new(gameObject.name);
            ınput = new PcInput();
            buildingController = GetComponent<BuildingController>();
        }
        private void Update()
        {
            HoldTimer();
            if (ınput.GetButtonDown0() && interactablePanel.activeSelf) // Etkileşim paneli açıksa ve ekrana tıklandıysa
                if (!InteractManager.Instance.CheckUIElements()) // uı elemanı yoksa 
                    interactablePanel.SetActive(false); // etkileşim panelini kapat

        }
        public void TrainUnitButton(GameObject knightHouse)
        {
            KnightHouseController knightHouseController = knightHouse.GetComponent<KnightHouseController>();
            if (knightHouseController.currentTrainedKnightNumber < knightHouseController.maxKnightNumber)
            {
                if (ResourcesManager.Instance.Buy(names.KnightName(), trainButton))
                {
                    trainUnitButton = true;
                    interactablePanel.SetActive(false);
                    TimerPanelVisibility(true);
                }
            }
        }
        public void CurrentBuilding(GameObject currentBuilding)
        {
            this.currentBuilding = currentBuilding;
        }
        public void Construct(GameObject construct) // Upgrade: yükselecek yapının inşaatı oluşturur
        {
            if (ResourcesManager.Instance.Buy(names.BuildingName(), upgradeButton))
            {
                // Son Seviye
                if (construct == null) return;
                // Ev yükseltildiği anda yok edilir
                construct.GetComponent<ConstructController>().previousBuilding = currentBuilding;
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
            if (ResourcesManager.Instance.Buy(names.DestructedBuildingName(), rebuildButton))
            {
                Destroy(gameObject);
                Instantiate(buildingController.construction, transform.position, Quaternion.identity);
            }
        }
        public void DestroyButton()
        {
            if (gameObject.TryGetComponent(out KnightHouseController knightHouseController))
            {
                for (int i = 0; i < knightHouseController.knights.Count; i++)
                {
                    Destroy(knightHouseController.knights[i]); // Kullanıcı evi yok ederse birimler de yok edilir
                }
            }
            if (gameObject.TryGetComponent(out ConstructController constructController))
            {
                for (int i = 0; i < constructController.knights.Count; i++)
                {
                    Destroy(constructController.knights[i]); // Baraka yok edilirse birimler de yok edilir
                }
            }

            if (gameObject.CompareTag("Repo"))
                ResourcesManager.Instance.DestroyRepo(gameObject);
            Destroy(gameObject);

        }
        public void TimerPanelVisibility(bool visibility)
        {
            trainTimePanel.SetActive(visibility);
        }
        public void DoubleClickForPanelVisibility() //Event trigger bileşeni ile tetiklenir
        {
            if (Time.time - lastClickTime <= doubleClickThreshold)
            {
                PanelVisibility();
            }

            lastClickTime = Time.time; // Tıklama zamanını güncelle
        }
        public void HoldForPanelVisibility(bool value)
        {
            if (value)
                hold = true;
            else
                hold = false;
        }
        void PanelVisibility()
        {
            if (buildingController == null)
            {
                interactablePanel.SetActive(true);
                return;
            }

            if (!buildingController.destruct) // Bina yıkılmadıysa
            {
                if (buildingController.isFull || InteractManager.Instance.selectedKnights.Count > 0)  // üzerinde birim varsa, yıkıldıysa veya 1 birim seçili ise panelleri kapat
                    interactablePanel.SetActive(false);
                else
                    interactablePanel.SetActive(true);
            }

            if (buildingController.destruct) // Bina yıkıldıysa
            {
                if (InteractManager.Instance.selectedKnights.Count > 0) // 1 birim seçili ise panelleri kapat
                    destructPanel.SetActive(false);
                else if (gameObject.TryGetComponent(out ConstructController constructController))
                {
                    if (constructController.isConstructing)
                        destructPanel.SetActive(false);
                }
                else
                    destructPanel.SetActive(true);
            }
        }

        void HoldTimer()
        {
            if (hold)
            {
                time += Time.deltaTime;
                if (time >= holdTreshold)
                {
                    PanelVisibility();
                    time = 0;
                    hold = false;
                }
            }
            else
                time = 0;
        }
    }
}
