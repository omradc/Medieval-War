using Assets.Scripts.Concrete.Controllers;
using System.Collections.Generic;
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
        public Value[] value; // Çok pahalı bir dizi**************************

        public List<GameObject> repos;

        public int totalGold;
        public int totalRock;
        public int totalWood;
        public int totalMeat;

        int index;
        bool isObjFound;
        TextMeshProUGUI goldText;
        TextMeshProUGUI rockText;
        TextMeshProUGUI woodText;
        TextMeshProUGUI meatText;
        private void Awake()
        {
            Singelton();
            repos = new();
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
            totalGold = 8;
            totalRock = 9;
            totalWood = 9;
            totalMeat = 1;

            DisplayResources();
        }
        public void DisplayResources()
        {
            goldText.text = totalGold.ToString();
            rockText.text = totalRock.ToString();
            woodText.text = totalWood.ToString();
            meatText.text = totalMeat.ToString();
        }
        public bool Buy(string name, ValueController valueController) // Satın alma işlemi
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
            // Ürün bulunamadı
            if (!isObjFound)
            {
                print("name not found");
                return false;
            }

            Value v = value[index];
            //Elindeki kaynak ile maliyeti kıyasla
            if (totalGold >= v.goldCost && totalRock >= v.rockCost && totalWood >= v.woodCost && totalMeat >= v.meatCost)
            {
                totalGold -= v.goldCost;
                totalRock -= v.rockCost;
                totalWood -= v.woodCost;
                totalMeat -= v.meatCost;
                UnloadRepo(v.goldCost + v.rockCost + v.woodCost + v.meatCost);
                DisplayCost(valueController, v);
                DisplayPanelColor(valueController, v);

                print($"{v.name}:  gold: {v.goldCost}, rock: {v.rockCost}, wood: {v.woodCost}, meat: {v.meatCost}");
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
            valueController.goldText.text = v.goldCost.ToString();
            valueController.rockText.text = v.rockCost.ToString();
            valueController.woodText.text = v.woodCost.ToString();
            valueController.meatText.text = v.meatCost.ToString();
        }
        void DisplayPanelColor(ValueController valueController, Value v)
        {
            print("renk göster");
            if (totalGold >= v.goldCost)
                valueController.goldPanel.color = Color.white;
            if (totalGold < v.goldCost)
                valueController.goldPanel.color = Color.red;

            if (totalRock >= v.rockCost)
                valueController.rockPanel.color = Color.white;
            if (totalRock < v.rockCost)
                valueController.rockPanel.color = Color.red;

            if (totalWood >= v.woodCost)
                valueController.woodPanel.color = Color.white;
            if (totalWood < v.woodCost)
                valueController.woodPanel.color = Color.red;

            if (totalMeat >= v.meatCost)
                valueController.meatPanel.color = Color.white;
            if (totalMeat < v.meatCost)
                valueController.meatPanel.color = Color.red;
        }
        public void FirstItemValuesDisplay(string name, out int goldPrice, out int rockPrice, out int woodPrice, out int meatPrice)// Nesnelerin  text fiyatını göstermek için
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

            goldPrice = v.goldCost;
            rockPrice = v.rockCost;
            woodPrice = v.woodCost;
            meatPrice = v.meatCost;
        }
        void UnloadRepo(int totalCostResources)
        {
            while (totalCostResources > 0) // Maliyet 0 dan büyük olduğu sürece tüm depolar maiyeti karşılayana kadar, kapasitesinden düşer.
            {
                GameObject depo = FindFullDepo(); //En dolu depoyu bul
                int remainingCapacity = depo.GetComponent<RepoController>().currentRepoCapacity -= totalCostResources;


                if (remainingCapacity < 0) // Kapasite karşılamıyorsa
                {
                    depo.GetComponent<RepoController>().currentRepoCapacity -= remainingCapacity; // depodan alınan fazlalığı ekle
                    totalCostResources = -remainingCapacity; // yeni maliyet 
                }
                else // Kapasite karşılıyorsa döngüyü sonlandır
                    totalCostResources = 0;
            }
        }
        GameObject FindFullDepo()
        {
            GameObject fullestRepo = null;
            float repoFullness = 0;
            for (int i = 0; i < repos.Count; i++)
            {
                float currentRepoFullnes = repos[i].GetComponent<RepoController>().currentRepoCapacity;
                if (currentRepoFullnes > repoFullness)
                {
                    repoFullness = currentRepoFullnes;
                    fullestRepo = repos[i];
                }
            }
            return fullestRepo;
        }
        public void RemoveRepo(GameObject repo)
        {
            totalGold -= repo.GetComponent<RepoController>().gold;
            totalRock -= repo.GetComponent<RepoController>().rock;
            totalWood -= repo.GetComponent<RepoController>().wood;
            totalMeat -= repo.GetComponent<RepoController>().meat;
            repos.Remove(repo);
        }
    }
}
