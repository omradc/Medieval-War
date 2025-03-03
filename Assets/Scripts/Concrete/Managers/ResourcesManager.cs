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


                DisplayCost(valueController, v);
                DisplayPanelColor(valueController, v);
                // UnloadRepoByCost(v.goldCost, v.rockCost, v.woodCost, v.meatCost);
                UnloadRepo(v.goldCost);
                DisplayResources();
                print($"{v.name}:  gold: {v.goldCost}, rock: {v.rockCost}, wood: {v.woodCost}, meat: {v.meatCost}");
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
        void UnloadRepo(int resourceCost)
        {
            int i = 0;
            while (resourceCost > 0 && i < 5) // Maliyet 0 dan büyük olduğu sürece tüm depolar maiyeti karşılayana kadar, kapasitesinden düşer.
            {
                //RepoController repoController = FindFullRepo().GetComponent<RepoController>(); //En dolu depoyu bul
                //print(repoController.GetInstanceID());
                //int remainingCapacity = repoController.gold -= resourceCost;
                //if (remainingCapacity < 0) // Kapasite karşılamıyorsa
                //{
                //    repoController.gold -= remainingCapacity; // depodan alınan fazlalığı ekle
                //    resourceCost = -remainingCapacity; // yeni maliyet 
                //}
                //else // Kapasite karşılıyorsa döngüyü sonlandır
                //    resourceCost = 0;
                //print(resourceCost);
                //i++;

                while (resourceCost > 0 && i < 5) // Maliyet 0 dan büyük olduğu sürece tüm depolar maiyeti karşılayana kadar, kapasitesinden düşer.
                {
                    GameObject depo = FindFullRepo(); //En dolu depoyu bul
                    int remainingCapacity = depo.GetComponent<RepoController>().gold -= resourceCost;


                    if (remainingCapacity < 0) // Kapasite karşılamıyorsa
                    {
                        depo.GetComponent<RepoController>().gold -= remainingCapacity; // depodan alınan fazlalığı ekle
                        resourceCost = -remainingCapacity; // yeni maliyet 
                    }
                    else // Kapasite karşılıyorsa döngüyü sonlandır
                        resourceCost = 0;
                    i++;
                }

            }
        }
        GameObject FindFullRepo()
        {
            GameObject fullestRepo = null;
            float repoFullness = 0;
            for (int i = 0; i < repos.Count; i++)
            {
                float currentRepoFullnes = repos[i].GetComponent<RepoController>().CurrentRepoCapacity();
                if (currentRepoFullnes > repoFullness)
                {
                    repoFullness = currentRepoFullnes;
                    fullestRepo = repos[i];
                }
            }
            return fullestRepo;
        }
        //void UnloadRepoByCost(int goldCost, int rockCost, int woodCost, int meatCost)
        //{

        //    UnloadRepo(goldCost);

        //    //UnloadRepo(ref repoController.rock, rockCost);
        //    //UnloadRepo(ref repoController.wood, woodCost);
        //    //UnloadRepo(ref repoController.meat, meatCost);
        //}
        public void DestroyRepo(GameObject repo)
        {
            RepoController repoController = repo.GetComponent<RepoController>();
            totalGold -= repoController.gold;
            totalRock -= repoController.rock;
            totalWood -= repoController.wood;
            totalMeat -= repoController.meat;
            repos.Remove(repo);
        }
        public void RemoveRepo(GameObject repo)
        {
            repos.Remove(repo);
        }
    }
}
