using UnityEngine;

namespace Assets.Scripts.Concrete.PowerStats
{
    public class TntStats : PowerStats
    {
        public float longRange;
        public float reportRange;

        [Header("DYNAMİTE")]
        public float dynamiteSpeed;
        public float dynamiteExplosionRadius = .5f;
    }
}