using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.Movements;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Concrete.Managers
{
    internal class KnightManager : MonoBehaviour
    {
        public static KnightManager Instance { get; private set; }
        [Header("Units")]
        public KnightFormation troopFormation;
        public float distanceBetweenUnits;
        [SerializeField] float optimumSendKnight = 0.1f;
        [Header("Setups")]
        public KnightOrderEnum unitOrderEnum;
        Move move;
        IInput ıInput;
        bool canMove;
        bool workOnce;

        private void Awake()
        {
            Singelton();
            move = new();
            ıInput = new PcInput();
        }

        private void Start()
        {
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
            if (ıInput.GetButton0())
            {
                if (!InteractManager.Instance.CheckUIElements())
                {
                    move.SendTheLeader();
                    canMove = true;
                    workOnce = true;
                }
            }

        }

        void OptimumSendKnights()
        {
            bool reached = move.LeaderReachTheTarget();
            if (canMove)   //Hareket edebiliyorsa 
                move.DynamicStayLineFormation(distanceBetweenUnits);
            if (!canMove && workOnce)  //Hareket edemiyorsa
            {
                move.StayLineFormation(distanceBetweenUnits);
                workOnce = false;
            }
            if (!reached)
                canMove = true;
            else
                canMove = false;
        }
    }
}
