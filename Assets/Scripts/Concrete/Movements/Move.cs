using Assets.Scripts.Abstracts.Movements;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.SelectSystem;
using UnityEngine;


namespace Assets.Scripts.Concrete.Movements
{
    internal class Move : IMove
    {

        public void MoveCommand()
        {

            HorizontalLineFormation(KnightManager.Instance.troopFormation == TroopFormation.HorizontalLineFormation, KnightManager.Instance.distanceBetweenUnits);
            VerticalLineFormation(KnightManager.Instance.troopFormation == TroopFormation.VerticalLineFormation, KnightManager.Instance.distanceBetweenUnits);
            RectangleFormation(KnightManager.Instance.troopFormation == TroopFormation.RectangleFormation, KnightManager.Instance.distanceBetweenUnits);
            RightTriangleFormation(KnightManager.Instance.troopFormation == TroopFormation.RightTriangleFormation, KnightManager.Instance.distanceBetweenUnits);
            LeftTriangleFormation(KnightManager.Instance.troopFormation == TroopFormation.LeftTriangleFormation, KnightManager.Instance.distanceBetweenUnits);
            UpTriangleFormation(KnightManager.Instance.troopFormation == TroopFormation.UpTriangleFormation, KnightManager.Instance.distanceBetweenUnits);
            DownTriangleFormation(KnightManager.Instance.troopFormation == TroopFormation.DownTriangleFormation, KnightManager.Instance.distanceBetweenUnits);
            RightCurveFormation(KnightManager.Instance.troopFormation == TroopFormation.RightCurveFormation, KnightManager.Instance.distanceBetweenUnits);
        }

        public void HorizontalLineFormation(bool horizontalLineFormation, float distance)
        {
            if (horizontalLineFormation)
            {
                float xPos = 0;
                UnitPathFinding2D pF2D = null;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
                {
                    GameObject selectedObj = InteractManager.Instance.selectedUnits[i];
                    if (i == 0)
                    {
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos);
                    }

                    if (i > 0)
                    {
                        xPos += distance;
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos + new Vector2(xPos, 0));
                    }
                }
            }
        }
        public void VerticalLineFormation(bool verticalLineFormation, float distance)
        {
            if (verticalLineFormation)
            {
                float yPos = 0;
                UnitPathFinding2D pF2D = null;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
                {
                    GameObject selectedObj = InteractManager.Instance.selectedUnits[i];
                    if (i == 0)
                    {
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos);
                    }

                    if (i > 0)
                    {
                        yPos += distance;
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos + new Vector2(0, yPos));
                    }

                }


            }
        }
        public void RectangleFormation(bool rectangleFormation, float distance)
        {
            if (rectangleFormation)
            {
                float xPos = 0;
                float yPos = 0;
                float xTroopNumber = Mathf.Ceil(Mathf.Sqrt(InteractManager.Instance.selectedUnits.Count));
                float firstXtroopNumber = xTroopNumber;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                UnitPathFinding2D pF2D = null;
                for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
                {
                    GameObject selectedObj = InteractManager.Instance.selectedUnits[i];

                    // Yatay sıraya ekle
                    if (i < xTroopNumber)
                    {
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos + new Vector2(xPos, yPos));
                        xPos += distance;
                    }
                    // satır ekle
                    if (i >= xTroopNumber)
                    {
                        xTroopNumber += firstXtroopNumber;
                        xPos = 0;
                        yPos -= distance;
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos + new Vector2(xPos, yPos));
                        xPos += distance;
                    }
                }
            }
        }
        public void RightTriangleFormation(bool rightTriangleFormation, float distance)
        {
            if (rightTriangleFormation)
            {
                float xPos = 0;
                float yPos = 0;
                bool reverse = false;
                UnitPathFinding2D pF2D = null;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


                for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
                {
                    GameObject selectedObj = InteractManager.Instance.selectedUnits[i];

                    if (i == 0)
                    {
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos + new Vector2(xPos, yPos));

                    }

                    else if (reverse)
                    {
                        xPos -= distance * distance;
                        yPos += distance;
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos + new Vector2(xPos, yPos));
                    }
                    else if (!reverse)
                    {
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos + new Vector2(xPos, -yPos));
                    }

                    reverse = !reverse;
                }
            }
        }
        public void LeftTriangleFormation(bool leftTriangleFormation, float distance)
        {
            if (leftTriangleFormation)
            {
                float xPos = 0;
                float yPos = 0;
                bool reverse = false;
                UnitPathFinding2D pF2D = null;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


                for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
                {
                    GameObject selectedObj = InteractManager.Instance.selectedUnits[i];

                    if (i == 0)
                    {
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos + new Vector2(xPos, yPos));
                    }

                    else if (reverse)
                    {
                        xPos += distance * distance;
                        yPos += distance;
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos + new Vector2(xPos, yPos));
                    }
                    else if (!reverse)
                    {
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos + new Vector2(xPos, -yPos));
                    }

                    reverse = !reverse;
                }
            }
        }
        public void UpTriangleFormation(bool upTriangleFormation, float distance)
        {
            if (upTriangleFormation)
            {
                float xPos = 0;
                float yPos = 0;
                bool reverse = false;
                UnitPathFinding2D pF2D = null;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


                for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
                {
                    GameObject selectedObj = InteractManager.Instance.selectedUnits[i];

                    if (i == 0)
                    {
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos + new Vector2(xPos, yPos));
                    }

                    else if (reverse)
                    {
                        xPos += distance;
                        yPos -= distance * distance;
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos + new Vector2(xPos, yPos));
                    }
                    else if (!reverse)
                    {
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos + new Vector2(-xPos, yPos));
                    }

                    reverse = !reverse;
                }
            }
        }
        public void DownTriangleFormation(bool downTriangleFormation, float distance)
        {
            if (downTriangleFormation)
            {
                float xPos = 0;
                float yPos = 0;
                bool reverse = false;

                UnitPathFinding2D pF2D = null;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
                {
                    GameObject selectedObj = InteractManager.Instance.selectedUnits[i];

                    if (i == 0)
                    {
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos + new Vector2(xPos, yPos));
                    }

                    else if (reverse)
                    {
                        xPos += distance;
                        yPos += distance * distance;
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos + new Vector2(xPos, yPos));
                    }
                    else if (!reverse)
                    {
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos + new Vector2(-xPos, yPos));
                    }

                    reverse = !reverse;
                }
            }
        }
        public void RightCurveFormation(bool rightCurveFormation, float distance)
        {
            if (rightCurveFormation)
            {
                float xPos = 0.5f;
                float yPos = 0;
                bool reverse = false;
                UnitPathFinding2D pF2D = null;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


                for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
                {
                    GameObject selectedObj = InteractManager.Instance.selectedUnits[i];

                    if (i == 0)
                    {
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos + new Vector2(xPos, yPos));
                    }

                    else if (reverse)
                    {

                        xPos *= distance;
                        yPos += distance;
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos + new Vector2(xPos, yPos));
                    }
                    else if (!reverse)
                    {
                        pF2D = selectedObj.GetComponent<UnitPathFinding2D>();
                        pF2D.GetMoveCommand(mousePos + new Vector2(xPos, -yPos));
                    }

                    reverse = !reverse;
                }
            }
        }
    }
}
