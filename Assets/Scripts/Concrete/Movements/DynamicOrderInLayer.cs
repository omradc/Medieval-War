using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Assets.Scripts.Concrete.Movements
{
    public class DynamicOrderInLayer
    {
        public int orderInLayer;

        /// <summary>
        ///  Use to Update
        /// </summary>
        public void OrderInLayerWithYPos(Transform transform, SpriteRenderer spriteRenderer)
        {
            if (spriteRenderer != null && transform.hasChanged)
            {
                spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 10);
                transform.hasChanged = false;
            }
        }

        /// <summary>
        ///  Use to Initialize
        /// </summary>
        public void OrderInLayerWithNumber(Transform transform, SpriteRenderer spriteRenderer, int order = 0)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 10) + order;
            }
        }

        /// <summary>
        ///  Use to Initialize
        /// </summary>
        public void OrderInLayerWithYPos(Transform transform, SpriteRenderer[] spriteRenderers)
        {
            if (spriteRenderers.Length == 0) return;
            int orderInLayer = Mathf.RoundToInt(-transform.position.y * 10);
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].sortingOrder = orderInLayer;
            }
        }

    }
}