using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class FenceController : MonoBehaviour
    {
        public GameObject open;
        public GameObject close;
        int villagerNumber;
        public void OpenTheDoor(bool isOpen)
        {
            if (isOpen)
            {
                open.SetActive(true);
                close.SetActive(false);
            }
            else
            {
                open.SetActive(false);
                close.SetActive(true);
            }
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Pawn"))
            {

                villagerNumber++;
                OpenTheDoor(true);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Pawn"))
            {
                villagerNumber--;
                if (villagerNumber == 0)
                    OpenTheDoor(false);
            }
        }

    }
}
