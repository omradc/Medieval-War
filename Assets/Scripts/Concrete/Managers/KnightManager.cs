using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Abstracts.Movements;
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
        public float distanceBetweenUnits;

        [Header("Setups")]
        public KnightOrderEnum unitOrderEnum;
        IMove ıMove;
        IInput ıInput;
        private void Awake()
        {
            Singelton();
            ıMove = new Move();
            ıInput = new PcInput();
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
            if (ıInput.GetButtonDown0)
            {
                if (!InteractManager.Instance.CheckUIElements())
                    ıMove.MoveCommand();
            }

        }
    }
}
