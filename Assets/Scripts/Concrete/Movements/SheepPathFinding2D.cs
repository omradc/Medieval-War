using Aoiti.Pathfinding;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using UnityEngine;

namespace Assets.Scripts.Concrete.Movements
{
    class SheepPathFinding2D : PathFinding2D
    {

        [SerializeField] LayerMask obstacle;
        [SerializeField] bool drawLine;
        [HideInInspector] public Vector2 lastMousePos;
        [HideInInspector] public Animator animator;
        SheepController sC;


        private void Start()
        {
            obstacles = obstacle;
            drawDebugLines = drawLine;
            pathfinder = new Pathfinder<Vector2>(GetDistance, GetNeighbourNodes, 1000); //increase patience or gridSize for larger maps
            animator = transform.GetChild(0).GetComponent<Animator>();
            sC = GetComponent<SheepController>();
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
                transform.position += dir.normalized * sC.currentMoveSpeed;

                if (((Vector2)transform.position - pathLeftToGo[0]).sqrMagnitude < sC.currentMoveSpeed * sC.currentMoveSpeed)
                {
                    transform.position = pathLeftToGo[0];
                    pathLeftToGo.RemoveAt(0);
                }

            }

            if (pathLeftToGo.Count == 0)
                moveCommand = false;






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