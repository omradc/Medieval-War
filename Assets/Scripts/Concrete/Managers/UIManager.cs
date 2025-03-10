﻿using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Concrete.Managers
{
    internal class UIManager : MonoBehaviour
    {
        public TMP_Dropdown formationDropdown;
        public TMP_Dropdown orderDropdown;
        public Toggle addUnitToggle;
        public Button clearUnits;
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
        public bool isClearUnits;
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
        }
        public void FormationDropdown()
        {
            switch (formationDropdown.value)
            {
                case 0:
                    KnightManager.Instance.troopFormation = KnightFormation.LineFormation;
                    break;
                case 1:
                    KnightManager.Instance.troopFormation = KnightFormation.RectangleFormation;
                    break;
                case 2:
                    KnightManager.Instance.troopFormation = KnightFormation.VFormation;
                    break;
                case 3:
                    KnightManager.Instance.troopFormation = KnightFormation.CurveFormation;
                    break;
                case 4:
                    KnightManager.Instance.troopFormation = KnightFormation.SingleLineFormation;
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
