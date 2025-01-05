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
        [SerializeField] LayerMask unitLayer;
        [SerializeField] LayerMask interactableLayers;
        public GameObject interactedObj;
        public GameObject interactedKnight;
        public GameObject interactedMine;
        public GameObject interactedTree;
        public GameObject interactedSheep;
        public GameObject interactedFences;
        public GameObject interactedTower;
        public GameObject interactedConstruction;
        IInput ıInput;
        Interact ınteract;

        public List<GameObject> selectedUnits;

        GameObject currentUnit;
        public Vector2 startPos;
        Vector2 endPos;
        public bool isDragging;
        float time;
        bool clicked;

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
            ınteract = new Interact(this, interactableLayers);
            ıInput = new PcInput();

        }
        private void Update()
        {
            //SelectOneByOne();
            SelectMultiple();
            ClearSelectedObjs();
            GiveOrder();
            if (ıInput.GetButtonDown0)
            {
                // mouse ile tıklanan obje ile etkileşime girilir
                ınteract.InteractClickedObj();
                if (interactedObj != null)
                {
                    // Etkileşim olan obje, birim ise,
                    if (interactedObj.layer == 6)
                        interactedKnight = interactedObj;

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

                    // Etkileşim olan obje, kule ise,
                    if (interactedObj.layer == 9)
                        interactedTower = interactedObj;

                    // Etkileşim olan obje, inşaat ise,
                    if (interactedObj.layer == 30)
                        interactedConstruction = interactedObj;

                }
            }
            if (ıInput.GetButtonUp0)
            {
                interactedObj = null;
                interactedMine = null;
                interactedTree = null;
                interactedSheep = null;
                interactedFences = null;
                interactedKnight = null; //follow AI da boşa düşürüldü
                interactedTower = null;
                interactedConstruction = null;
            }


        }

        public bool CheckUIElements()
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
                    currentUnit = hit.collider.gameObject;
                    KnightController kC = currentUnit.GetComponent<KnightController>();
                    kC.unitOrderEnum = KnightManager.Instance.unitOrderEnum;
                    kC.workOnce = true;
                    kC.followingObj = null;
                    kC.isSeleceted = true;

                    // Aynı birimi tekrar diziye atma
                    if (!selectedUnits.Contains(currentUnit))
                        selectedUnits.Add(currentUnit);

                    // Seçili birimi vurgula
                    SelectedObjColor(0.5f, currentUnit);

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
                    currentUnit = hits[i].gameObject;

                    // Aynı nesneyi tekrar diziye atma
                    if (!selectedUnits.Contains(currentUnit))
                        selectedUnits.Add(currentUnit);
                    KnightController uC = selectedUnits[i].gameObject.GetComponent<KnightController>();
                    uC.unitOrderEnum = KnightManager.Instance.unitOrderEnum;
                    uC.workOnce = true;
                    uC.followingObj = null;
                    uC.isSeleceted = true;

                    // Seçili birimi vurgula
                    SelectedObjColor(0.5f, currentUnit);
                }

            }
        }
        void GiveOrder()
        {
            if (ıInput.GetButtonDown0)
            {
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    selectedUnits[i].GetComponent<KnightController>().unitOrderEnum = KnightManager.Instance.unitOrderEnum;
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
                    selectedUnits[i].GetComponent<KnightController>().isSeleceted = false;
                }

                selectedUnits.Clear();
            }


            // Hareket emrinden önce çalışmamamsı 5 frame için bekletilir.
            if (!ıInput.GetKeyLShift && ıInput.GetButtonDown0)
                clicked = true;

            if (clicked)
            {
                if (time > Time.deltaTime * 5) // 5 Frame bekle
                {
                    for (int i = 0; i < selectedUnits.Count; i++)
                    {
                        SelectedObjColor(1, selectedUnits[i]);
                        selectedUnits[i].GetComponent<KnightController>().isSeleceted = false;
                    }

                    selectedUnits.Clear();
                    time = 0;
                    clicked = false;
                }
                time += Time.deltaTime;
            }

            // Sadece köylüleri siler
            if (ıInput.GetButtonDown0)
            {
                int j = 0;
                int selectedUnitsCount = selectedUnits.Count;
                for (int i = 0; i < selectedUnitsCount; i++)
                {
                    if (selectedUnits[j].GetComponent<KnightController>().troopType == TroopTypeEnum.Villager)
                    {
                        SelectedObjColor(1, selectedUnits[j]);
                        selectedUnits.RemoveAt(j);
                    }
                    else
                        j++;
                }
            }



        }
        public void SelectedObjColor(float alphaValue, GameObject obj)
        {
            Color color = obj.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            color.a = alphaValue;
            obj.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
        }

    }
}
