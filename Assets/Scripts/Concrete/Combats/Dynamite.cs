using Assets.Scripts.Concrete.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Concrete.Combats
{
    internal class Dynamite : MonoBehaviour
    {
        public GameObject target;
        public LayerMask targetLayer;
        public int damage = 0;
        public float radius = 0.5f;
        public float dynamiteSpeed = 10;
        public Collider2D[] hits;
        float dynamiteExplosionDistance = 0.3f;
        bool isExploded;
        public float rotateValue;
        Transform visual;
        GameObject explosion;
        Animator animator;
        private void Awake()
        {
            visual = transform.GetChild(0);
            explosion = transform.GetChild(1).gameObject;
            animator = visual.GetComponent<Animator>();

        }
        private void Start()
        {
            explosion.transform.localScale = new Vector2(radius * 2, radius * 2);
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
        }
        private void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }
            if (isExploded)
            {
                Destroy(gameObject, 0.67f);
                return;
            }

            visual.Rotate(0, 0, 1 * Time.deltaTime * dynamiteSpeed * rotateValue);
            Vector2 direction = (target.transform.position - transform.position).normalized;
            transform.Translate(direction * dynamiteSpeed * Time.deltaTime, Space.World);

            // Dinamit hedefe ulaştı
            if (Vector2.Distance(transform.position, target.transform.position) < dynamiteExplosionDistance)
            {
                // Hasar ver
                hits = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);
                for (int i = 0; i < hits.Length; i++)
                {
                    hits[i].GetComponent<Health>().GetHit(damage);
                }

                explosion.SetActive(true);
                isExploded = true;

            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
