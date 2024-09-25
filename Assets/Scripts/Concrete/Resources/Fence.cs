using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class Fence : MonoBehaviour
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
            if (collision.gameObject.CompareTag("Villager"))
            {

                villagerNumber++;
                OpenTheDoor(true);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Villager"))
            {
                villagerNumber--;
                if (villagerNumber == 0)
                    OpenTheDoor(false);
            }
        }

    }
}
