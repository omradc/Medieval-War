using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Assets.Scripts.Concrete.Movements
{
    public class DynamicOrderInLayer
    {
        public int orderInLayer;

        /// <summary>
        ///  Use to Update, used transform.hasChanged, for 1 transform
        /// </summary>
        public void OrderInLayerUpdate(Transform transform, SpriteRenderer spriteRenderer, int order = 0)
        {
            if (spriteRenderer != null && transform.hasChanged)
            {
                spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 10) + order;
                transform.hasChanged = false;
            }
        }

        public void OrderInLayerUpdatePreview(Transform transform, SpriteRenderer spriteRenderer, int order = 0)
        {
            spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 10) + order;
        }

        /// <summary>
        ///  Use to Initialize
        /// </summary>
        public void OrderInLayerInitialize(Transform transform, SpriteRenderer spriteRenderer, int order = 0)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 10) + order;
            }
        }

        /// <summary>
        ///  Use to Initialize
        /// </summary>
        public void OrderInLayerInitialize(Transform transform, SpriteRenderer[] spriteRenderers, int order = 0)
        {
            if (spriteRenderers.Length == 0) return;
            int orderInLayer = Mathf.RoundToInt(-transform.position.y * 10);
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].sortingOrder = orderInLayer + order;
                if (order > 0)
                    order++;
                if (order < 0)
                    order--;
            }
        }

    }
}