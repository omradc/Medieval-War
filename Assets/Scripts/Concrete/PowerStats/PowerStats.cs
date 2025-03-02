using UnityEngine;

namespace Assets.Scripts.Concrete.PowerStats
{
    public class PowerStats : MonoBehaviour
    {
        [Header("Health")]
        public int health = 10;
        public bool regrenation = true;
        public float regenerationAmount = 1;
        public float regrenationPerTime = 1;
        public float regrenationAfterDamageTime = 5;

        [Header("Movement")]
        [Range(0.5f, 4f)] public float moveSpeed = 1f;

        [Header("Attack")]
        public int damage = 1;
        public float attackSpeed = 1;
        public float attackInterval = 1;

        [Header("Range")]
        public float attackRange = .6f;
        public float sightRange = 4;
    }
}