using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Concrete.Managers
{
    internal class ResourcesManager : MonoBehaviour
    {
        public static ResourcesManager Instance;
        public Transform displayResourcesPanel;
        public int collectGoldAmount;
        public int collectRockAmount;
        public int collectWoodAmount;
        public int collectMeatAmount;


        public int goldPrice;
        public int rockPrice;
        public int woodPrice;
        public int meatPrice;
        public Value[] value;

        public int totalGold;
        public int totalRock;
        public int totalWood;
        public int totalMeat;

        [HideInInspector] public bool goldIsEnough;
        [HideInInspector] public bool rockIsEnough;
        [HideInInspector] public bool woodIsEnough;
        [HideInInspector] public bool meatIsEnough;


        int index;
        TextMeshProUGUI goldText;
        TextMeshProUGUI rockText;
        TextMeshProUGUI woodText;
        TextMeshProUGUI meatText;
        private void Awake()
        {
            Singelton();

            goldText = displayResourcesPanel.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            rockText = displayResourcesPanel.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            woodText = displayResourcesPanel.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
            meatText = displayResourcesPanel.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        }
        void Singelton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
                Destroy(this);
        }
        private void Start()
        {
            totalGold = 10;
            totalRock = 10;
            totalWood = 10;
            totalMeat = 10;
            
            DisplayResources();
        }
        public void DisplayResources()
        {
            goldText.text = totalGold.ToString();
            rockText.text = totalRock.ToString();
            woodText.text = totalWood.ToString();
            meatText.text = totalMeat.ToString();
        }

        // Satın alma işlemi
        public bool Buy(string name)
        {
            // Satın alınan ürünün adını değer listesinde ara, bulunca index sırasını eşitle
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].name == name)
                {
                    index = i;
                    break;
                }
            }

            Value v = value[index];

            if (totalGold >= v.goldPrice && totalRock >= v.rockPrice && totalWood >= v.woodPrice && totalMeat >= v.meatPrice)
            {
                totalGold -= v.goldPrice;
                totalRock -= v.rockPrice;
                totalWood -= v.woodPrice;
                totalMeat -= v.meatPrice;
                DisplayResources();
                return true;
            }

            else
            {
                return false;
            }
        }

        // Bir panelde nesnenin satın alınıp alınamayacağını kontrol eder
        public void CheckResources(string name)
        {
            // Satın alınan ürünün adını değer listesinde ara, bulunca index sırasını eşitle
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].name == name)
                {
                    index = i;
                    break;
                }
            }

            Value v = value[index];

            if (totalGold >= v.goldPrice)
                goldIsEnough = true;
            if (totalGold < v.goldPrice)
                goldIsEnough = false;

            if (totalRock >= v.rockPrice)
                rockIsEnough = true;
            if (totalRock < v.rockPrice)
                rockIsEnough = false;

            if (totalWood >= v.woodPrice)
                woodIsEnough = true;
            if (totalWood < v.woodPrice)
                woodIsEnough = false;

            if (totalMeat >= v.meatPrice)
                meatIsEnough = true;
            if (totalMeat < v.meatPrice)
                meatIsEnough = false;
        }

        // Nesnelerin  text fiyatını göstermek için
        public void ResourcesValues(string name)
        {
            // Satın alınan ürünün adını değer listesinde ara, bulunca index sırasını eşitle
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].name == name)
                {
                    index = i;
                    break;
                }
            }

            Value v = value[index];

            goldPrice = v.goldPrice;
            rockPrice = v.rockPrice;
            woodPrice = v.woodPrice;
            meatPrice = v.meatPrice;
        }
    }
}
