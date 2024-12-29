using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Concrete.Movements
{
    internal class Direction
    {
        public bool right;
        public bool left;
        public bool up;
        public bool down;
        public bool upRight;
        public bool upLeft;
        public bool downRight;
        public bool downLeft;
        Transform transform;

        Vector3 rightDirection = new Vector3(1, 1, 1);
        Vector3 leftDirection = new Vector3(-1, 1, 1);
        public Direction(Transform transform)
        {
            this.transform = transform;
        }

        void SetBooleans()
        {
            right = false;
            left = false;
            up = false;
            down = false;
            upRight = false;
            upLeft = false;
            downRight = false;
            downLeft = false;
        }
        // Ajanın hız vektörüne göre yönü belirler
        public void Turn2DirectionWithVelocity(float velocity)
        {
            if (velocity < 0)
            {
                transform.localScale = leftDirection;
                SetBooleans();
                left = true;

            }
            if (velocity > 0)
            {
                transform.localScale = rightDirection;
                SetBooleans();
                right = true;
            }
        }
        public void Turn2DirectionWithPos(float xPos)
        {
            if (transform.position.x > xPos)
            {
                transform.localScale = leftDirection;
                SetBooleans();
                left = true;

            }
            if (transform.position.x < xPos)
            {
                transform.localScale = rightDirection;
                SetBooleans();
                right = true;
            }
        }
        public void Turn4Direction(Vector2 target)
        {
            Vector2 direction = target - (Vector2)transform.position;

            // Açı hesapla (radyan cinsinden)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Açıyı -180 ile 180 arasına çeker
            angle = (angle + 360) % 360;

            SetBooleans();

            //Left
            if (angle >= 135 && angle <= 225)
            {
                left = true;
                transform.localScale = leftDirection;
            }

            //Up
            else if (angle >= 45 && angle <= 135)
            {
                up = true;
            }

            //Down
            else if (angle >= 225 && angle <= 315)
            {
                down = true;
            }

            //Right
            //if (angle < 45 && angle > 315) Impossible :D
            else
            {
                right = true;
                transform.localScale = rightDirection;
            }
        }
        public void Turn8Direction(Vector2 target)
        {
            Vector2 direction = target - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle = (angle + 360) % 360;

            SetBooleans();

            //UpRight
            if (angle > 30 && angle <= 60)
            {
                upRight = true;
                transform.localScale = rightDirection;
            }
            //Up
            else if (angle > 60 && angle <= 120)
            {
                up = true;
            }
            //UpLeft
            else if (angle > 120 && angle <= 150)
            {
                upLeft = true;
                transform.localScale = leftDirection;
            }
            //Left
            else if (angle > 150 && angle <= 210)
            {
                left = true;
                transform.localScale = leftDirection;
            }
            //DownLeft
            else if (angle > 210 && angle <= 240)
            {
                downLeft = true;
                transform.localScale = leftDirection;
            }
            //Down
            else if (angle > 240 && angle <= 300)
            {
                down = true;
            }
            //DownRight
            else if (angle > 300 && angle <= 330)
            {
                downRight = true;
                transform.localScale = rightDirection;
            }
            //Right
            else
            {
                right = true;
                transform.localScale = rightDirection;
            }
        }
    }
}
