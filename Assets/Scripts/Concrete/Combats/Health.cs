using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Concrete.Combats
{
    internal class Health : MonoBehaviour
    {
        public float health;
        public float currentHealth;

        private void Start()
        {
            currentHealth = health;
        }

        public void GetHit(int attackDamage)
        {
            currentHealth -= attackDamage;
            if (currentHealth <= 0)
                Dead();
        }
        public void Dead()
        {

            //transform.position = Vector3.zero;
            Destroy(gameObject);
        }
    }
}
