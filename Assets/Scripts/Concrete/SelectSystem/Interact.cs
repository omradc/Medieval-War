using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using UnityEngine;

namespace Assets.Scripts.Concrete.SelectSystem
{
    internal class Interact
    {
        InteractManager ınteractManager;
        public Interact(InteractManager ınteractController)
        {
            this.ınteractManager = ınteractController;
        }

        public void InteractClickedObj()
        {
            // Ekran koordinatlarını dünya koordinatlarına çevir
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitObj = Physics2D.Raycast(mouseWorldPosition, Vector2.zero, 1);

            if (hitObj.collider != null)
            {
                ınteractManager.interactedObj = hitObj.collider.gameObject;
            }
        }
    }
}