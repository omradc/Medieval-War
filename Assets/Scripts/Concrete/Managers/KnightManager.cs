using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Managers
{
    internal class KnightManager : MonoBehaviour
    {
        public static KnightManager Instance { get; private set; }
        [Header("Units")]
        public KnightFormation troopFormation;
        public float distance;
        [SerializeField] float optimumSendKnight = 0.1f;
        [Header("Setups")]
        public KnightOrderEnum unitOrderEnum;
        public Move move;
        IInput ıInput;
        public bool canMove;
        public bool workOnce;

        private void Awake()
        {
            Singelton();
            move = new();
            ıInput = new PcInput();
        }

        private void Start()
        {
            canMove = false;
            InvokeRepeating(nameof(OptimumSendKnights), 0, optimumSendKnight);
        }
        void Singelton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
                Destroy(this);
        }
        private void Update()
        {

            if (UIManager.Instance.isClearUnits)
            {
                Debug.Log("Ok");
            }
            if (ıInput.GetButton0())
            {
                if (!InteractManager.Instance.CheckUIElements() && InteractManager.Instance.tempKnights0.Count > 0)
                {
                    move.SetMousePos();
                    move.LineFormation(distance, false);
                    canMove = true;
                    workOnce = true;
                }
            }
        }

        void OptimumSendKnights()
        {
            bool reached = move.LeaderReachTheTarget(distance);
            if (canMove)   //Hareket emri; dokunma ile etkinleşir, liderin hedefe ulaşması ile sona erer 
                move.LineFormation(distance, false); // dinamik çalışır
            if (!canMove && workOnce && UIManager.Instance.addUnitToggle.isOn)
            {
                move.LineFormation(distance, true); // son kez çalışır
                workOnce = false;
            }
            if (reached)
                canMove = false;

        }
    }
}
