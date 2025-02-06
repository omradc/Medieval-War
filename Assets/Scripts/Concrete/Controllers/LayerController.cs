using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    public class LayerController : MonoBehaviour
    {
        public int layer;
        SpriteRenderer objSprite;
        SpriteRenderer collidedObjSprite;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Wall"))
            {
                objSprite = transform.parent.GetChild(0).GetComponent<SpriteRenderer>();
                collidedObjSprite = collision.transform.parent.GetChild(0).GetComponent<SpriteRenderer>();
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Wall"))
            {
                // Oyuncu engelin önünde 
                if (transform.position.y < collision.transform.position.y)
                {
                    objSprite.sortingOrder = collidedObjSprite.sortingOrder + 3;
                    Debug.Log("Önünde");
                }

                else
                {
                    objSprite.sortingOrder = collidedObjSprite.sortingOrder - 3;
                    Debug.Log("Arkasında");
                }
            }

        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Wall"))
            {
                objSprite.sortingOrder = layer;
            }
        }




    }
}