using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using UnityEngine;

namespace Assets.Scripts.Concrete.SelectSystem
{
    internal class Interact
    {
        InteractManager ınteractManager;
        LayerMask interactableLayers;
        public Interact(InteractManager ınteractManager, LayerMask interactableLayers)
        {
            this.ınteractManager = ınteractManager;
            this.interactableLayers = interactableLayers;
        }

        public void InteractClickedObj()
        {
            // Ekran koordinatlarını dünya koordinatlarına çevir
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitObj = Physics2D.Raycast(mouseWorldPosition, Vector2.zero, 1, interactableLayers);

            if (hitObj.collider != null)
                ınteractManager.interactedObj = hitObj.collider.gameObject;

        }
    }
}