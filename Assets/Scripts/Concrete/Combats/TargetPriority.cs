using UnityEngine;

namespace Assets.Scripts.Concrete.Combats
{
    public class TargetPriority : MonoBehaviour
    {
        /// <summary>
        /// Değer arttıkça öceliği azalır
        /// </summary>
        public int priority;
        /// <summary>
        /// Maksimum saldırgan sayısına ulaştığında, önceliği azalır
        /// </summary>
        [Range(0, 10)] public int maxAttacker = 3;
        [HideInInspector] public int attackingPersonNumber;
    }
}