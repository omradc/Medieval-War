﻿using System;
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
        public static int food;

        private void FixedUpdate()
        {
            print(gold);
        }
    }
}