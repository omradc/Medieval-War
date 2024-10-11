using Assets.Scripts.Concrete.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Concrete.Combats
{
    internal class Arrow : MonoBehaviour
    {
        [HideInInspector] public GameObject target;
        [HideInInspector] public int damage = 0;
        [HideInInspector] public float arrowSpeed = 2;
        float arrowStabDistance = 0.3f;
        bool isStabbed;
        private void Start()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }
            Vector2 direction = target.transform.position - transform.position;
            // Yönü açıya çevir (atanmış olduğu 2D düzlem için Z ekseni etrafında döndür)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // Z ekseni etrafında rotasyonu ayarla
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            Destroy(gameObject, 10);
        }
        private void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }
            if (isStabbed) return;

            Vector2 direction = (target.transform.position - transform.position).normalized;
            transform.Translate(direction * arrowSpeed * Time.deltaTime, Space.World);

            // Ok hedefe saplandı
            if (Vector2.Distance(transform.position, target.transform.position) < arrowStabDistance)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                // Hasar ver
                HealthController health = target.GetComponent<HealthController>();
                health.GetHit(damage);

                // Hedefe saplanınca dur
                isStabbed = true;
                transform.SetParent(health.transform);

            }
        }
    }
}
