using System.Collections.Generic;
using UnityEngine;
using Aoiti.Pathfinding;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;

namespace Assets.Scripts.Concrete.Movements
{
    internal class UnitPathFinding2D : PathFinding2D
    {
        [SerializeField] LayerMask obstacle;
        [SerializeField] bool drawLine;
        [HideInInspector] public Vector2 lastMousePos;
        [HideInInspector] public Animator animator;
        [HideInInspector] public UnitDirection direction;
        KnightController kC;

        void Start()
        {
            base.obstacles = obstacle;
            drawDebugLines = drawLine;
            pathfinder = new Pathfinder<Vector2>(GetDistance, GetNeighbourNodes, 1000); //increase patience or gridSize for larger maps
            animator = transform.GetChild(0).GetComponent<Animator>();
            kC = GetComponent<KnightController>();
            direction = new(this, kC);
        }


        void Update()
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
                transform.position += dir.normalized * kC.currentMoveSpeed;

                //pathLeftToGo[0]; hedefe giderken kullandýðý yola bakmasýný saðlar
                direction.Turn2Direction(pathLeftToGo[0].x);

                if (((Vector2)transform.position - pathLeftToGo[0]).sqrMagnitude < kC.currentMoveSpeed * kC.currentMoveSpeed)
                {
                    transform.position = pathLeftToGo[0];
                    pathLeftToGo.RemoveAt(0);
                }
            }

            if (pathLeftToGo.Count == 0)
            {
                isUserPathFinding = false;
                moveCommand = false;
                kC.currentAttackRange = kC.attackRange;
                direction.Turn2Direction(Mathf.Infinity);
                if (kC.followTargets.Length <= 0)
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Chop") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Build"))
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

        // UPDATE ile ÇALIÞTIRMA
        public void GetMoveCommand(Vector2 mousePos)
        {
            Debug.Log("GetMoveCommand");
            moveCommand = true;
            AnimationManager.Instance.RunAnim(animator, 1);
            isUserPathFinding = true;
            isPathEnd = false;
            searchShortcut = false;
            kC.currentAttackRange = 0;
            lastMousePos = mousePos;
            Vector2 closestNode = GetClosestNode(transform.position);
            if (pathfinder.GenerateAstarPath(closestNode, GetClosestNode(mousePos), out path)) //Generate path between two points on grid that are close to the transform position and the assigned target.
            {
                if (searchShortcut && path.Count > 0)
                    pathLeftToGo = ShortenPath(path);
                else
                {
                    pathLeftToGo = new List<Vector2>(path);
                    if (!snapToGrid) pathLeftToGo.Add(mousePos);
                }
            }
        }
    }
}
