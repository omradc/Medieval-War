using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Combats
{
    public class TargetPriority : MonoBehaviour
    {
        /// <summary>
        /// Değer arttıkça öceliği artar
        /// </summary>
        [HideInInspector] public int priority;
        [HideInInspector] public int currentPriority;

        /// <summary>
        /// Maksimum saldırgan sayısına ulaştığında, önceliği azalır
        /// </summary>
        [HideInInspector][Range(0, 10)] public int maxAttacker = 3;
        [HideInInspector] public int currentAttackerNumber;
    }
}