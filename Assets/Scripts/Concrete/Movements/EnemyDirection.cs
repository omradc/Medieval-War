using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Concrete.Movements
{
    internal class EnemyDirection
    {
        EnemyPathFinding2D ePF2D;
        GoblinController gC;
        Vector3 rightDirection = new Vector3(1, 1, 1);
        Vector3 leftDirection = new Vector3(-1, 1, 1);
        public EnemyDirection(EnemyPathFinding2D ePF2D, GoblinController gC)
        {
            this.ePF2D = ePF2D;
            this.gC = gC;
        }
        void SetBooleans()
        {
            ePF2D.right = false;
            ePF2D.left = false;
            ePF2D.up = false;
            ePF2D.down = false;
            ePF2D.upRight = false;
            ePF2D.upLeft = false;
            ePF2D.downRight = false;
            ePF2D.downLeft = false;
        }
        public void Turn2Direction(float lookXPos)
        {
            if (ePF2D.transform.position.x > lookXPos)
            {
                ePF2D.transform.localScale = leftDirection;
                SetBooleans();
                ePF2D.left = true;

            }
            if (ePF2D.transform.position.x < lookXPos)
            {
                ePF2D.transform.localScale = rightDirection;

                if (gC.enemyTypeEnum == TroopTypeEnum.Torch)
                    gC.torchAttackPoint.localPosition = Vector3.right * gC.torchAttackPointDistance;

                SetBooleans();
                ePF2D.right = true;
            }
        }
        public void Turn4Direction(Vector2 target)
        {
            Vector2 direction = target - (Vector2)ePF2D.transform.position;

            // Açı hesapla (radyan cinsinden)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Açıyı -180 ile 180 arasına çeker
            angle = (angle + 360) % 360;

            SetBooleans();

            //Left
            if (angle >= 135 && angle <= 225)
            {
                ePF2D.left = true;
                ePF2D.transform.localScale = leftDirection;
                // Meşalenin saldırı pozisyonunu belirler
                if (gC.enemyTypeEnum == TroopTypeEnum.Torch)
                    gC.torchAttackPoint.localPosition = Vector3.right * gC.torchAttackPointDistance;
            }

            //Up
            else if (angle >= 45 && angle <= 135)
            {
                ePF2D.up = true;
                // Meşalenin saldırı pozisyonunu belirler
                if (gC.enemyTypeEnum == TroopTypeEnum.Torch)
                    gC.torchAttackPoint.localPosition = Vector3.up * gC.torchAttackPointDistance;
            }

            //Down
            else if (angle >= 225 && angle <= 315)
            {
                ePF2D.down = true;
                // Meşalenin saldırı pozisyonunu belirler
                if (gC.enemyTypeEnum == TroopTypeEnum.Torch)
                    gC.torchAttackPoint.localPosition = Vector3.down * gC.torchAttackPointDistance;
            }

            //Right
            //if (angle < 45 && angle > 315) Impossible :D
            else
            {
                ePF2D.right = true;
                ePF2D.transform.localScale = rightDirection;
                // Meşalenin saldırı pozisyonunu belirler
                if (gC.enemyTypeEnum == TroopTypeEnum.Torch)
                    gC.torchAttackPoint.localPosition = Vector3.right * gC.torchAttackPointDistance;
            }
        }
        public void Turn8Direction(Vector2 target)
        {
            Vector2 direction = target - (Vector2)ePF2D.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle = (angle + 360) % 360;

            SetBooleans();

            //UpRight
            if (angle > 30 && angle <= 60)
            {
                ePF2D.upRight = true;
                ePF2D.transform.localScale = rightDirection;
            }
            //Up
            else if (angle > 60 && angle <= 120)
            {
                ePF2D.up = true;
            }
            //UpLeft
            else if (angle > 120 && angle <= 150)
            {
                ePF2D.upLeft = true;
                ePF2D.transform.localScale = leftDirection;
            }
            //Left
            else if (angle > 150 && angle <= 210)
            {
                ePF2D.left = true;
                ePF2D.transform.localScale = leftDirection;
            }
            //DownLeft
            else if (angle > 210 && angle <= 240)
            {
                ePF2D.downLeft = true;
                ePF2D.transform.localScale = leftDirection;
            }
            //Down
            else if (angle > 240 && angle <= 300)
            {
                ePF2D.down = true;
            }
            //DownRight
            else if (angle > 300 && angle <= 330)
            {
                ePF2D.downRight = true;
                ePF2D.transform.localScale = rightDirection;
            }
            //Right
            else
            {
                ePF2D.right = true;
                ePF2D.transform.localScale = rightDirection;
            }
        }
    }
}
