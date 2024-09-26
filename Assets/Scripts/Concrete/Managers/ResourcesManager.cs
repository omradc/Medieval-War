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
        public static int gold;
        public static int rock;
        public static int wood;
        public static int meat;

        private void Start()
        {
            InvokeRepeating(nameof(DisplayResources), 0.1f, 0.5f);
            InvokeRepeating(nameof(DisplayResources), 0.1f, 0.5f);
        }
        void DisplayResources()
        {
            print($"gold: {gold} / rock: {rock} / wood: {wood} / meat: {meat}");

        }
    }
}
