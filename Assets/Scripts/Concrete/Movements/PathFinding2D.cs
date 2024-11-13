using System.Collections.Generic;
using UnityEngine;
using Aoiti.Pathfinding;

namespace Assets.Scripts.Concrete.Movements
{
    internal class PathFinding2D : MonoBehaviour
    {

        protected float gridSize = 0.5f; //increase patience or gridSize for larger maps

        protected Pathfinder<Vector2> pathfinder; //the pathfinder object that stores the methods and patience
        protected LayerMask obstacles;
        protected bool searchShortcut = false;
        protected bool snapToGrid = false;
        //protected Vector2 targetNode; //target in 2D space
        protected List<Vector2> path;
        public List<Vector2> pathLeftToGo = new List<Vector2>();
        protected bool drawDebugLines;
        [HideInInspector] public bool right;
        [HideInInspector] public bool left;
        [HideInInspector] public bool up;
        [HideInInspector] public bool down;
        [HideInInspector] public bool upRight;
        [HideInInspector] public bool upLeft;
        [HideInInspector] public bool downRight;
        [HideInInspector] public bool downLeft;
        [HideInInspector] public bool isUserPathFinding;
        public bool isPathEnd;
        [HideInInspector] public bool moveCommand;

        //Update ile çalışamamalı, her karede yeni bir liste oluşturur
        public void AIGetMoveCommand(Vector2 targetPos)
        {
            // Kullanıcının verdiği hareket emri, yapay zekanın verdiği hareket emrinden daha önceliklidir!
            if (isUserPathFinding) return;
            Debug.Log("AIGetMoveCommand");
            searchShortcut = true;
            Vector2 closestNode = GetClosestNode(transform.position);
            if (pathfinder.GenerateAstarPath(closestNode, GetClosestNode(targetPos), out path)) //Generate path between two points on grid that are close to the transform position and the assigned target.
            {
                if (searchShortcut && path.Count > 0)
                    pathLeftToGo = ShortenPath(path);
                else
                {
                    pathLeftToGo = new List<Vector2>(path);
                    if (!snapToGrid) pathLeftToGo.Add(targetPos);
                }

            }
        }


        /// <summary>
        /// Finds closest point on the grid
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected Vector2 GetClosestNode(Vector2 target)
        {
            return new Vector2(Mathf.Round(target.x / gridSize) * gridSize, Mathf.Round(target.y / gridSize) * gridSize);
        }
        /// <summary>
        /// A distance approximation. 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        protected float GetDistance(Vector2 A, Vector2 B)
        {
            return (A - B).sqrMagnitude; //Uses square magnitude to lessen the CPU time.
        }
        /// <summary>
        /// Finds possible conenctions and the distances to those connections on the grid.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        protected Dictionary<Vector2, float> GetNeighbourNodes(Vector2 pos)
        {
            Dictionary<Vector2, float> neighbours = new Dictionary<Vector2, float>();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0) continue;

                    Vector2 dir = new Vector2(i, j) * gridSize;
                    if (!Physics2D.Linecast(pos, pos + dir, obstacles))
                    {
                        neighbours.Add(GetClosestNode(pos + dir), dir.magnitude);
                    }
                }

            }
            return neighbours;
        }
        protected List<Vector2> ShortenPath(List<Vector2> path)
        {
            List<Vector2> newPath = new List<Vector2>();

            for (int i = 0; i < path.Count; i++)
            {
                newPath.Add(path[i]);
                for (int j = path.Count - 1; j > i; j--)
                {
                    if (!Physics2D.Linecast(path[i], path[j], obstacles))
                    {

                        i = j;
                        break;
                    }
                }
                newPath.Add(path[i]);
            }
            newPath.Add(path[path.Count - 1]);
            return newPath;
        }
    }
}
