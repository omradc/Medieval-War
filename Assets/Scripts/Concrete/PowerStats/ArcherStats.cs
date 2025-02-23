using UnityEngine;

namespace Assets.Scripts.Concrete.PowerStats
{
    public class ArcherStats : PowerStats
    {
        [Header("Arrow")]
        public float arrowSpeed = 25;
        public float arrowDestroyTime = 10;


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, sightRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}