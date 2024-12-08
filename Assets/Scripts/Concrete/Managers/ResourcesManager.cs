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
        public Value[] value;
        public static int gold;
        public static int rock;
        public static int wood;
        public static int meat;

        [HideInInspector] public bool goldIsEnough;
        [HideInInspector] public bool rockIsEnough;
        [HideInInspector] public bool woodIsEnough;
        [HideInInspector] public bool meatIsEnough;
        [HideInInspector] public int goldValue;
        [HideInInspector] public int rockValue;
        [HideInInspector] public int woodValue;
        [HideInInspector] public int meatValue;

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
            gold = 40;
            rock = 40;
            wood = 40;
            meat = 40;
            DisplayResources();
        }
        public void DisplayResources()
        {
            goldText.text = gold.ToString();
            rockText.text = rock.ToString();
            woodText.text = wood.ToString();
            meatText.text = meat.ToString();
        }

        public bool Buy(string name)
        {
            // Satın alınan ürünün adını değer listesinde ara, bulunca index sırasını eşitle
            for (int i = 0; i < value.Length; i++)
            {
                if (name == "")
                {
                    Debug.Log("---NOT FOUND---");
                    break;
                }
                if (value[i].name == name)
                {
                    index = i;
                    break;
                }
            }

            Value v = value[index];

            if (gold >= v.goldValue && rock >= v.rockValue && wood >= v.woodValue && meat >= v.meatValue)
            {
                gold -= v.goldValue;
                rock -= v.rockValue;
                wood -= v.woodValue;
                meat -= v.meatValue;
                DisplayResources();
                return true;
            }

            else
            {
                return false;
            }
        }

        public void CheckResources(string name)
        {
            // Satın alınan ürünün adını değer listesinde ara, bulunca index sırasını eşitle
            for (int i = 0; i < value.Length; i++)
            {
                // Index bulunamadı
                if (name == "")
                {
                    Debug.Log("---NOT FOUND---");
                    break;
                }

                // Index bulundu
                if (value[i].name == name)
                {
                    index = i;
                    break;
                }
            }

            Value v = value[index];

            if (gold >= v.goldValue)
                goldIsEnough = true;
            if (gold < v.goldValue)
                goldIsEnough = false;

            if (rock >= v.rockValue)
                rockIsEnough = true;
            if (rock < v.rockValue)
                rockIsEnough = false;

            if (wood >= v.woodValue)
                woodIsEnough = true;
            if (wood < v.woodValue)
                woodIsEnough = false;

            if (meat >= v.meatValue)
                meatIsEnough = true;
            if (meat < v.meatValue)
                meatIsEnough = false;
        }

        public void ResourcesValues(string name)
        {
            // Satın alınan ürünün adını değer listesinde ara, bulunca index sırasını eşitle
            for (int i = 0; i < value.Length; i++)
            {
                // Index bulunamadı
                if (name == "")
                {
                    Debug.Log("---NOT FOUND---");
                    break;
                }

                // Index bulundu
                if (value[i].name == name)
                {
                    index = i;
                    break;
                }
            }

            Value v = value[index];

            goldValue = v.goldValue;
            rockValue = v.rockValue;
            woodValue = v.woodValue;    
            meatValue = v.meatValue;
        }
    }
}
