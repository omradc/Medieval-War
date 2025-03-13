using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Concrete.Managers
{
    internal class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        public Toggle addUnitToggle;
        public TMP_Dropdown formationDropdown;
        [SerializeField] TMP_Dropdown orderDropdown;
        [SerializeField] GameObject buildPanel;
        [SerializeField] GameObject buildingMovement;
        [SerializeField] GameObject savedFormation0;
        [SerializeField] GameObject savedFormation1;
        [SerializeField] GameObject savedFormation2;
        [SerializeField] GameObject savedFormation3;
        [HideInInspector] public bool canBuild;
        [HideInInspector] public GameObject previewObj;
        public bool canDragPreviewObj = true;
        public float gridSize = 0.1f;
        Vector3 up;
        Vector3 down;
        Vector3 right;
        Vector3 left;
        [HideInInspector] public GameObject valuePanel;
        public bool isClearUnits;
        public float time;
        public bool hold;
        float holdTreshold;
        GameObject currentSavedFormation;
        int index;
        private void Awake()
        {
            Singelton();
        }
        void Singelton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
                Destroy(this);
        }
        private void Start()
        {
            up = new Vector3(0, gridSize, 0);
            down = new Vector3(0, -gridSize, 0);
            right = new Vector3(gridSize, 0, 0);
            left = new Vector3(-gridSize, 0, 0);
            holdTreshold = .3f;
            InvokeRepeating(nameof(HoldForClearFormationTimer), 0, 0.1f);
        }
        public void FormationDropdown()
        {
            switch (formationDropdown.value)
            {
                case 0:
                    KnightManager.Instance.knightFormation = KnightFormation.RectangleFormation;
                    break;
                case 1:
                    KnightManager.Instance.knightFormation = KnightFormation.ArcFormation;
                    break;
                case 2:
                    KnightManager.Instance.knightFormation = KnightFormation.LineFormation;
                    break;
                case 3:
                    KnightManager.Instance.knightFormation = KnightFormation.VFormation;
                    break;
                case 4:
                    KnightManager.Instance.knightFormation = KnightFormation.SingleLineFormation;
                    break;
            }
        }
        public void OrderDropdown()
        {
            switch (orderDropdown.value)
            {
                case 0:
                    KnightManager.Instance.unitOrderEnum = KnightOrderEnum.AttackOrder;
                    break;
                case 1:
                    KnightManager.Instance.unitOrderEnum = KnightOrderEnum.DefendOrder;
                    break;
                case 2:
                    KnightManager.Instance.unitOrderEnum = KnightOrderEnum.FollowOrder;
                    break;
                case 3:
                    KnightManager.Instance.unitOrderEnum = KnightOrderEnum.StayOrder;
                    break;
            }
        }
        public void AddUnitToggle(bool on) { print(addUnitToggle.isOn); }
        public void ClearUnits()
        {
            isClearUnits = true;
        }
        public void IndexAssing(int index)
        {
            previewObj.GetComponent<BuildPreviewController>().index = index;
        }
        public void ValuePanel(GameObject valuePanel)
        {
            this.valuePanel = valuePanel;
        }
        public void PreviewBuilding(GameObject previewObj)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.previewObj = Instantiate(previewObj, pos, Quaternion.identity);
            BuildPanelVisibility(false);
            canDragPreviewObj = true;
        }
        public void BuildPanelVisibility(bool visiblity)
        {
            buildPanel.SetActive(visiblity);
            buildingMovement.SetActive(!visiblity);
        }
        public void Up()
        {
            Move(up);
        }
        public void Down()
        {
            Move(down);
        }
        public void Right()
        {
            Move(right);
        }
        public void Left()
        {
            Move(left);
        }
        void Move(Vector3 pos)
        {
            previewObj.transform.position += pos;
            previewObj.transform.position = new Vector3(TruncateToOneDecimal(previewObj.transform.position.x), TruncateToOneDecimal(previewObj.transform.position.y));
        }
        float TruncateToOneDecimal(float value)
        {
            return Mathf.Round(value * 10) / 10;
        }

        public void FormationIndex(int index)
        {
            this.index = index;
        }
        public void SaveFormation()
        {
            InteractManager.Instance.SaveFormation(index);
        }
        public void ClearSavedFormation(bool value)
        {
            if (value)
                hold = true;
            else
                hold = false;
        }
        void HoldForClearFormationTimer()
        {
            if (hold)
            {
                time += 0.1f;
                if (time >= holdTreshold)
                {
                    InteractManager.Instance.ClearSavedFormation(index);
                    time = 0;
                    hold = false;
                }
            }
            else
                time = 0;
        }
        public void UpdateFormationImage(bool allClose = false)
        {
            switch (index) // Hangi liste ile çalışıldığını bulur
            {
                case 0:
                    currentSavedFormation = savedFormation0;
                    break;
                case 1:
                    currentSavedFormation = savedFormation1;
                    break;
                case 2:
                    currentSavedFormation = savedFormation2;
                    break;
                case 3:
                    currentSavedFormation = savedFormation3;
                    break;
            }
            CloseAllFormationImage();
            if (allClose) return;
            switch (index)
            {
                case 0:
                    switch (KnightManager.Instance.knightFormation) // seçili listenin formasyon bilgisini gösterir
                    {
                        case KnightFormation.RectangleFormation:
                            currentSavedFormation.transform.GetChild(0).gameObject.SetActive(true);
                            break;
                        case KnightFormation.ArcFormation:
                            currentSavedFormation.transform.GetChild(1).gameObject.SetActive(true);
                            break;
                        case KnightFormation.LineFormation:
                            currentSavedFormation.transform.GetChild(2).gameObject.SetActive(true);
                            break;
                        case KnightFormation.VFormation:
                            currentSavedFormation.transform.GetChild(3).gameObject.SetActive(true);
                            break;
                        case KnightFormation.SingleLineFormation:
                            currentSavedFormation.transform.GetChild(4).gameObject.SetActive(true);
                            break;
                    }
                    break;
                case 1:
                    switch (KnightManager.Instance.knightFormation) // seçili listenin formasyon bilgisini gösterir
                    {
                        case KnightFormation.RectangleFormation:
                            currentSavedFormation.transform.GetChild(0).gameObject.SetActive(true);
                            break;
                        case KnightFormation.ArcFormation:
                            currentSavedFormation.transform.GetChild(1).gameObject.SetActive(true);
                            break;
                        case KnightFormation.LineFormation:
                            currentSavedFormation.transform.GetChild(2).gameObject.SetActive(true);
                            break;
                        case KnightFormation.VFormation:
                            currentSavedFormation.transform.GetChild(3).gameObject.SetActive(true);
                            break;
                        case KnightFormation.SingleLineFormation:
                            currentSavedFormation.transform.GetChild(4).gameObject.SetActive(true);
                            break;
                    }
                    break;
                case 2:
                    switch (KnightManager.Instance.knightFormation) // seçili listenin formasyon bilgisini gösterir
                    {
                        case KnightFormation.RectangleFormation:
                            currentSavedFormation.transform.GetChild(0).gameObject.SetActive(true);
                            break;
                        case KnightFormation.ArcFormation:
                            currentSavedFormation.transform.GetChild(1).gameObject.SetActive(true);
                            break;
                        case KnightFormation.LineFormation:
                            currentSavedFormation.transform.GetChild(2).gameObject.SetActive(true);
                            break;
                        case KnightFormation.VFormation:
                            currentSavedFormation.transform.GetChild(3).gameObject.SetActive(true);
                            break;
                        case KnightFormation.SingleLineFormation:
                            currentSavedFormation.transform.GetChild(4).gameObject.SetActive(true);
                            break;
                    }
                    break;
                case 3:
                    switch (KnightManager.Instance.knightFormation) // seçili listenin formasyon bilgisini gösterir
                    {
                        case KnightFormation.RectangleFormation:
                            currentSavedFormation.transform.GetChild(0).gameObject.SetActive(true);
                            break;
                        case KnightFormation.ArcFormation:
                            currentSavedFormation.transform.GetChild(1).gameObject.SetActive(true);
                            break;
                        case KnightFormation.LineFormation:
                            currentSavedFormation.transform.GetChild(2).gameObject.SetActive(true);
                            break;
                        case KnightFormation.VFormation:
                            currentSavedFormation.transform.GetChild(3).gameObject.SetActive(true);
                            break;
                        case KnightFormation.SingleLineFormation:
                            currentSavedFormation.transform.GetChild(4).gameObject.SetActive(true);
                            break;
                    }
                    break;
            }            
        }
        void CloseAllFormationImage()
        {
            currentSavedFormation.transform.GetChild(0).gameObject.SetActive(false);
            currentSavedFormation.transform.GetChild(1).gameObject.SetActive(false);
            currentSavedFormation.transform.GetChild(2).gameObject.SetActive(false);
            currentSavedFormation.transform.GetChild(3).gameObject.SetActive(false);
            currentSavedFormation.transform.GetChild(4).gameObject.SetActive(false);
        }

    }
}
