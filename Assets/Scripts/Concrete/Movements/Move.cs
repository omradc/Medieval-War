using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using JetBrains.Annotations;
using UnityEngine;


namespace Assets.Scripts.Concrete.Movements
{
    internal class Move
    {
        Vector2 leaderPos;
        GameObject leader;
        float leaderDistance;
        
        public void MoveCommand()
        {
            //HorizontalLineFormation(KnightManager.Instance.troopFormation == KnightFormation.HorizontalLineFormation, KnightManager.Instance.distanceBetweenUnits);
            //VerticalLineFormation(KnightManager.Instance.troopFormation == KnightFormation.VerticalLineFormation, KnightManager.Instance.distanceBetweenUnits);
            //RectangleFormation(KnightManager.Instance.troopFormation == KnightFormation.RectangleFormation, KnightManager.Instance.distanceBetweenUnits);
            //RightTriangleFormation(KnightManager.Instance.troopFormation == KnightFormation.RightTriangleFormation, KnightManager.Instance.distanceBetweenUnits);
            //LeftTriangleFormation(KnightManager.Instance.troopFormation == KnightFormation.LeftTriangleFormation, KnightManager.Instance.distanceBetweenUnits);
            //UpTriangleFormation(KnightManager.Instance.troopFormation == KnightFormation.UpTriangleFormation, KnightManager.Instance.distanceBetweenUnits);
            //DownTriangleFormation(KnightManager.Instance.troopFormation == KnightFormation.DownTriangleFormation, KnightManager.Instance.distanceBetweenUnits);
            //RightCurveFormation(KnightManager.Instance.troopFormation == KnightFormation.RightCurveFormation, KnightManager.Instance.distanceBetweenUnits);
        }
        public void SendTheLeader()
        {
            if (InteractManager.Instance.selectedKnights.Count > 0)
            {
                Debug.Log("Send Leader");
                leader = InteractManager.Instance.selectedKnights[0];
                PathFinding pF = leader.GetComponent<PathFinding>();
                leaderPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Hedef konum
                pF.Move(leaderPos, 0);
            }
        }
        public void DynamicStayLineFormation(float distance)
        {
            int knightCount = InteractManager.Instance.selectedKnights.Count;
            if (knightCount > 1)
            {
                Debug.Log("Dynamic Stay Line Formation");
                for (int i = 1; i < knightCount; i++)
                {
                    GameObject knight = InteractManager.Instance.selectedKnights[i];
                    PathFinding pF = knight.GetComponent<PathFinding>();

                    // Önceki birimin hedef konumu
                    Vector2 previousTarget = InteractManager.Instance.selectedKnights[i - 1].transform.position;
                    // Çizgi formasyonu için yeni hedef pozisyonu hesapla
                    Vector2 targetPos = previousTarget + new Vector2(distance, 0);

                    if ((previousTarget - (Vector2)knight.transform.position).magnitude > distance) // Hedefle arasındaki mesafe açılırsa hareket et
                        pF.Move(targetPos, 0);
                }
            }
        }
        public void StayLineFormation(float distance)
        {
            float xPos = 0;
            int knightCount = InteractManager.Instance.selectedKnights.Count;
            if (knightCount > 1)
            {
                Debug.Log("Stay Line Formation");
                for (int i = 1; i < knightCount; i++)
                {
                    GameObject knight = InteractManager.Instance.selectedKnights[i];
                    PathFinding pF = knight.GetComponent<PathFinding>();

                    xPos += distance;
                    // Çizgi formasyonu için yeni hedef pozisyonu hesapla
                    Vector2 targetPos = leaderPos + new Vector2(xPos, 0);
                    pF.Move(targetPos, 0);
                }
            }
        }
        public bool LeaderReachTheTarget()
        {
            if (leader != null)
            {
                leaderDistance = ((Vector2)leader.transform.position - leaderPos).magnitude;
                if (leaderDistance < 0.1f)
                    leaderDistance = 0;
            }

            if (leaderDistance == 0f)
                return true;
            else
                return false;

        }
        //public void StayFormation(float distance)
        //{
        //    Debug.Log("Stay Formation");

        //    if (InteractManager.Instance.selectedKnights.Count == 0)
        //        return; // Eğer hiç birlik seçili değilse çık.

        //    PathFinding pF;
        //    Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //    for (int i = 0; i < InteractManager.Instance.selectedKnights.Count; i++)
        //    {
        //        GameObject selectedKnight = InteractManager.Instance.selectedKnights[i];
        //        pF = selectedKnight.GetComponent<PathFinding>();

        //        if (i == 0) // Lider birim hedef pozisyona gider.
        //        {
        //            pF.Move(targetPosition, 0);
        //        }
        //        else // Diğerleri bir önceki birimi takip eder.
        //        {
        //            GameObject previousKnight = InteractManager.Instance.selectedKnights[i - 1]; // Önceki birimi al
        //            Vector2 followPosition = previousKnight.transform.position /*- (previousKnight.transform.up * distance)*/; // Önündeki birimin arkasına geç

        //            pF.Move(followPosition, distance);
        //        }
        //    }
        //}


        //public void HorizontalLineFormation(bool horizontalLineFormation, float distance)
        //{
        //    if (horizontalLineFormation)
        //    {
        //        float xPos = 0;
        //        PathFinding pF;
        //        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        for (int i = 0; i < InteractManager.Instance.selectedKnights.Count; i++)
        //        {
        //            GameObject selectedObj = InteractManager.Instance.selectedKnights[i];
        //            if (i == 0)
        //            {

        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos, 0);
        //            }

        //            if (i > 0)
        //            {
        //                xPos += distance;
        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos + new Vector2(xPos, 0), 0);
        //            }
        //        }
        //    }
        //}
        //public void VerticalLineFormation(bool verticalLineFormation, float distance)
        //{
        //    if (verticalLineFormation)
        //    {
        //        float yPos = 0;
        //        PathFinding pF;
        //        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
        //        {
        //            GameObject selectedObj = InteractManager.Instance.selectedUnits[i];
        //            if (i == 0)
        //            {
        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos, 0);
        //            }

        //            if (i > 0)
        //            {
        //                yPos += distance;
        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos + new Vector2(0, yPos), 0);
        //            }

        //        }


        //    }
        //}
        //public void RectangleFormation(bool rectangleFormation, float distance)
        //{
        //    if (rectangleFormation)
        //    {
        //        float xPos = 0;
        //        float yPos = 0;
        //        float xTroopNumber = Mathf.Ceil(Mathf.Sqrt(InteractManager.Instance.selectedUnits.Count));
        //        float firstXtroopNumber = xTroopNumber;
        //        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        PathFinding pF;
        //        for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
        //        {
        //            GameObject selectedObj = InteractManager.Instance.selectedUnits[i];

        //            // Yatay sıraya ekle
        //            if (i < xTroopNumber)
        //            {
        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos + new Vector2(xPos, yPos), 0);
        //                xPos += distance;
        //            }
        //            // satır ekle
        //            if (i >= xTroopNumber)
        //            {
        //                xTroopNumber += firstXtroopNumber;
        //                xPos = 0;
        //                yPos -= distance;
        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos + new Vector2(xPos, yPos), 0);
        //                xPos += distance;
        //            }
        //        }
        //    }
        //}
        //public void RightTriangleFormation(bool rightTriangleFormation, float distance)
        //{
        //    if (rightTriangleFormation)
        //    {
        //        float xPos = 0;
        //        float yPos = 0;
        //        bool reverse = false;
        //        PathFinding pF;
        //        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        //        for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
        //        {
        //            GameObject selectedObj = InteractManager.Instance.selectedUnits[i];

        //            if (i == 0)
        //            {
        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos + new Vector2(xPos, yPos), 0);

        //            }

        //            else if (reverse)
        //            {
        //                xPos -= distance * distance;
        //                yPos += distance;
        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos + new Vector2(xPos, yPos), 0);
        //            }
        //            else if (!reverse)
        //            {
        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos + new Vector2(xPos, -yPos), 0);
        //            }

        //            reverse = !reverse;
        //        }
        //    }
        //}
        //public void LeftTriangleFormation(bool leftTriangleFormation, float distance)
        //{
        //    if (leftTriangleFormation)
        //    {
        //        float xPos = 0;
        //        float yPos = 0;
        //        bool reverse = false;
        //        PathFinding pF;
        //        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        //        for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
        //        {
        //            GameObject selectedObj = InteractManager.Instance.selectedUnits[i];

        //            if (i == 0)
        //            {
        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos + new Vector2(xPos, yPos), 0);

        //            }

        //            else if (reverse)
        //            {
        //                xPos += distance * distance;
        //                yPos += distance;
        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos + new Vector2(xPos, yPos), 0);

        //            }
        //            else if (!reverse)
        //            {

        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos + new Vector2(xPos, -yPos), 0);

        //            }

        //            reverse = !reverse;
        //        }
        //    }
        //}
        //public void UpTriangleFormation(bool upTriangleFormation, float distance)
        //{
        //    if (upTriangleFormation)
        //    {
        //        float xPos = 0;
        //        float yPos = 0;
        //        bool reverse = false;
        //        PathFinding pF;
        //        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        //        for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
        //        {
        //            GameObject selectedObj = InteractManager.Instance.selectedUnits[i];

        //            if (i == 0)
        //            {
        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos + new Vector2(xPos, yPos), 0);
        //            }

        //            else if (reverse)
        //            {
        //                xPos += distance;
        //                yPos -= distance * distance;
        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos + new Vector2(xPos, yPos), 0);
        //            }
        //            else if (!reverse)
        //            {
        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos + new Vector2(-xPos, yPos), 0);
        //            }

        //            reverse = !reverse;
        //        }
        //    }
        //}
        //public void DownTriangleFormation(bool downTriangleFormation, float distance)
        //{
        //    if (downTriangleFormation)
        //    {
        //        float xPos = 0;
        //        float yPos = 0;
        //        bool reverse = false;


        //        PathFinding pF;
        //        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //        for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
        //        {
        //            GameObject selectedObj = InteractManager.Instance.selectedUnits[i];

        //            if (i == 0)
        //            {
        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos + new Vector2(xPos, yPos), 0);
        //            }

        //            else if (reverse)
        //            {
        //                xPos += distance;
        //                yPos += distance * distance;
        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos + new Vector2(xPos, yPos), 0);
        //            }
        //            else if (!reverse)
        //            {
        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos + new Vector2(-xPos, yPos), 0);
        //            }

        //            reverse = !reverse;
        //        }
        //    }
        //}
        //public void RightCurveFormation(bool rightCurveFormation, float distance)
        //{
        //    if (rightCurveFormation)
        //    {
        //        float xPos = 0.5f;
        //        float yPos = 0;
        //        bool reverse = false;
        //        PathFinding pF;
        //        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        //        for (int i = 0; i < InteractManager.Instance.selectedUnits.Count; i++)
        //        {
        //            GameObject selectedObj = InteractManager.Instance.selectedUnits[i];

        //            if (i == 0)
        //            {
        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos + new Vector2(xPos, yPos), 0);
        //            }

        //            else if (reverse)
        //            {

        //                xPos *= distance;
        //                yPos += distance;
        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos + new Vector2(xPos, yPos), 0);
        //            }
        //            else if (!reverse)
        //            {
        //                pF = selectedObj.GetComponent<PathFinding>();
        //                pF.Move(mousePos + new Vector2(xPos, -yPos), 0);
        //            }

        //            reverse = !reverse;
        //        }
        //    }
        //}
    }
}
