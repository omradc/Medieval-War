﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Concrete.Resources
{
    internal class Mine : MonoBehaviour
    {
        int villagerNumber;
        public int mineAmount;
        [HideInInspector] public float currentMineAmount;
        GameObject activated;
        GameObject inactivited;
        GameObject destroyed;
        GameObject minePanel;
        Collider2D[] colliders;
        public List<GameObject> villagers;
        public Image mineAmountFillValue;

        private void Start()
        {
            inactivited = transform.GetChild(0).gameObject;
            activated = transform.GetChild(1).gameObject;
            destroyed = transform.GetChild(2).gameObject;
            minePanel = transform.GetChild(3).gameObject;
            colliders = GetComponents<Collider2D>();
            currentMineAmount = mineAmount;
        }

        void CloseAllSprites()
        {
            activated.SetActive(false);
            inactivited.SetActive(false);
            destroyed.SetActive(false);
        }
        public void Activated()
        {
            CloseAllSprites();
            activated.SetActive(true);
        }
        public void Inactivated()
        {
            CloseAllSprites();
            inactivited.SetActive(true);

        }
        public void Destroyed()
        {
            CloseAllSprites();
            destroyed.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Villager"))
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
            if (collision.gameObject.CompareTag("Villager"))
            {
                villagerNumber--;

                //Maden bittiyse
                if (currentMineAmount <= 0)
                {
                    Destroyed();
                    minePanel.SetActive(false);
                    for (int i = 0; i < villagers.Count; i++)
                    {
                        villagers[i].GetComponent<CollectResources>().isMineEmpty = true;
                    }
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        Destroy(colliders[i]);
                    }
                    return;
                }

                // Köylü kalmadıysa
                if (villagerNumber == 0)
                    Inactivated();
            }
        }
    }
}