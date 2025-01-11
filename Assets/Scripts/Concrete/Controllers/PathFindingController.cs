using Assets.Scripts.Concrete.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Concrete.Movements
{
    public class PathFindingController : MonoBehaviour
    {
        public bool isStopped = true;
        public bool isUserControl;
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public Vector2 lastMousePos;
        [HideInInspector] public float moveSpeed;
        Direction direction;
        float time;

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
            // Birim hareket ederken gittiği yolu izler
            if (!isStopped)
                direction.Turn2DirectionWithVelocity(agent.velocity.x);

            // Durma kontrolü
            if (agent.hasPath && agent.velocity.magnitude > 0)
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

        // Kullanıcı hareketi
        public void Move(Vector3 mousePos)
        {
            // Pozisyon verilmediyse veya etkilleşimli bir objeye tıklandıysa çalışma
            if (mousePos == null || InteractManager.Instance.interactedObj != null) return;
            print("Move");
            lastMousePos = mousePos;
            agent.SetDestination(mousePos);
            isUserControl = true;
        }

        // AI hareketi
        public void MoveAI(Vector3 pos)
        {
            if (pos == null || isUserControl) return;
            print("MoveAI");
            agent.SetDestination(pos);
        }
    }
}