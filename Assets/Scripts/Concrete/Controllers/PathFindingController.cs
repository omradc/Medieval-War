using Assets.Scripts.Concrete.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Concrete.Movements
{
    public class PathFindingController : MonoBehaviour
    {
        public bool isStoping = true;
        public bool isUserControl;
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public Vector2 lastMousePos;
        [HideInInspector] public float moveSpeed;
        Direction direction;
        float time;
        bool forDirectionStopping;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            direction = new Direction(transform);
        }
        void Start()
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            InvokeRepeating(nameof(SetDirection), 0, .1f);

        }

        private void Update()
        {
            MovementControl();
        }

        // Kullanıcı hareketi
        public void Move(Vector3 mousePos, float stoppingDistance)
        {
            // Pozisyon verilmediyse veya etkilleşimli bir objeye tıklandıysa çalışma
            if (mousePos == null || InteractManager.Instance.interactedObj != null) return;
            print("Move");
            lastMousePos = mousePos;
            agent.stoppingDistance = stoppingDistance;
            agent.SetDestination(mousePos);
            isUserControl = true;
        }

        // AI hareketi
        public void MoveAI(Vector3 pos, float stoppingDistance)
        {
            if (pos == null || isUserControl) return;
            print("MoveAI");
            agent.stoppingDistance = stoppingDistance;
            agent.SetDestination(pos);
        }


        void SetDirection()
        {
            // Yön için durma kontrolü
            if (agent.hasPath && agent.velocity.magnitude > 0.12f)
                forDirectionStopping = false;

            else
                forDirectionStopping = true;

            // Birim hareket ederken gittiği yolu izler
            if (!forDirectionStopping)
                direction.Turn2DirectionWithVelocity(agent.velocity.x);
        }


        void MovementControl()
        {
            // Durma kontrolü
            if (agent.hasPath && agent.velocity.magnitude > 0.01f)
                isStoping = false;
            else
            {
                isStoping = true;
                if (time > Time.deltaTime * 5) // 5 Frame bekle
                {
                    time = 0;
                    isUserControl = false;
                }
                time += Time.deltaTime;
            }
        }
    }
}