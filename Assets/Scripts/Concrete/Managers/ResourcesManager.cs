using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Names;
using System.ComponentModel;
using TMPro;
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
        public Value[] value;

        [HideInInspector] public int totalGold;
        [HideInInspector] public int totalRock;
        [HideInInspector] public int totalWood;
        [HideInInspector] public int totalMeat;

        int index;
        bool isObjFound;
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
            totalGold = 99;
            totalRock = 99;
            totalWood = 99;
            totalMeat = 99;

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
        public bool Buy(string name, ValueController valueController)
        {

            // Satın alınan ürünün adını değer listesinde ara, bulunca index sırasını eşitle
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].name == name)
                {
                    isObjFound = true;
                    index = i;
                    break;
                }
                else
                    isObjFound = false;
            }

            if (!isObjFound)
            {
                print("name not found");
                return false;
            }

            Value v = value[index];

            if (totalGold >= v.goldPrice && totalRock >= v.rockPrice && totalWood >= v.woodPrice && totalMeat >= v.meatPrice)
            {
                totalGold -= v.goldPrice;
                totalRock -= v.rockPrice;
                totalWood -= v.woodPrice;
                totalMeat -= v.meatPrice;


                DisplayCost(valueController, v);
                DisplayPanelColor(valueController, v);

                print($"{v.name}:  gold: {v.goldPrice}, rock: {v.rockPrice}, wood: {v.woodPrice}, meat: {v.meatPrice}");
                DisplayResources();
                return true;
            }

            else
            {
                return false;
            }
        }

        void DisplayCost(ValueController valueController, Value v)
        {
            valueController.goldText.text = v.goldPrice.ToString();
            valueController.rockText.text = v.rockPrice.ToString();
            valueController.woodText.text = v.woodPrice.ToString();
            valueController.meatText.text = v.meatPrice.ToString();
        }

        void DisplayPanelColor(ValueController valueController, Value v)
        {
            if (totalGold >= v.goldPrice)
                valueController.goldPanel.color = Color.white;
            if (totalGold < v.goldPrice)
                valueController.goldPanel.color = Color.red;

            if (totalRock >= v.rockPrice)
                valueController.rockPanel.color = Color.white;
            if (totalRock < v.rockPrice)
                valueController.rockPanel.color = Color.red;

            if (totalWood >= v.woodPrice)
                valueController.woodPanel.color = Color.white;
            if (totalWood < v.woodPrice)
                valueController.woodPanel.color = Color.red;

            if (totalMeat >= v.meatPrice)
                valueController.meatPanel.color = Color.white;
            if (totalMeat < v.meatPrice)
                valueController.meatPanel.color = Color.red;
        }

        // Nesnelerin  text fiyatını göstermek için
        public void FirstItemValuesDisplay(string name, out int goldPrice,out int rockPrice, out int woodPrice, out int meatPrice)
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
