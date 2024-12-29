using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Concrete.Movements
{
    public class PathFindingController : MonoBehaviour
    {
        public bool isStopped = true;
        public bool isUserControl;
        public float userAndAIControlTransitionTime = 0.1f;

        float time;

        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public Vector2 lastMousePos;
        [HideInInspector] public float moveSpeed;
        Direction direction;
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            direction = new Direction(transform);
        }
        void Start()
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;

        }

        private void Update()
        {

            //direction.Turn2DirectionWithVelocity(agent.velocity.x);
            if (agent.hasPath)
                isStopped = false;

            else
            {
                isStopped = true;
                if (time > Time.deltaTime * 5) // 5 Frame bekle
                {
                    time = 0;
                    isUserControl = false;
                }
                time += Time.deltaTime;
            }
        }
        public void Move(Vector3 mousePos)
        {
            if (mousePos == null) return;
            print("Move");
            lastMousePos = mousePos;
            agent.SetDestination(mousePos);
            isUserControl = true;
        }

        public void MoveAI(Vector3 pos)
        {
            if (pos == null || isUserControl) return;
            print("MoveAI");
            agent.SetDestination(pos);
        }
    }
}