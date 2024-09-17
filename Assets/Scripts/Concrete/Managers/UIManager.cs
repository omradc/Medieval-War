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
                    UnitManager.Instance.troopFormation = TroopFormation.RectangleFormation;
                    break;
                case 1:
                    UnitManager.Instance.troopFormation = TroopFormation.HorizontalLineFormation;
                    break;
                case 2:
                    UnitManager.Instance.troopFormation = TroopFormation.VerticalLineFormation;
                    break;
                case 3:
                    UnitManager.Instance.troopFormation = TroopFormation.RightTriangleFormation;
                    break;
                case 4:
                    UnitManager.Instance.troopFormation = TroopFormation.LeftTriangleFormation;
                    break;
                case 5:
                    UnitManager.Instance.troopFormation = TroopFormation.UpTriangleFormation;
                    break;
                case 6:
                    UnitManager.Instance.troopFormation = TroopFormation.DownTriangleFormation;
                    break;
                case 7:
                    UnitManager.Instance.troopFormation = TroopFormation.RightCurveFormation;
                    break;
            }
        }
        public void OrderDropdown(int index)
        {
            switch (index)
            {
                case 0:
                    UnitManager.Instance.unitOrderEnum = UnitOrderEnum.AttackOrder;
                    break;
                case 1:
                    UnitManager.Instance.unitOrderEnum = UnitOrderEnum.DefendOrder;
                    break;
                case 2:
                    UnitManager.Instance.unitOrderEnum = UnitOrderEnum.FollowOrder;
                    break;
                case 3:
                    UnitManager.Instance.unitOrderEnum = UnitOrderEnum.StayOrder;
                    break;
            }
        }
    }
}
