using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Abstracts.Movements;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.Movements;
using Assets.Scripts.Concrete.SelectSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Concrete.Managers
{
    internal class UnitManager : MonoBehaviour
    {
        public static UnitManager Instance { get; private set; }
        [Header("Units")]
        public TroopFormation troopFormation;
        public float distanceBetweenUnits;

        [Header("Setups")]
        [SerializeField] LayerMask troopLayer;

        IMove ıMove;
        IInput ıInput;
        public UnitOrderEnum unitOrderEnum;
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
                ıMove.MoveCommand();
            }

        }
    }
}
