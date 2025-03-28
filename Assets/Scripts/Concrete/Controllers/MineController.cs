using Assets.Scripts.Concrete.CollectResource;
using Assets.Scripts.Concrete.Movements;
using Assets.Scripts.Concrete.Resources;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class MineController : MonoBehaviour
    {
        int villagerNumber;
        public int mineAmount;
        [SerializeField] public float currentMineAmount;
        [SerializeField] GameObject inactive;
        [SerializeField] GameObject active;
        [SerializeField] GameObject destroyed;
        [SerializeField] GameObject minePanel;
        [SerializeField] GameObject navmeshObstacle;
        CircleCollider2D coll;
        public List<GameObject> villagers;
        public Image mineAmountFillValue;
        CollectResourceController cRC;
        DynamicOrderInLayer dynamicOrderInLayer;
        [SerializeField] Transform orderInLayerSpriteAnchor;


        private void Awake()
        {
            dynamicOrderInLayer = new();
            coll = GetComponent<CircleCollider2D>();
            dynamicOrderInLayer.OrderInLayerInitialize(orderInLayerSpriteAnchor, inactive.GetComponent<SpriteRenderer>());
            dynamicOrderInLayer.OrderInLayerInitialize(orderInLayerSpriteAnchor, active.GetComponent<SpriteRenderer>());
            dynamicOrderInLayer.OrderInLayerInitialize(orderInLayerSpriteAnchor, destroyed.GetComponent<SpriteRenderer>());
        }
        private void Start()
        {
            currentMineAmount = mineAmount;
        }
        void CloseAllSprites()
        {
            active.SetActive(false);
            inactive.SetActive(false);
            destroyed.SetActive(false);
        }
        public void Activated()
        {
            CloseAllSprites();
            active.SetActive(true);
        }
        public void Inactivated()
        {
            CloseAllSprites();
            inactive.SetActive(true);

        }
        public void Destroyed()
        {
            CloseAllSprites();
            destroyed.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Pawn"))
            {
                if (currentMineAmount <= 0) return;

                if (!villagers.Contains(collision.gameObject))
                    villagers.Add(collision.gameObject);
                villagerNumber++;

                if (villagerNumber > 0)
                    Activated();


            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Pawn"))
            {
                villagerNumber--;
                if (currentMineAmount <= 0) //Maden bittiyse
                {
                    Destroyed();
                    minePanel.SetActive(false);
                    coll.enabled = false;
                    navmeshObstacle.SetActive(false);
                    return;
                }
                if (villagerNumber <= 0) // Köylü kalmadıysa
                    Inactivated();
            }
        }
    }
}
