using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Combats
{
    public class TargetPriority : MonoBehaviour
    {
        /// <summary>
        /// Değer arttıkça öceliği artar
        /// </summary>
        public int priority;
        public int currentPriority;
        
        /// <summary>
        /// Maksimum saldırgan sayısına ulaştığında, önceliği azalır
        /// </summary>
        [Range(0, 10)] public int maxAttacker = 3;
        [HideInInspector] public int attackingPersonNumber;
    }
}