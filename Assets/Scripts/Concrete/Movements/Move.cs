﻿using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Managers;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Concrete.Movements
{
    internal class Move
    {
        DirectionEnum direction;
        public Move()
        {
            direction = new();
        }
        public void MoveCommand()
        {
            LineFormation(KnightManager.Instance.knightFormation == KnightFormation.LineFormation, KnightManager.Instance.distance);
            RectangleFormation(KnightManager.Instance.knightFormation == KnightFormation.RectangleFormation, KnightManager.Instance.distance);
            VFormation(KnightManager.Instance.knightFormation == KnightFormation.VFormation, KnightManager.Instance.distance);
            SingleLineFormation(KnightManager.Instance.knightFormation == KnightFormation.SingleLineFormation, KnightManager.Instance.distance);
            ArcFormation(KnightManager.Instance.knightFormation == KnightFormation.ArcFormation, KnightManager.Instance.distance);

            InteractManager.Instance.AddKnightModeStatus(); // seçim durumuna göre seçim dizisini hemen siler veya bekler
        }

        void CalculateDirections(Vector2 curentPos, Vector2 target)
        {
            Vector2 direction = target - curentPos;

            // Açı hesapla (radyan cinsinden)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Açıyı -180 ile 180 arasına çeker
            angle = (angle + 360) % 360;

            //Left
            if (angle >= 135 && angle <= 225)
                this.direction = DirectionEnum.left;
            //Up
            else if (angle >= 45 && angle <= 135)
                this.direction = DirectionEnum.up;

            //Down
            else if (angle >= 225 && angle <= 315)
                this.direction = DirectionEnum.down;

            //Right
            //if (angle < 45 && angle > 315) Impossible :D
            else
                this.direction = DirectionEnum.right;
        }
        public void LineFormation(bool lineFormation, float distance)
        {
            if (lineFormation)
            {
                int knightCount = InteractManager.Instance.selectedKnights.Count;
                if (knightCount == 0) return;

                PathFinding pF;
                float speed;
                Vector2 targetPos;
                float formationDiff = ((float)knightCount - 1) / 2 * distance; // 2f veya casting işlemi olmazsa "integer division" yüzünden ondalıklı kısım kayboluyor
                float xPos = 0;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (knightCount % 2 != 0)
                    CalculateDirections(InteractManager.Instance.selectedKnights[(knightCount - 1) / 2].transform.position, mousePos);
                else // tek ise: ortanca şovalye + distance /2
                    CalculateDirections((Vector2)InteractManager.Instance.selectedKnights[Mathf.FloorToInt(knightCount - 1) / 2].transform.position + new Vector2(distance / 2, 0), mousePos);

                if (direction == DirectionEnum.right || direction == DirectionEnum.left)
                    mousePos -= new Vector2(0, formationDiff);
                else
                    mousePos -= new Vector2(formationDiff, 0);

                if (knightCount > 1) // Seçili şovalye sayısı 1 den fazla ise en yavaş şovalye hızına ayarla
                    speed = CalculateSlowestKnight(InteractManager.Instance.selectedKnights);
                else // şovalye sayısı 1 ise kendi hızına ayarla
                    speed = InteractManager.Instance.selectedKnights[0].GetComponent<KnightController>().moveSpeed;
                for (int i = 0; i < knightCount; i++)
                {
                    GameObject knight = InteractManager.Instance.selectedKnights[i];
                    if (direction == DirectionEnum.right || direction == DirectionEnum.left)
                        targetPos = mousePos + new Vector2(0, xPos);
                    else
                        targetPos = mousePos + new Vector2(xPos, 0);
                    xPos += distance;
                    pF = knight.GetComponent<PathFinding>();
                    pF.agent.speed = speed;
                    pF.Move(targetPos, 0);
                }
            }
        }
        public void SingleLineFormation(bool SingleLineFormation, float distance)
        {
            if (SingleLineFormation)
            {
                int knightCount = InteractManager.Instance.selectedKnights.Count;
                if (knightCount == 0) return;

                PathFinding pF;
                float speed;
                Vector2 targetPos;
                float formationDiff = ((float)knightCount - 1) / 2 * distance; // 2f veya casting işlemi olmazsa "integer division" yüzünden ondalıklı kısım kayboluyor
                float xPos = 0;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (knightCount % 2 != 0)
                    CalculateDirections(InteractManager.Instance.selectedKnights[(knightCount - 1) / 2].transform.position, mousePos);
                else // tek ise: ortanca şovalye + distance /2
                    CalculateDirections((Vector2)InteractManager.Instance.selectedKnights[Mathf.FloorToInt(knightCount - 1) / 2].transform.position + new Vector2(distance / 2, 0), mousePos);

                if (direction == DirectionEnum.right || direction == DirectionEnum.left)
                    mousePos -= new Vector2(formationDiff, 0);
                else
                    mousePos -= new Vector2(0, formationDiff);

                if (knightCount > 1) // Seçili şovalye sayısı 1 den fazla ise en yavaş şovalye hızına ayarla
                    speed = CalculateSlowestKnight(InteractManager.Instance.selectedKnights);
                else // şovalye sayısı 1 ise kendi hızına ayarla
                    speed = InteractManager.Instance.selectedKnights[0].GetComponent<KnightController>().moveSpeed;

                for (int i = 0; i < knightCount; i++)
                {
                    GameObject knight = InteractManager.Instance.selectedKnights[i];

                    if (direction == DirectionEnum.right || direction == DirectionEnum.left)
                        targetPos = mousePos + new Vector2(xPos, 0);
                    else
                        targetPos = mousePos + new Vector2(0, xPos);
                    xPos += distance;
                    pF = knight.GetComponent<PathFinding>();
                    pF.agent.speed = speed;
                    pF.Move(targetPos, 0);
                }
            }
        }
        public void RectangleFormation(bool rectangleFormation, float distance)
        {
            if (rectangleFormation)
            {
                int knightCount = InteractManager.Instance.selectedKnights.Count;
                if (knightCount == 0) return;

                PathFinding pF;
                float speed;
                Vector2 targetPos;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int squareRoot = Mathf.CeilToInt(Mathf.Sqrt(InteractManager.Instance.selectedKnights.Count)); // En az kenar uzunluğu
                float side = (float)squareRoot / 2;
                float formationDiff = side * distance - 0.5f;
                mousePos -= new Vector2(formationDiff, formationDiff);
                if (knightCount > 1) // Seçili şovalye sayısı 1 den fazla ise en yavaş şovalye hızına ayarla
                    speed = CalculateSlowestKnight(InteractManager.Instance.selectedKnights);
                else // şovalye sayısı 1 ise kendi hızına ayarla
                    speed = InteractManager.Instance.selectedKnights[0].GetComponent<KnightController>().moveSpeed;

                for (int i = 0; i < InteractManager.Instance.selectedKnights.Count; i++)
                {
                    GameObject knight = InteractManager.Instance.selectedKnights[i];
                    int row = i / squareRoot; // Satır
                    int col = i % squareRoot; // Sütun

                    targetPos = mousePos + new Vector2(col * distance, row * distance);
                    pF = knight.GetComponent<PathFinding>();
                    pF.agent.speed = speed;
                    pF.Move(targetPos, 0);
                }
            }
        }
        public void VFormation(bool vFormation, float distance)
        {
            if (vFormation)
            {
                int knightCount = InteractManager.Instance.selectedKnights.Count;
                if (knightCount == 0) return;

                PathFinding pF;
                float speed;
                Vector2 targetPos = Vector2.zero;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CalculateDirections((Vector2)InteractManager.Instance.selectedKnights[0].transform.position, mousePos);
                float spacing = distance * 0.7f;

                if (knightCount > 1) // Seçili şovalye sayısı 1 den fazla ise en yavaş şovalye hızına ayarla
                    speed = CalculateSlowestKnight(InteractManager.Instance.selectedKnights);
                else // şovalye sayısı 1 ise kendi hızına ayarla
                    speed = InteractManager.Instance.selectedKnights[0].GetComponent<KnightController>().moveSpeed;

                for (int i = 0; i < InteractManager.Instance.selectedKnights.Count; i++)
                {
                    int row = (i + 1) / 2;  // Kaçıncı sırada olduğunu belirler +1 0 ve 1. index çakışmasını önler
                    float offset = (i % 2 == 0 ? -1 : 1) * row * spacing;  // Sağa ve sola yerleştirme

                    targetPos = mousePos;
                    if (direction == DirectionEnum.up)
                    {
                        targetPos.x += offset;
                        targetPos.y -= row * spacing;
                    }
                    else if (direction == DirectionEnum.down)
                    {
                        targetPos.x += offset;
                        targetPos.y += row * spacing;
                    }
                    else if (direction == DirectionEnum.left)
                    {
                        targetPos.x += row * spacing;
                        targetPos.y += offset;
                    }
                    else
                    {
                        targetPos.x -= row * spacing;
                        targetPos.y += offset;
                    }
                    pF = InteractManager.Instance.selectedKnights[i].GetComponent<PathFinding>();
                    pF.agent.speed = speed;
                    pF.Move(targetPos, 0);
                }
            }
        }
        public void ArcFormation(bool arcFormation, float distance)
        {
            if (!arcFormation) return;
            int knightCount = InteractManager.Instance.selectedKnights.Count;

            PathFinding pF;
            float speed;
            Vector2 targetPos;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Yayın merkezi
            if (knightCount < 2)
            {
                pF = InteractManager.Instance.selectedKnights[0].GetComponent<PathFinding>();
                pF.agent.speed = InteractManager.Instance.selectedKnights[0].GetComponent<KnightController>().moveSpeed;
                pF.Move(mousePos, 0);
                return;
            }
            float radius = knightCount / 2;
            float formationDiff = radius / 1.5f;
            float arcAngle = 120; // Açı
            float startAngle = 30; // Başlangıç açısı (Yayın sol ucu) // arcAngle 90 ise start angle 45
            float angleStep = arcAngle / (knightCount - 1); // Birimler arası açı farkı

            if (knightCount % 2 != 0)
                CalculateDirections(InteractManager.Instance.selectedKnights[(knightCount - 1) / 2].transform.position, mousePos);
            else // tek ise: ortanca şovalye + distance /2
                CalculateDirections((Vector2)InteractManager.Instance.selectedKnights[Mathf.FloorToInt(knightCount - 1) / 2].transform.position + new Vector2(distance / 2, 0), mousePos);

            if (knightCount > 1) // Seçili şovalye sayısı 1 den fazla ise en yavaş şovalye hızına ayarla
                speed = CalculateSlowestKnight(InteractManager.Instance.selectedKnights);
            else // şovalye sayısı 1 ise kendi hızına ayarla
                speed = InteractManager.Instance.selectedKnights[0].GetComponent<KnightController>().moveSpeed;

            for (int i = 0; i < knightCount; i++)
            {
                float angle = startAngle + i * angleStep; // Her birimin açısını hesapla
                float radians = angle * Mathf.Deg2Rad; // Dereceyi radyana çevir

                switch (direction)
                {
                    case DirectionEnum.up:
                        targetPos = new Vector2(mousePos.x + radius * Mathf.Cos(radians), mousePos.y - radius * Mathf.Sin(radians) + formationDiff);
                        break;
                    case DirectionEnum.down:
                        targetPos = new Vector2(mousePos.x + radius * Mathf.Cos(radians), mousePos.y + radius * Mathf.Sin(radians) - formationDiff);
                        break;
                    case DirectionEnum.left:
                        targetPos = new Vector2(mousePos.x + radius * Mathf.Sin(radians) - formationDiff, mousePos.y + radius * Mathf.Cos(radians));
                        break;
                    default:
                        targetPos = new Vector2(mousePos.x - radius * Mathf.Sin(radians) + formationDiff, mousePos.y + radius * Mathf.Cos(radians));
                        break;
                }
                pF = InteractManager.Instance.selectedKnights[i].GetComponent<PathFinding>();
                pF.agent.speed = speed;
                pF.Move(targetPos, 0);

            }
        }
        float CalculateSlowestKnight(List<GameObject> knights)
        {
            float slowestKnightSpeed = Mathf.Infinity;
            for (int i = 0; i < knights.Count; i++)
            {
                float currentKnightSpeed = knights[i].GetComponent<PathFinding>().agent.speed;
                if (slowestKnightSpeed > currentKnightSpeed)
                    slowestKnightSpeed = currentKnightSpeed;
            }
            return slowestKnightSpeed;
        }

        //public void SetSpeed()
        //{
        //    if (knightCount == 0) return;
        //    float totalRemainingDistance = 0;
        //    float distance = 0;
        //    for (int i = 0; i < knightCount; i++)
        //    {
        //        PathFinding pF = InteractManager.Instance.selectedKnights[i].GetComponent<PathFinding>();
        //        totalRemainingDistance += pF.agent.remainingDistance;
        //    }

        //    distance = totalRemainingDistance / knightCount;
        //    for (int i = 0; i < knightCount; i++)
        //    {
        //        PathFinding pF = InteractManager.Instance.selectedKnights[i].GetComponent<PathFinding>();
        //        Debug.Log("pF.agent.remainingDistance: " + pF.agent.remainingDistance + " > " + " distance: " + distance);
        //        if (pF.agent.remainingDistance > distance)
        //            pF.agent.speed = 2f;
        //        else
        //            pF.agent.speed = 1.5f;
        //    }
        //}
        #region Old

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
        #endregion
        #region DynamicFormation
        //Vector2 mousePos;
        //GameObject leader;
        //Vector2 selectedPos;
        //Vector2 tempSelectedPos;
        //Vector2 targetPos;
        //int knightCount;
        //int currentDir;
        //float leaderDistance;
        //public void LineFormation(float distance, bool sendLastPos) // Lider orta noktada
        //{
        //    Vector2 targetPos;
        //    int knightCount = InteractManager.Instance.selectedKnights.Count;
        //    float xPos = distance;
        //    if (knightCount > 0)
        //    {
        //        Debug.Log("Dynamic Stay Line Formation");
        //        for (int i = 0; i < knightCount; i++)
        //        {
        //            GameObject knight = InteractManager.Instance.selectedKnights[i];
        //            PathFinding pF = knight.GetComponent<PathFinding>();

        //            if (i == 0) // Leader
        //            {
        //                leader = knight;
        //                targetPos = mousePos;
        //            }
        //            else // Followers
        //            {
        //                if (!sendLastPos)
        //                {
        //                    Vector2 trackedKnightPos;
        //                    if (i == 1 || i == 2)
        //                        trackedKnightPos = InteractManager.Instance.selectedKnights[0].transform.position;
        //                    else
        //                        trackedKnightPos = InteractManager.Instance.selectedKnights[i - 2].transform.position;

        //                    targetPos = trackedKnightPos + new Vector2(SetDistanceForMoving(pF, i, distance, .5f) * CalcDirValue(i), 0); // Çizgi formasyonu için yeni hedef pozisyonu hesapla
        //                    SetSpeed(trackedKnightPos, knight, pF, distance);
        //                }
        //                else
        //                {
        //                    targetPos = mousePos + new Vector2(xPos * CalcDirValue(i), 0); // Çizgi formasyonu için yeni hedef pozisyonu hesapla
        //                    if (i % 2 == 0)
        //                        xPos += distance;
        //                }
        //            }
        //            pF.Move(targetPos, 0);
        //        }
        //    }
        //}
        //public void SetMousePos()
        //{
        //    if (InteractManager.Instance.selectedKnights.Count > 0)
        //        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Hedef konum
        //}
        //public void LineFormation(float distance, bool sendLastPos, List<GameObject> knights) // Lider başta
        //{
        //    knightCount = knights.Count;
        //    float xPos = distance;
        //    if (knightCount > 0)
        //    {
        //        Debug.Log("Line Formation");
        //        leader = knights[0];
        //        for (int i = 0; i < knightCount; i++)
        //        {
        //            GameObject knight = knights[i];
        //            KnightController knightController = knight.GetComponent<KnightController>();
        //            PathFinding pF = knight.GetComponent<PathFinding>();
        //            if (i == 0) // Leader
        //            {
        //                if (InteractManager.Instance.selectedKnights.Count > 0)
        //                    tempSelectedPos = mousePos;

        //                selectedPos = tempSelectedPos;
        //                targetPos = selectedPos;
        //                SetFormationDirection(CalculateDirections(leader.transform, selectedPos), knights);
        //            }
        //            else // Followers
        //            {
        //                if (!sendLastPos)
        //                {
        //                    Vector2 trackedKnightPos = knights[i - 1].transform.position;
        //                    //SetSpeed(trackedKnightPos, knight, pF, distance);
        //                    if (currentDir == 0)
        //                        targetPos = trackedKnightPos + new Vector2(xPos, 0);
        //                    else if (currentDir == 2)
        //                        targetPos = trackedKnightPos - new Vector2(xPos, 0);
        //                    else if (currentDir == 3)
        //                        targetPos = trackedKnightPos + new Vector2(0, xPos);
        //                    else
        //                        targetPos = trackedKnightPos - new Vector2(0, xPos);

        //                }
        //                else
        //                {
        //                    if (currentDir == 0)
        //                        targetPos = selectedPos + new Vector2(xPos, 0);
        //                    else if (currentDir == 2)
        //                        targetPos = selectedPos - new Vector2(xPos, 0);
        //                    else if (currentDir == 3)
        //                        targetPos = selectedPos + new Vector2(0, xPos);
        //                    else
        //                        targetPos = selectedPos - new Vector2(0, xPos);
        //                    xPos += distance;
        //                }
        //            }

        //            pF.Move(targetPos, 0);
        //        }
        //    }
        //}
        //void SetFormationDirection(int dirValue, List<GameObject> knights)
        //{
        //    if (currentDir != dirValue)
        //    {
        //        if (dirValue == 0 || dirValue == 2)
        //        {
        //            knights.Reverse();
        //            currentDir = dirValue;
        //        }
        //        if (dirValue == 1 || dirValue == 3)
        //        {
        //            knights.Reverse();
        //            currentDir = dirValue;
        //        }
        //    }
        //}
        //public void ClearTemp(ref bool workOnce, List<GameObject> knights)
        //{
        //    if (knightCount > 0 && leaderDistance < 0.1f)
        //    {
        //        Debug.Log("Clear selectMode");
        //        LineFormation(KnightManager.Instance.distance, true, knights); // Hafıza temizlenmeden son kez emir verir
        //        knights.Clear();
        //        workOnce = false;
        //    }
        //}
        //public bool LeaderReachTheTarget(float distance, List<GameObject> knights) // Liderin hedefe ulaşma kontrolü, dinamik formasyon çalışmayı durdurur ve sadece son bir kez konumlanacakları noktayı belirler. 
        //{
        //    if (knightCount > 0)
        //    {
        //        leaderDistance = ((Vector2)leader.transform.position - selectedPos).magnitude;
        //        if (leaderDistance < 0.1f)
        //        {
        //            if (!UIManager.Instance.addUnitToggle.isOn) // Tek tek seçim modu için, geçici hafızadan birimleri sil
        //            {
        //                Debug.Log("Clear noneSelectMode");
        //                LineFormation(distance, true, knights);
        //                knights.Clear();
        //            }
        //            return true;
        //        }
        //    }
        //    return false;

        //}
        //float SetDistanceForMoving(PathFinding pF, int i, float distance, float addDistance)
        //{
        //    bool isDouble = i % 2 == 0;
        //    int dir = CalculateDirections(leader.transform, mousePos);
        //    switch (dir)
        //    {
        //        case 0: //sol
        //            if (isDouble)
        //                return distance + addDistance;
        //            else
        //                return distance;

        //        case 1: //yukarı
        //            return distance;

        //        case 2: //sağ
        //            if (isDouble)
        //                return distance;
        //            else
        //                return distance + addDistance;

        //        default: //aşağı
        //            return distance;
        //    }
        //}
        #endregion

    }
}
