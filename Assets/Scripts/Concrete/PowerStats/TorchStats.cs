using UnityEngine;

namespace Assets.Scripts.Concrete.PowerStats
{
    public class TorchStats : PowerStats
    {
        public float longRange;
        public float reportRange;

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