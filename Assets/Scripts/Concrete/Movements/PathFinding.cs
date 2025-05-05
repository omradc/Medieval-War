using Assets.Scripts.Concrete.Combats;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Concrete.Movements
{
    public class PathFinding : MonoBehaviour
    {
        [HideInInspector] public bool isStopping;
        [HideInInspector] public bool isMovementStopping;
        [HideInInspector] public bool isUserControl;
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public Vector2 targetPos;
        [HideInInspector] public Vector2 lastMousePos;
        [HideInInspector] public float moveSpeed;
        Direction direction;
        float time;
        Vector2 worldCenterPos;
        NavMeshPath path;
        bool isStuck;
        TargetPriority targetPriority;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            direction = new(transform);
            path = new();
            if (gameObject.layer == 6 || gameObject.layer == 13)
                targetPriority = GetComponent<TargetPriority>();
        }
        void Start()
        {
            isStopping = true;
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            worldCenterPos = new Vector2(0, -2);

            InvokeRepeating(nameof(SetDirection), 0, .1f);
            //InvokeRepeating(nameof(CalculateStuck), 0, 1);
        }
        private void Update()
        {
            MovementControl();
            CalculateStuck();
        }

        public void Move(Vector3 mousePos, float stoppingDistance = 0) // Kullanıcı hareketi
        {
            // Pozisyon verilmediyse veya etkilleşimli bir objeye tıklandıysa çalışma
            if (InteractManager.Instance.interactedObj == null)
            {
                print("Move");
                lastMousePos = mousePos;
                targetPos = mousePos;
                agent.stoppingDistance = stoppingDistance;
                agent.SetDestination(mousePos);
                isUserControl = true;
            }
        }
        public void MoveAI(Vector3 pos, float stoppingDistance = 0)  // AI hareketi
        {
            if (isUserControl) return;
            if (gameObject.layer == 6 && isStuck) return;//Şovalye surlardayken, düşman takibi yapmaz. 
            print("MoveAI");
            targetPos = pos;
            agent.stoppingDistance = stoppingDistance;
            agent.SetDestination(pos);
        }
        void SetDirection()
        {
            if (agent.velocity.magnitude > 0.05f)
                isMovementStopping = false;

            else
                isMovementStopping = true;

            // Birim hareket ederken gittiği yolu izler
            if (!isMovementStopping)
                direction.Turn2DirectionWithVelocity(agent.velocity.x);
        }
        void MovementControl()
        {
            // Durma kontrolü
            if (agent.hasPath && agent.velocity.magnitude > 0.01f)
                isStopping = false;
            else
            {
                isStopping = true;
                if (time > Time.deltaTime * 5) // 5 Frame bekle
                {
                    time = 0;
                    isUserControl = false;
                }
                time += Time.deltaTime;
            }
        }
        void CalculateStuck()
        {
            if (targetPriority == null) return;
            // İç noktadan dış noktaya bir yol olup olmadığını kontrol et
            if (NavMesh.CalculatePath(transform.position, worldCenterPos, NavMesh.AllAreas, path))
            {
                // Eğer yol eksikse veya hiç yoksa, kale tamamen kapanmış demektir
                isStuck = path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid;
            }
            if (isStuck)
                targetPriority.currentPriority = 0; // Öncelik yok
            else
                targetPriority.currentPriority = targetPriority.priority;
        }
    }
}