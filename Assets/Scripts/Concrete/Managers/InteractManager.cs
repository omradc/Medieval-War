using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.SelectSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Concrete.Managers
{
    internal class InteractManager : MonoBehaviour
    {
        public GraphicRaycaster raycaster; // Sahnedeki UI'yi taramak için
        public EventSystem eventSystem;    // EventSystem'i kullanarak raycast işlemi yapmak için
        private PointerEventData pointerEventData; // Pointer verilerini tutar
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
        public GameObject interactedTower;
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
            GiveOrder();
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
                    {
                        Debug.Log("Interacted Sheep");
                        interactedSheep = interactedObj;
                    }

                    // Etkileşim olan obje, çitler ise,
                    if (interactedObj.layer == 12)
                        interactedFences = interactedObj;

                    // Etkileşim olan obje, kule ise,
                    if (interactedObj.layer == 9)
                        interactedTower = interactedObj;

                }
            }
            if (ıInput.GetButtonUp0)
            {
                interactedObj = null;
                interactedMine = null;
                interactedTree = null;
                interactedSheep = null;
                interactedFences = null;
                interactedUnit = null; //follow AI da boşa düşürüldü
                interactedTower = null;
            }


        }

        public bool CheckUIElemnts()
        {
            // Sol fare tuşuna basıldığında
            if (Input.GetMouseButtonDown(0))
            {
                // Pointer event datası oluşturuyoruz ve fare pozisyonunu alıyoruz
                pointerEventData = new PointerEventData(eventSystem);
                pointerEventData.position = Input.mousePosition;

                // Raycast sonuçlarını tutmak için bir liste
                List<RaycastResult> results = new List<RaycastResult>();

                // UI nesnelerine raycast yapıyoruz
                raycaster.Raycast(pointerEventData, results);

                // Eğer raycast sonucu varsa, bir UI nesnesine tıklanmıştır
                if (results.Count > 0)
                {
                    return true;
                }
            }

            return false;
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
                    uC.unitOrderEnum = UnitManager.Instance.unitOrderEnum;
                    uC.workOnce = true;
                    uC.followingObj = null;
                    uC.isSeleceted = true;

                    // Aynı birimi tekrar diziye atma
                    if (!selectedUnits.Contains(currentObj))
                        selectedUnits.Add(currentObj);

                    // Seçili birimi vurgula
                    SelectedObjColor(0.5f, currentObj);

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
                    uC.followingObj = null;
                    uC.isSeleceted = true;

                    // Seçili birimi vurgula
                    SelectedObjColor(0.5f, currentObj);
                }

            }
        }
        void GiveOrder()
        {
            if (ıInput.GetButtonDown0)
            {
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    selectedUnits[i].GetComponent<UnitController>().unitOrderEnum = UnitManager.Instance.unitOrderEnum;
                }
            }
        }
        void ClearSelectedObjs()
        {
            // Bütün seçili birimleri siler
            if (ıInput.GetKeyDownEscape)
            {
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    SelectedObjColor(1, selectedUnits[i]);
                    selectedUnits[i].GetComponent<UnitController>().isSeleceted = false;
                }

                selectedUnits.Clear();
            }

            // Sadece köylüleri siler
            if (ıInput.GetButtonDown0)
            {
                int j = 0;
                int selectedUnitsCount = selectedUnits.Count;
                for (int i = 0; i < selectedUnitsCount; i++)
                {
                    if (selectedUnits[j].GetComponent<UnitController>().unitTypeEnum == UnitTypeEnum.Villager)
                    {
                        SelectedObjColor(1, selectedUnits[j]);
                        selectedUnits.RemoveAt(j);
                    }
                    else
                        j++;

                }

            }

        }
        void SelectedObjColor(float alphaValue, GameObject obj)
        {
            Color color = obj.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            color.a = alphaValue;
            obj.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
        }

    }
}
