using Assets.Scripts.Abstracts.Movements;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
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
                PathFindingController pF;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
                {
                    GameObject selectedObj = InteractManager.Instance.selectedUnits[i];
                    if (i == 0)
                    {

                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos, 0);
                    }

                    if (i > 0)
                    {
                        xPos += distance;
                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos + new Vector2(xPos, 0), 0);
                    }
                }
            }
        }
        public void VerticalLineFormation(bool verticalLineFormation, float distance)
        {
            if (verticalLineFormation)
            {
                float yPos = 0;
                PathFindingController pF;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
                {
                    GameObject selectedObj = InteractManager.Instance.selectedUnits[i];
                    if (i == 0)
                    {
                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos, 0);
                    }

                    if (i > 0)
                    {
                        yPos += distance;
                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos + new Vector2(0, yPos), 0);
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
                PathFindingController pF;
                for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
                {
                    GameObject selectedObj = InteractManager.Instance.selectedUnits[i];

                    // Yatay sıraya ekle
                    if (i < xTroopNumber)
                    {
                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos + new Vector2(xPos, yPos), 0);
                        xPos += distance;
                    }
                    // satır ekle
                    if (i >= xTroopNumber)
                    {
                        xTroopNumber += firstXtroopNumber;
                        xPos = 0;
                        yPos -= distance;
                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos + new Vector2(xPos, yPos), 0);
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
                PathFindingController pF;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


                for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
                {
                    GameObject selectedObj = InteractManager.Instance.selectedUnits[i];

                    if (i == 0)
                    {
                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos + new Vector2(xPos, yPos), 0);

                    }

                    else if (reverse)
                    {
                        xPos -= distance * distance;
                        yPos += distance;
                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos + new Vector2(xPos, yPos), 0);
                    }
                    else if (!reverse)
                    {
                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos + new Vector2(xPos, -yPos), 0);
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
                PathFindingController pF;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


                for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
                {
                    GameObject selectedObj = InteractManager.Instance.selectedUnits[i];

                    if (i == 0)
                    {
                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos + new Vector2(xPos, yPos), 0);

                    }

                    else if (reverse)
                    {
                        xPos += distance * distance;
                        yPos += distance;
                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos + new Vector2(xPos, yPos), 0);

                    }
                    else if (!reverse)
                    {

                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos + new Vector2(xPos, -yPos), 0);

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
                PathFindingController pF;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


                for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
                {
                    GameObject selectedObj = InteractManager.Instance.selectedUnits[i];

                    if (i == 0)
                    {
                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos + new Vector2(xPos, yPos), 0);
                    }

                    else if (reverse)
                    {
                        xPos += distance;
                        yPos -= distance * distance;
                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos + new Vector2(xPos, yPos), 0);
                    }
                    else if (!reverse)
                    {
                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos + new Vector2(-xPos, yPos), 0);
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


                PathFindingController pF;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
                {
                    GameObject selectedObj = InteractManager.Instance.selectedUnits[i];

                    if (i == 0)
                    {
                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos + new Vector2(xPos, yPos), 0);
                    }

                    else if (reverse)
                    {
                        xPos += distance;
                        yPos += distance * distance;
                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos + new Vector2(xPos, yPos), 0);
                    }
                    else if (!reverse)
                    {
                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos + new Vector2(-xPos, yPos), 0);
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
                PathFindingController pF;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


                for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
                {
                    GameObject selectedObj = InteractManager.Instance.selectedUnits[i];

                    if (i == 0)
                    {
                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos + new Vector2(xPos, yPos), 0);
                    }

                    else if (reverse)
                    {

                        xPos *= distance;
                        yPos += distance;
                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos + new Vector2(xPos, yPos), 0);
                    }
                    else if (!reverse)
                    {
                        pF = selectedObj.GetComponent<PathFindingController>();
                        pF.Move(mousePos + new Vector2(xPos, -yPos), 0);
                    }

                    reverse = !reverse;
                }
            }
        }
    }
}
