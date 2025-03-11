using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.AI;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.Movements;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Concrete.Managers
{
    internal class KnightManager : MonoBehaviour
    {
        public static KnightManager Instance { get; private set; }
        [Header("Units")]
        public KnightFormation troopFormation;
        public float distance;
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
            //InvokeRepeating(nameof(Optimum), 0, 1f);
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
            if (ıInput.GetButtonDown0())
            {
                if (!InteractManager.Instance.CheckUIElements())
                    move.MoveCommand();
            }
        }
        //void Optimum()
        //{
        //    move.SetSpeed();
        //}
        #region Dynamic
        //private void Update()
        //{

        //    if (UIManager.Instance.isClearUnits)
        //    {
        //        Debug.Log("Ok");
        //    }
        //    if (ıInput.GetButton0())
        //    {
        //        if (!InteractManager.Instance.CheckUIElements())
        //        {
        //            TakeFormation(InteractManager.Instance.tempKnights0, true);
        //            TakeFormation(InteractManager.Instance.tempKnights1, true);
        //            TakeFormation(InteractManager.Instance.tempKnights2, true);
        //            TakeFormation(InteractManager.Instance.tempKnights3, true);
        //        }
        //    }
        //}

        //void OptimumSendKnights()
        //{
        //    TakeFormation(InteractManager.Instance.tempKnights0, false);
        //    TakeFormation(InteractManager.Instance.tempKnights1, false);
        //    TakeFormation(InteractManager.Instance.tempKnights2, false);
        //    TakeFormation(InteractManager.Instance.tempKnights3, false);
        //}

        //bool canMove;
        //bool workOnce;
        //void TakeFormation(List<GameObject> knights, bool initialize)
        //{
        //    if (knights.Count > 0)
        //    {
        //        if (initialize) // First click setup
        //        {
        //            move.SetMousePos();
        //            move.LineFormation(distance, true, knights);
        //            canMove = true;
        //            workOnce = true;
        //            return;
        //        }

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
        #endregion

    }
}
