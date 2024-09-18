using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.SelectSystem;
using System;
using UnityEngine;

namespace Assets.Scripts.Concrete.Managers
{
    internal class InteractManager : MonoBehaviour
    {
        public static InteractManager Instance { get; private set; }

        [SerializeField] LayerMask barrackLayer;
        [SerializeField] LayerMask towerLayer;
        [SerializeField] LayerMask castleLayer;
        [SerializeField] LayerMask unitLayer;
        Interact ınteract;
        [HideInInspector] public GameObject interactedObj;
        [HideInInspector] public GameObject interactedUnit;
        public GameObject interactedMine;
        public GameObject interactedTree;
        IInput ıInput;
        private void Awake()
        {
            Singelton();

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
        private void Start()
        {
            ınteract = new Interact(this);
            ıInput = new PcInput();

        }
        private void Update()
        {
            if (ıInput.GetButtonDown0)
            {

                // mouse ile tıklanan obje ile etkileşime girilir
                ınteract.InteractClickedObj();
                if (interactedObj != null)
                {
                    // Etkileşim olan obje baraka ise, birlik basma ekranı açılır
                    if (interactedObj.layer == 8)
                        interactedObj.GetComponent<PanelController>().TrainUnitVisibility(true);

                    // Etkileşim olan obje, birim ise,
                    if (interactedObj.layer == 6)
                        interactedUnit = interactedObj;

                    // Etkileşim olan obje, maden ise,
                    if (interactedObj.layer == 14)
                        interactedMine = interactedObj;

                    // Etkileşim olan obje, ağaç ise,
                    if (interactedObj.layer == 15)
                        interactedMine = interactedObj;


                }
            }

            if (ıInput.GetButtonUp0)
            {
                interactedObj = null;
                interactedUnit = null;
                interactedMine = null;

            }
        }

    }
}
