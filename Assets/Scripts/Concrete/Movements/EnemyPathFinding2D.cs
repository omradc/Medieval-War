﻿using Aoiti.Pathfinding;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using UnityEngine;

namespace Assets.Scripts.Concrete.Movements
{
    internal class EnemyPathFinding2D : PathFinding2D
    {
        [SerializeField] LayerMask obstacle;
        [SerializeField] bool drawLine;
        [HideInInspector] public Vector2 lastMousePos;
        [HideInInspector] public Animator animator;
        [HideInInspector] public EnemyDirection direction;
        EnemyController eC;

        private void Start()
        {
            obstacles = obstacle;
            drawDebugLines = drawLine;
            pathfinder = new Pathfinder<Vector2>(GetDistance, GetNeighbourNodes, 1000); //increase patience or gridSize for larger maps
            animator = transform.GetChild(0).GetComponent<Animator>();
            eC = GetComponent<EnemyController>();
            direction = new(this, eC);
        }
        private void Update()
        {
            if (isPathEnd)
            {
                path.Clear();
                pathLeftToGo.Clear();
                return;
            }

            if (pathLeftToGo.Count > 0) //if the target is not yet reached
            {
                Vector3 dir = (Vector3)pathLeftToGo[0] - transform.position;
                transform.position += dir.normalized * eC.currentSpeed;
                if (((Vector2)transform.position - pathLeftToGo[0]).sqrMagnitude < eC.currentSpeed * eC.currentSpeed)
                {
                    transform.position = pathLeftToGo[0];
                    pathLeftToGo.RemoveAt(0);
                }

            }

            if (pathLeftToGo.Count == 0)
            {
                moveCommand = false;
                eC.currentAttackRange = eC.attackRange;
                if (eC.playerUnits.Length <= 0)
                {
                    AnimationManager.Instance.IdleAnim(animator);
                }
            }


            if (drawDebugLines)
            {
                for (int i = 0; i < pathLeftToGo.Count - 1; i++) //visualize your path in the sceneview
                {
                    Debug.DrawLine(pathLeftToGo[i], pathLeftToGo[i + 1]);
                }
            }
        }

    }
}