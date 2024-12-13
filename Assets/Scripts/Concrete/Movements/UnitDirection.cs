using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Concrete.Movements
{
    internal class UnitDirection
    {
        UnitPathFinding2D pF2D;
        KnightController kC;
        float xPos;
        float yPos;


        Vector3 rightDirection = new Vector3(1, 1, 1);
        Vector3 leftDirection = new Vector3(-1, 1, 1);
        public UnitDirection(UnitPathFinding2D pF2D, KnightController kC)
        {
            this.pF2D = pF2D;
            this.kC = kC;
        }
        void SetBooleans()
        {
            pF2D.right = false;
            pF2D.left = false;
            pF2D.up = false;
            pF2D.down = false;
            pF2D.upRight = false;
            pF2D.upLeft = false;
            pF2D.downRight = false;
            pF2D.downLeft = false;
        }
        public int Turn2Direction(float lookXPos)
        {
            if (pF2D.transform.position.x > lookXPos)
            {
                pF2D.transform.localScale = leftDirection;
                SetBooleans();
                pF2D.left = true;

            }
            if (pF2D.transform.position.x < lookXPos)
            {
                pF2D.transform.localScale = rightDirection;

                if (kC.unitTypeEnum == UnitTypeEnum.Worrior)
                    kC.attackPoint.localPosition = Vector3.right * kC.attackPointDistance;

                SetBooleans();
                pF2D.right = true;
            }
            if (pF2D.left)
                return 1;
            return -1;
        }
        public void Turn4Direction(Vector2 target)
        {
            Vector2 direction = target - (Vector2)pF2D.transform.position;

            // Açı hesapla (radyan cinsinden)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Açıyı -180 ile 180 arasına çeker
            angle = (angle + 360) % 360;

            SetBooleans();

            //Left
            if (angle >= 135 && angle <= 225)
            {
                pF2D.left = true;
                pF2D.transform.localScale = leftDirection;
                // Savaşçının saldırı pozisyonunu belirler
                if (kC.unitTypeEnum == UnitTypeEnum.Worrior)
                    kC.attackPoint.localPosition = Vector3.right * kC.attackPointDistance;
            }

            //Up
            else if (angle >= 45 && angle <= 135)
            {
                pF2D.up = true;
                // Savaşçının saldırı pozisyonunu belirler
                if (kC.unitTypeEnum == UnitTypeEnum.Worrior)
                    kC.attackPoint.localPosition = Vector3.up * kC.attackPointDistance;
            }

            //Down
            else if (angle >= 225 && angle <= 315)
            {
                pF2D.down = true;
                // Savaşçının saldırı pozisyonunu belirler
                if (kC.unitTypeEnum == UnitTypeEnum.Worrior)
                    kC.attackPoint.localPosition = Vector3.down * kC.attackPointDistance;
            }

            //Right
            //if (angle < 45 && angle > 315) Impossible :D
            else
            {
                pF2D.right = true;
                pF2D.transform.localScale = rightDirection;
                // Savaşçının saldırı pozisyonunu belirler
                if (kC.unitTypeEnum == UnitTypeEnum.Worrior)
                    kC.attackPoint.localPosition = Vector3.right * kC.attackPointDistance;
            }
        }
        public void Turn8Direction(Vector2 target)
        {
            Vector2 direction = target - (Vector2)pF2D.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle = (angle + 360) % 360;

            SetBooleans();

            //UpRight
            if (angle > 30 && angle <= 60)
            {
                pF2D.upRight = true;
                pF2D.transform.localScale = rightDirection;
            }
            //Up
            else if (angle > 60 && angle <= 120)
            {
                pF2D.up = true;
            }
            //UpLeft
            else if (angle > 120 && angle <= 150)
            {
                pF2D.upLeft = true;
                pF2D.transform.localScale = leftDirection;
            }
            //Left
            else if (angle > 150 && angle <= 210)
            {
                pF2D.left = true;
                pF2D.transform.localScale = leftDirection;
            }
            //DownLeft
            else if (angle > 210 && angle <= 240)
            {
                pF2D.downLeft = true;
                pF2D.transform.localScale = leftDirection;
            }
            //Down
            else if (angle > 240 && angle <= 300)
            {
                pF2D.down = true;
            }
            //DownRight
            else if (angle > 300 && angle <= 330)
            {
                pF2D.downRight = true;
                pF2D.transform.localScale = rightDirection;
            }
            //Right
            else
            {
                pF2D.right = true;
                pF2D.transform.localScale = rightDirection;
            }
        }
    }
}
