using UnityEngine;

namespace Assets.Scripts.Concrete.PowerStats
{
    public class ArcherStats : PowerStats
    {
        public float followDistance = 3;
        
        [Header("Arrow")]
        public float arrowSpeed = 25;
        public float arrowDestroyTime = 10;
    }
}