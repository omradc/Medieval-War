using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Combats
{
    internal class Dynamite : MonoBehaviour
    {
        [HideInInspector] public GameObject target;
        [HideInInspector] public GameObject dynamite;
        [HideInInspector] public LayerMask targetLayer;
        [HideInInspector] public int damage = 0;
        [HideInInspector] public float radius = 0.5f;
        [HideInInspector] public float dynamiteSpeed = 10;
        Collider2D[] hits;
        float dynamiteExplosionDistance = 0.3f;
        bool isExploded;
        public float rotateValue;
        Transform visual;
        GameObject explosion;
        private void Awake()
        {
            visual = transform.GetChild(0);
            explosion = transform.GetChild(1).gameObject;
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
                    HealthController targetHealth = hits[i].GetComponent<HealthController>();
                    targetHealth.GetHit(damage, dynamite);
                    if (targetHealth.isDead)
                    {
                        dynamite.GetComponent<GoblinController>().nonRangeDetechEnemy = null;
                        dynamite.GetComponent<PathFinding>().agent.ResetPath();
                    }
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
