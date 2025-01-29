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
        public bool canBuild;
        [HideInInspector] public GameObject previewObj;

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
            formationDropdown.onValueChanged.AddListener(FormationDropdown);
            orderDropdown.onValueChanged.AddListener(OrderDropdown);
        }
        public void FormationDropdown(int index)
        {
            switch (index)
            {
                case 0:
                    KnightManager.Instance.troopFormation = TroopFormation.RectangleFormation;
                    break;
                case 1:
                    KnightManager.Instance.troopFormation = TroopFormation.HorizontalLineFormation;
                    break;
                case 2:
                    KnightManager.Instance.troopFormation = TroopFormation.VerticalLineFormation;
                    break;
                case 3:
                    KnightManager.Instance.troopFormation = TroopFormation.RightTriangleFormation;
                    break;
                case 4:
                    KnightManager.Instance.troopFormation = TroopFormation.LeftTriangleFormation;
                    break;
                case 5:
                    KnightManager.Instance.troopFormation = TroopFormation.UpTriangleFormation;
                    break;
                case 6:
                    KnightManager.Instance.troopFormation = TroopFormation.DownTriangleFormation;
                    break;
                case 7:
                    KnightManager.Instance.troopFormation = TroopFormation.RightCurveFormation;
                    break;
            }
        }
        public void OrderDropdown(int index)
        {
            switch (index)
            {
                case 0:
                    KnightManager.Instance.unitOrderEnum = UnitOrderEnum.AttackOrder;
                    break;
                case 1:
                    KnightManager.Instance.unitOrderEnum = UnitOrderEnum.DefendOrder;
                    break;
                case 2:
                    KnightManager.Instance.unitOrderEnum = UnitOrderEnum.FollowOrder;
                    break;
                case 3:
                    KnightManager.Instance.unitOrderEnum = UnitOrderEnum.StayOrder;
                    break;
            }
        }
        public void PreviewBuilding(GameObject obj)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            previewObj = Instantiate(obj, pos, Quaternion.identity);
        }


    }
}
