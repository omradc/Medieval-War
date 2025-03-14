﻿using Assets.Scripts.Concrete.Controllers;
using UnityEngine;

namespace Assets.Scripts.Concrete.Combats
{
    internal class Arrow : MonoBehaviour
    {
        [HideInInspector] public float arrowSpeed;
        [HideInInspector] public float arrowDestroyTime;
        [HideInInspector] public GameObject target;
        [HideInInspector] public GameObject archer;
        [HideInInspector] public int damage = 0;
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

            Destroy(gameObject, arrowDestroyTime);
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
            if ((transform.position - target.transform.position).magnitude < arrowStabDistance)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                // Hasar ver
                HealthController health = target.GetComponent<HealthController>();
                health.GetHit(damage, archer);

                // Hedefe saplanınca dur
                isStabbed = true;
                transform.SetParent(health.transform);

                // Hedef goblin ise
                if (target.layer == 13)
                {
                    GoblinController gC = target.GetComponent<GoblinController>();
                    if (gC.nonRangeDetechEnemy == null) // İlk saldıran hedefi takip et
                        gC.nonRangeDetechEnemy = archer;
                }

            }
        }
    }
}
