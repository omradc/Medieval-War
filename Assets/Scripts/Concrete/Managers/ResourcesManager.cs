using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Concrete.Managers
{
    internal class ResourcesManager : MonoBehaviour
    {
        public static ResourcesManager Instance;
        public static int gold;
        public static int rock;
        public static int wood;
        public static int meat;
        public Value[] value;
        int index;

        private void Awake()
        {
            Singelton();
            gold = 40;
            rock = 40;
            wood = 40;
            meat = 40;
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
            InvokeRepeating(nameof(DisplayResources), 0.1f, 0.5f);
        }
        void DisplayResources()
        {
            print($"gold: {gold} / rock: {rock} / wood: {wood} / meat: {meat}");
        }

        public bool Buy(string name)
        {
            // Satın alınan ürünün adını değer listesinde ara, bulunca index sırasını eşitle
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].name == name)
                    index = i;
            }

            Value v = value[index];

            if (gold >= v.goldValue && rock >= v.rockValue && wood >= v.woodValue && meat >= v.meatValue)
            {
                gold -= v.goldValue;
                rock -= v.rockValue;
                wood -= v.woodValue;
                meat -= v.meatValue;
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
