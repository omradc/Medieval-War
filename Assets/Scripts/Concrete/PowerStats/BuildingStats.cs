using UnityEngine;

namespace Assets.Scripts.Concrete.PowerStats
{
	public class BuildingStats : MonoBehaviour
	{
        [Header("Health")]
        public int health;
        public bool regrenation;
        public float regenerationAmount = 1;
        public float regrenationPerTime = 1;
        public float regrenationAfterDamageTime = 5;
    }
}