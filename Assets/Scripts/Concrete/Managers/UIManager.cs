using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Names;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


namespace Assets.Scripts.Concrete.Managers
{
    internal class UIManager : MonoBehaviour
    {
        public TMP_Dropdown formationDropdown;
        public TMP_Dropdown orderDropdown;
        public GameObject build;
        public GameObject buildingMovement;
        [HideInInspector] public bool canBuild;
        [HideInInspector] public GameObject previewObj;
        public bool canDragPreviewObj = true;
        public float gridSize = 0.1f;
        Vector3 up;
        Vector3 down;
        Vector3 right;
        Vector3 left;
        [HideInInspector] public GameObject valuePanel;
        public static UIManager Instance { get; private set; }
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

            formationDropdown.onValueChanged.AddListener(FormationDropdown);
            orderDropdown.onValueChanged.AddListener(OrderDropdown);
        }
        public void FormationDropdown(int index)
        {
            switch (index)
            {
                case 0:
                    KnightManager.Instance.troopFormation = KnightFormation.RectangleFormation;
                    break;
                case 1:
                    KnightManager.Instance.troopFormation = KnightFormation.HorizontalLineFormation;
                    break;
                case 2:
                    KnightManager.Instance.troopFormation = KnightFormation.VerticalLineFormation;
                    break;
                case 3:
                    KnightManager.Instance.troopFormation = KnightFormation.RightTriangleFormation;
                    break;
                case 4:
                    KnightManager.Instance.troopFormation = KnightFormation.LeftTriangleFormation;
                    break;
                case 5:
                    KnightManager.Instance.troopFormation = KnightFormation.UpTriangleFormation;
                    break;
                case 6:
                    KnightManager.Instance.troopFormation = KnightFormation.DownTriangleFormation;
                    break;
                case 7:
                    KnightManager.Instance.troopFormation = KnightFormation.RightCurveFormation;
                    break;
            }
        }
        public void OrderDropdown(int index)
        {
            switch (index)
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

        public void ValuePanel(GameObject valuePanel)
        {
            this.valuePanel = valuePanel;
        }

        public void PreviewBuilding(GameObject previewObj)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.previewObj = Instantiate(previewObj, pos, Quaternion.identity);
            //// Ön izleme objenin ValueController değerinin  ataması yapıldı
            //previewObj.GetComponent<BuildPreviewController>().valueController = valuePanel.GetComponent<ValueController>();

            BuildPanelVisibility(false);
            canDragPreviewObj = true;
        }
        public void BuildPanelVisibility(bool visiblity)
        {
            build.SetActive(visiblity);
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
    }
}
