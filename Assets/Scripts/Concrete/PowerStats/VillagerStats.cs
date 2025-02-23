using Assets.Scripts.Concrete.Controllers;
using UnityEngine;

namespace Assets.Scripts.Concrete.PowerStats
{
     class VillagerStats : PowerStats
    {
        [Header("TREE")]
        public int treeDamage = 1;
        public float chopSpeed;
        public float chopTreeSightRange;
        public float woodCollectTime;

        [Header("MİNE")]
        public float miningTime;

        [Header("SHEEP")]
        public float meatCollectTime;

        [Header("CONSTRUCTION")]
        public float buildSpeed;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, sightRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}