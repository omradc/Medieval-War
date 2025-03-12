using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.SelectSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Concrete.Managers
{
    internal class InteractManager : MonoBehaviour
    {
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
        public GameObject interactedRepo;
        IInput ıInput;
        Interact ınteract;
        public List<GameObject> selectedKnights;

        public List<GameObject> save2;
        public List<GameObject> save3;
        public List<GameObject> save4;

        Vector2 startPos;
        Vector2 endPos;
        float time;
        bool clicked;
        bool openCloseDoor;

        public SavedFormation[] savedFormations;

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
            savedFormations = new SavedFormation[3];
        }
        private void Update()
        {
            SelectMultipleKnight();
            ClearSelectedKnights();
            GiveOrder();
            if (ıInput.GetButtonDown0())
            {
                // mouse ile tıklanan obje ile etkileşime girilir
                ınteract.InteractClickedObj();
                if (interactedObj != null)
                {
                    // Etkileşim olan obje, şovalye ise,
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
                    // Etkileşim olan obje, kapı ise,
                    if (interactedObj.layer == 11 && interactedObj.CompareTag("Door"))
                    {
                        interactedObj.transform.GetChild(2).GetChild(0).gameObject.SetActive(openCloseDoor);
                        openCloseDoor = !openCloseDoor;
                    }
                    if (interactedObj.layer == 10)
                    {
                        if (interactedObj.GetComponent<RepoController>().CanUseRepo())
                            interactedRepo = interactedObj;
                    }
                }
            }
            if (ıInput.GetButtonUp0())
            {
                interactedObj = null;
                interactedMine = null;
                interactedTree = null;
                interactedSheep = null;
                interactedFences = null;
                interactedKnight = null; //follow AI da boşa düşürüldü
                interactedTower = null;
                interactedConstruction = null;
                interactedRepo = null;
            }
        }

        public bool CheckUIElements() // Tıklanan yerde UI elemanı olup olmadığını sorgular. Sadece 1 kez kullan
        {
            PointerEventData eventData = new(EventSystem.current)
            {
                position = Input.mousePosition // Dokunmatik ekranlar için de geçerli
            };

            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(eventData, results);

            return results.Count > 0;
        }
        // Select
        public void SelectMultipleKnight()
        {
            //if (ıInput.GetButtonDown0())
            if (Input.GetMouseButtonDown(1))
            {
                startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            //if (ıInput.GetButtonUp0())
            if (Input.GetMouseButtonUp(1))
            {
                GameObject currentUnit;
                endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D[] hits = Physics2D.OverlapAreaAll(startPos, endPos, unitLayer);
                for (int i = 0; i < hits.Length; i++)
                {
                    currentUnit = hits[i].gameObject;

                    // Aynı nesneyi tekrar diziye atma
                    if (!selectedKnights.Contains(currentUnit))
                        selectedKnights.Add(currentUnit);
                    KnightController kC = selectedKnights[i].GetComponent<KnightController>();
                    kC.unitOrderEnum = KnightManager.Instance.unitOrderEnum;
                    kC.workOnce = true;
                    kC.followingObj = null;
                    kC.isSeleceted = true;

                }
                // Seçili birimi vurgula
                SelectedKnightsColor(0.5f);
                // Birimleri sıralar
                //selectedKnightControllers.Sort((a, b) => (a.factionType == FactionTypeEnum.Archer ? 1 : 0) - (b.factionType == FactionTypeEnum.Archer ? 1 : 0));
            }
        }
        void ClearSelectedKnights()
        {
            // Bütün seçili birimleri siler
            if (UIManager.Instance.isClearUnits)
            {
                for (int i = 0; i < selectedKnights.Count; i++)
                {
                    selectedKnights[i].GetComponent<KnightController>().isSeleceted = false;
                }
                SelectedKnightsColor(1f);
                selectedKnights.Clear();
                UIManager.Instance.isClearUnits = false;
            }

            //Hareket emrinden önce çalışmamamsı 5 frame için bekletilir.
            if (!UIManager.Instance.addUnitToggle.isOn && ıInput.GetButtonDown0())
                clicked = true;

            if (clicked)
            {
                if (time > Time.deltaTime * 5) // 5 Frame bekle
                {
                    for (int i = 0; i < selectedKnights.Count; i++)
                    {
                        selectedKnights[i].GetComponent<KnightController>().isSeleceted = false;
                    }
                    SelectedKnightsColor(1f);
                    selectedKnights.Clear();
                    time = 0;
                    clicked = false;
                }
                time += Time.deltaTime;
            }

            #region Sadece köylüleri siler
            //if (ıInput.GetButtonDown0)
            //{
            //    int j = 0;
            //    int selectedUnitsCount = selectedUnits.Count;
            //    for (int i = 0; i < selectedUnitsCount; i++)
            //    {
            //        if (selectedUnits[j].GetComponent<KnightController>().troopType == TroopTypeEnum.Villager)
            //        {
            //            SelectedObjColor(1, selectedUnits[j]);
            //            selectedUnits.RemoveAt(j);
            //        }
            //        else
            //            j++;
            //    }
            //}
            #endregion
        }
        public void SelectedKnightColor(float alphaValue, GameObject obj)
        {
            Color color = obj.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            color.a = alphaValue;
            obj.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
        }
        public void SelectedKnightsColor(float alphaValue)
        {
            for (int i = 0; i < selectedKnights.Count; i++)
            {
                Color color = selectedKnights[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                color.a = alphaValue;
                selectedKnights[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
            }
        }
        // Save
        public void SelectSavedFormation() // Kayıtlı Formasyonu seç
        {
            if (savedFormations[0].savedKnights.Count == 0) return; // kayıtlı birimler silinmeden yeni kayıt yapılamaz
            print("Selected");
            SelectedKnightsColor(1f);
            selectedKnights = new(savedFormations[0].savedKnights); // kayıtlı birimleri eşitle, deep copy
            KnightManager.Instance.knightFormation = savedFormations[0].knightFormation; // kayıtlı formasyonu eşitle
            SelectedKnightsColor(0.5f);

        }
        public void SaveFormation(int listNumber) // Seçili formasyonu kaydet
        {
            if (savedFormations[0].savedKnights != null && savedFormations[0].savedKnights.Count != 0) return;
            print("Saved");
            savedFormations[0] = new(new(selectedKnights), KnightManager.Instance.knightFormation); // seçili birimleri ve şimdiki formasyonu kaydet
        }
        public void ClearSavedFormation(int listNumber) // Kaydı sil
        {
            print("Clear");
            SelectedKnightsColor(1f);
            savedFormations[0].savedKnights.Clear();
            selectedKnights.Clear();
        }
        void GiveOrder()
        {
            if (ıInput.GetButtonDown0())
            {
                for (int i = 0; i < selectedKnights.Count; i++)
                {
                    selectedKnights[i].GetComponent<KnightController>().unitOrderEnum = KnightManager.Instance.unitOrderEnum;
                }
            }
        }

    }
    [System.Serializable]
    class SavedFormation
    {
        public List<GameObject> savedKnights;
        public KnightFormation knightFormation;
        public SavedFormation(List<GameObject> savedKnights, KnightFormation knightFormation)
        {
            this.savedKnights = savedKnights;
            this.knightFormation = knightFormation;
        }
    }
}
