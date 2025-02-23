using UnityEngine;

namespace Assets.Scripts.Concrete.PowerStats
{
    public class BarrelStats : PowerStats
    {
        public float longRange;
        public float reportRange;

        [Header("BARREL")]
        public float barrelExplosionRadius = 2;
    }
}