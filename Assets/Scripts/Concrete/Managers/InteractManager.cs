using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Enums;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.SelectSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

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
        public List<GameObject> targetImages;
        Vector2 startPos;
        Vector2 endPos;
        float time;
        bool openCloseDoor;
        float holdTreshold = 0.2f;
        public SavedFormation[] savedFormations;
        DrawLineRenderer drawLine;
        [SerializeField] Camera cam;
        public Transform formationPreview;
        [SerializeField] GameObject targetImage;

        private void Awake()
        {
            Singelton();
            drawLine = GetComponent<DrawLineRenderer>();
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
            selectedKnights = new();// Başlangıç ataması;
            savedFormations = new SavedFormation[4];
            for (int i = 0; i < savedFormations.Length; i++) // Başlangıç ataması;
            {
                savedFormations[i] = new SavedFormation(new(selectedKnights), KnightManager.Instance.knightFormation);
            }
            CreateTargetImage();
            InvokeRepeating(nameof(OptimumLineWidthnes), 0, 0.1f);
        }
        private void Update()
        {
            SelectMultipleKnight();
            SelectKnightWhitFaction();
            ClearSelectedKnights();
            GiveOrder();
            InteractableObjects();
            DrawFormationIndicator();
        }
        void InteractableObjects()
        {
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
        void SelectMultipleKnight()
        {
            if (ıInput.GetButtonDown0())
            {
                startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                drawLine.InitializeDrawSquare();
            }

            if (ıInput.GetButton0())
                drawLine.DrawSquare();

            if (ıInput.GetButtonUp0())
            {
                drawLine.ResetSquare();
                GameObject currentUnit;
                endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D[] hits = Physics2D.OverlapAreaAll(startPos, endPos, unitLayer);
                for (int i = 0; i < hits.Length; i++)
                {
                    currentUnit = hits[i].gameObject;

                    // Aynı nesneyi tekrar diziye atma
                    if (!selectedKnights.Contains(currentUnit))
                    {
                        selectedKnights.Add(currentUnit);
                    }
                }
                for (int i = 0; i < selectedKnights.Count; i++)
                {
                    SelectedKnightSetup(selectedKnights, i);
                }
            }
        }
        void SelectKnightWhitFaction()
        {
            if (interactedKnight != null)
            {
                time += Time.deltaTime;
                if (time > holdTreshold)
                {
                    FactionTypeEnum factionTypeEnum = interactedKnight.GetComponent<KnightController>().factionType;

                    if (factionTypeEnum == FactionTypeEnum.Warrior)
                    {
                        GameObject[] knights = GameObject.FindGameObjectsWithTag("Warrior");
                        for (int i = 0; i < knights.Length; i++)
                        {
                            if (!selectedKnights.Contains(knights[i]))
                            {
                                selectedKnights.Add(knights[i]);
                            }
                        }
                        for (int i = 0; i < selectedKnights.Count; i++)
                        {
                            SelectedKnightSetup(selectedKnights, i);
                        }
                    }
                    if (factionTypeEnum == FactionTypeEnum.Archer)
                    {
                        GameObject[] knights = GameObject.FindGameObjectsWithTag("Archer");
                        for (int i = 0; i < knights.Length; i++)
                        {
                            if (!selectedKnights.Contains(knights[i]))
                            {
                                selectedKnights.Add(knights[i]);
                            }
                        }
                        for (int i = 0; i < selectedKnights.Count; i++)
                        {
                            SelectedKnightSetup(selectedKnights, i);
                        }
                    }
                    if (factionTypeEnum == FactionTypeEnum.Pawn)
                    {
                        GameObject[] knights = GameObject.FindGameObjectsWithTag("Pawn");
                        for (int i = 0; i < knights.Length; i++)
                        {
                            if (!selectedKnights.Contains(knights[i]))
                            {
                                selectedKnights.Add(knights[i]);
                            }
                        }
                        for (int i = 0; i < selectedKnights.Count; i++)
                        {
                            SelectedKnightSetup(selectedKnights, i);
                        }
                    }

                    time = 0;
                }
            }
        }
        void SelectedKnightSetup(List<GameObject> knights, int i)
        {
            KnightController kC = knights[i].GetComponent<KnightController>();
            kC.unitOrderEnum = KnightManager.Instance.knightOrderEnum;
            kC.workOnce = true;
            kC.followingObj = null;
            kC.isSeleceted = true;
            SelectedKnightColor(.5f, knights[i]);
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
        public void SelectedKnightColor(float alphaValue, GameObject obj) // 1 Şovalye
        {
            Color color = obj.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            color.a = alphaValue;
            obj.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
        }
        void SelectedKnightsColor(float alphaValue) // Tüm şovalyeler
        {
            for (int i = 0; i < selectedKnights.Count; i++)
            {
                Color color = selectedKnights[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                color.a = alphaValue;
                selectedKnights[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
            }
        }
        public void SaveAndSelectFormation(int index) // Seçili formasyonu kaydet
        {
            if (selectedKnights.Count > 0) // seçili birim varsa kaydet
            {
                if (savedFormations[index].savedKnights.Count == 0)
                {
                    print("Saved");
                    UIManager.Instance.UpdateFormationButtonImage();
                    savedFormations[index] = new(new(selectedKnights), KnightManager.Instance.knightFormation); // seçili birimleri ve şimdiki formasyonu kaydet
                }
            }

            if (savedFormations[index].savedKnights != null)
            {
                if (savedFormations[index].savedKnights.Count > 0)
                {
                    print("Selected");
                    SelectedKnightsColor(1f);
                    selectedKnights = new(savedFormations[index].savedKnights); // kayıtlı birimleri eşitle, deep copy
                    for (int i = 0; i < savedFormations[index].savedKnights.Count; i++)
                    {
                        SelectedKnightSetup(savedFormations[index].savedKnights, i);
                    }
                    KnightManager.Instance.knightFormation = savedFormations[index].knightFormation; // kayıtlı formasyonu eşitle
                    UIManager.Instance.formationDropdown.value = (int)savedFormations[index].knightFormation; // formasyon  panelini de güncelle
                }
            }
        }
        public void ClearSavedFormation(int index) // Kaydı sil
        {
            print("Clear");
            UIManager.Instance.UpdateFormationButtonImage(true);
            SelectedKnightsColor(1f);
            foreach (var item in savedFormations[index].savedKnights)
            {
                item.GetComponent<KnightController>().isSeleceted = false;
            }
            savedFormations[index].savedKnights.Clear();
            selectedKnights.Clear();
        }
        void GiveOrder()
        {
            if (ıInput.GetButtonDown0())
            {
                for (int i = 0; i < selectedKnights.Count; i++)
                {
                    selectedKnights[i].GetComponent<KnightController>().unitOrderEnum = KnightManager.Instance.knightOrderEnum;
                }
            }
        }
        public void AddKnightModeStatus()
        {
            if (!UIManager.Instance.addKnightModeToggle.isOn)
            {
                for (int i = 0; i < selectedKnights.Count; i++)
                {
                    selectedKnights[i].GetComponent<KnightController>().isSeleceted = false;
                }
                SelectedKnightsColor(1f);
                selectedKnights.Clear();
            }
        }
        void DrawFormationIndicator()
        {
            if (selectedKnights.Count > 0 && UIManager.Instance.dynamicAngleModeToggle.isOn)
                drawLine.DrawFormationIndicator(cam);
            //KnightManager.Instance.move.FormationPreview(formationPreview, targetImage, KnightManager.Instance.distance);
        }
        public void CreateTargetImage()
        {
            for (int i = 0; i < 9; i++)
            {
                targetImages.Add(Instantiate(targetImage, formationPreview));
            }
        }
        void OptimumLineWidthnes()
        {
            drawLine.DynamicLineRendererWidthness();
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
            UIManager.Instance.formationDropdown.value = (int)knightFormation;
        }
    }
}
