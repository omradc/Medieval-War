using UnityEngine;

namespace Assets.Scripts.Concrete.PowerStats
{
	public class PowerStats : MonoBehaviour
	{
        [Header("Health")]
        public int health;
        public float regenerationAmount = 1;
        public float regrenationPerTime = 1;
        public float regrenationAfterDamageTime = 5;


        [Header("Movement")]
        [Range(2, 4f)] public float moveSpeed = 1f;

        [Header("Attack")]
        public int damage;
        public float attackSpeed;
        public float attackInterval;
        
        [Header("Range")]
        public float attackRange;
        public float sightRange;
    }
}