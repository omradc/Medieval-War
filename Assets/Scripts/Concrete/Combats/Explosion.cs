﻿using Assets.Scripts.Concrete.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Concrete.Combats
{
    internal class Explosion : MonoBehaviour
    {
        [HideInInspector] public int damage = 0;
        [HideInInspector] public float radius = 0.5f;
        [HideInInspector] public LayerMask targetLayer;
        Collider2D[] hits;
        private void Start()
        {
            transform.localScale = new Vector2(radius * 2, radius * 2);
            // Hasar ver
            hits = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);
            for (int i = 0; i < hits.Length; i++)
            {
                hits[i].GetComponent<HealthController>().GetHit(damage);
            }
            Destroy(gameObject, 0.67f);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
