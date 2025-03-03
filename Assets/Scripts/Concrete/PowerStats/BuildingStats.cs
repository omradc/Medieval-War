using UnityEngine;

namespace Assets.Scripts.Concrete.PowerStats
{
    public class BuildingStats : MonoBehaviour
    {
        [Header("Health")]
        public int health = 10;
        public bool regrenation = true;
        public float regenerationAmount = 1;
        public float regrenationPerTime = 1;
        public float regrenationAfterDamageTime = 5;
    }
}