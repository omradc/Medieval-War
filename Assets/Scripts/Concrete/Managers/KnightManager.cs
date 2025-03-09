using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.AI;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.Movements;
using NUnit.Framework;
using System.Collections.Generic;
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
                if (!InteractManager.Instance.CheckUIElements())
                {
                    TakeFormation(InteractManager.Instance.tempKnights0, true);
                    TakeFormation(InteractManager.Instance.tempKnights1, true);
                    TakeFormation(InteractManager.Instance.tempKnights2, true);
                    TakeFormation(InteractManager.Instance.tempKnights3, true);
                }
            }
        }

        void OptimumSendKnights()
        {
            TakeFormation(InteractManager.Instance.tempKnights0, false);
            TakeFormation(InteractManager.Instance.tempKnights1, false);
            TakeFormation(InteractManager.Instance.tempKnights2, false);
            TakeFormation(InteractManager.Instance.tempKnights3, false);
        }

        bool canMove;
        bool workOnce;
        void TakeFormation(List<GameObject> knights, bool initialize)
        {
            if (knights.Count > 0)
            {
                if (initialize) // First click setup
                {
                    move.SetMousePos();
                    move.LineFormation(distance, false, knights);
                    canMove = true;
                    workOnce = true;
                    return;
                }

                bool reached = move.LeaderReachTheTarget(distance, knights);
                if (canMove)   //Hareket emri; dokunma ile etkinleşir, liderin hedefe ulaşması ile sona erer 
                    move.LineFormation(distance, false, knights); // dinamik çalışır
                if (!canMove && workOnce && UIManager.Instance.addUnitToggle.isOn)
                {
                    move.LineFormation(distance, true, knights); // son kez çalışır
                    workOnce = false;
                }
                if (reached)
                    canMove = false;
            }
        }
        //void TakeFormation(List<GameObject> knights)
        //{
        //    if (knights.Count > 0)
        //    {
        //        bool reached = move.LeaderReachTheTarget(distance, knights);
        //        if (canMove)   //Hareket emri; dokunma ile etkinleşir, liderin hedefe ulaşması ile sona erer 
        //            move.LineFormation(distance, false, knights); // dinamik çalışır
        //        if (!canMove && workOnce && UIManager.Instance.addUnitToggle.isOn)
        //        {
        //            move.LineFormation(distance, true, knights); // son kez çalışır
        //            workOnce = false;
        //        }
        //        if (reached)
        //            canMove = false;
        //    }
        //}
        //void InitializeFormation(List<GameObject> knights)
        //{
        //    if (knights.Count > 0)
        //    {
        //        move.SetMousePos();
        //        move.LineFormation(distance, false, knights);
        //        canMove = true;
        //        workOnce = true;
        //    }
        //}
    }
}
