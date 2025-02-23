using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Concrete.PowerStats
{
    public class WarriorStats : PowerStats
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, sightRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}