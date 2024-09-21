using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.SelectSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Concrete.Managers
{
    internal class InteractManager : MonoBehaviour
    {
        public static InteractManager Instance { get; private set; }
        [SerializeField] LayerMask terrainLayer;
        [SerializeField] LayerMask unitLayer;
        Interact ınteract;
        public GameObject interactedObj;
        public GameObject interactedUnit;
        public GameObject interactedMine;
        public GameObject interactedTree;
        public GameObject interactedSheep;
        public GameObject interactedFences;
        IInput ıInput;

        public List<GameObject> selectedUnits;
        GameObject currentObj;
        public Vector2 startPos;
        Vector2 endPos;
        public bool isDragging;


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
            SelectOneByOne();
            SelectMultiple();
            ClearSelectedObjs();

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
                        interactedTree = interactedObj;

                    // Etkileşim olan obje, koyun ise,
                    if (interactedObj.layer == 16)
                        interactedSheep = interactedObj;

                    // Etkileşim olan obje, çitler ise,
                    if (interactedObj.layer == 12)
                        interactedFences = interactedObj;

                }
            }
            if (ıInput.GetButtonUp0)
            {
                interactedObj = null;
                interactedUnit = null;
                interactedMine = null;
                interactedTree = null;
                interactedSheep = null;
                interactedFences = null;
            }
        }

        public void SelectOneByOne()
        {
            if (ıInput.GetButtonDown1)
            {
                // Ekran koordinatlarını dünya koordinatlarına çevir
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, 1, unitLayer);
                if (hit.collider != null)
                {
                    currentObj = hit.collider.gameObject;
                    UnitController uC = currentObj.GetComponent<UnitController>();
                    uC.workOnce = true;
                    uC.isSeleceted = true;
                    // Aynı nesneyi tekrar diziye atma
                    if (!selectedUnits.Contains(currentObj))
                        selectedUnits.Add(currentObj);

                    SelectedObjColor(0.5f, currentObj);

                }

            }
            if (ıInput.GetButtonDown0)
            {
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    selectedUnits[i].GetComponent<UnitController>().unitOrderEnum = UnitManager.Instance.unitOrderEnum;
                    SelectedObjColor(1f, selectedUnits[i]);
                }

            }


        }
        public void SelectMultiple()
        {
            if (ıInput.GetButtonDown1)
            {
                startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                isDragging = true;
            }


            if (ıInput.GetButtonUp1)
            {
                endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                isDragging = false;
                Collider2D[] hits = Physics2D.OverlapAreaAll(startPos, endPos, unitLayer);
                for (int i = 0; i < hits.Length; i++)
                {

                    currentObj = hits[i].gameObject;

                    // Aynı nesneyi tekrar diziye atma
                    if (!selectedUnits.Contains(currentObj))
                        selectedUnits.Add(currentObj);
                    UnitController uC = selectedUnits[i].gameObject.GetComponent<UnitController>();
                    uC.unitOrderEnum = UnitManager.Instance.unitOrderEnum;
                    uC.workOnce = true;
                    uC.isSeleceted = true;
                    SelectedObjColor(0.5f, currentObj);
                }

            }

            if (ıInput.GetButtonDown0)
            {
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    selectedUnits[i].GetComponent<UnitController>().unitOrderEnum = UnitManager.Instance.unitOrderEnum;
                }

            }
        }
        public void ClearSelectedObjs()
        {
            if (ıInput.GetButtonUp0 && !ıInput.GetButton1)
                selectedUnits.Clear();
        }
        void SelectedObjColor(float alphaValue, GameObject obj)
        {
            Color color = obj.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            color.a = alphaValue;
            obj.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
        }

    }
}
