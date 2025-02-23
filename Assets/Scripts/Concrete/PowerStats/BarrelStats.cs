using UnityEngine;

namespace Assets.Scripts.Concrete.PowerStats
{
    public class BarrelStats : PowerStats
    {
        public float longRange;
        public float reportRange;

        [Header("BARREL")]
        public float barrelExplosionRadius = 2;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, sightRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, reportRange);
            
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, longRange);
        }
    }
}