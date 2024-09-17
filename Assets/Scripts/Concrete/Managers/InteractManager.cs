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
        [HideInInspector] public GameObject ınteractedObj;
        [HideInInspector] public GameObject ınteractedUnit;
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
                if (ınteractedObj != null)
                {
                    // Etkileşim olan obje baraka ise, birlik basma ekranı açılır
                    if (ınteractedObj.layer == 8)
                        ınteractedObj.GetComponent<PanelController>().TrainUnitVisibility(true);

                    // Etkileşim olan obje, birim ise,
                    if (ınteractedObj.layer == 6)
                        ınteractedUnit = ınteractedObj;




                }
            }

            if (ıInput.GetButtonUp0)
            {
                ınteractedObj = null;
                ınteractedUnit = null;

            }
        }

    }
}
