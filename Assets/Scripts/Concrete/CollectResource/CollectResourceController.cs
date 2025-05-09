﻿using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.CollectResource
{
    class CollectResourceController : MonoBehaviour
    {
        public GameObject targetResource;

        [Header("REPO")]
        public GameObject repo;
        public RepoController repoController;
        public bool goRepo;
        public float dropResourceLifeTime = 1;

        [Header("MİNE")]
        public float miningTime;
        public GameObject resourceGold;
        public GameObject resourceRock;
        public float currentTime = 0f;
        public bool collect;
        public MineController mineControler;

        [Header("SHEEP")]
        public float meatCollectTime;
        public GameObject resourceMeat;

        [Header("TREE")]
        public GameObject resourceWood;

        [Header("CONSTRUCTION")]
        public float buildSpeed;
        public GameObject constructionObj;
        public ConstructController constructController;


        Transform resourceDropPoint;
        [HideInInspector] public KnightController kC;
        [HideInInspector] public PathFinding pF;
        [HideInInspector] public SpriteRenderer pawnSpriteRenderer;
        [HideInInspector] public Animator animator;
        [HideInInspector] public GameObject goldIdle;
        [HideInInspector] public GameObject rockIdle;
        [HideInInspector] public GameObject woodIdle;
        [HideInInspector] public GameObject meatIdle;
        AnimationEventController animationEventController;
        public bool workOnce;

        IInput ınput;
        private void Awake()
        {
            kC = GetComponent<KnightController>();
            pF = GetComponent<PathFinding>();
            pawnSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            ınput = new PcInput();
            animator = transform.GetChild(0).GetComponent<Animator>();
            goldIdle = transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
            rockIdle = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
            woodIdle = transform.GetChild(0).GetChild(0).GetChild(2).gameObject;
            meatIdle = transform.GetChild(0).GetChild(0).GetChild(3).gameObject;
            animationEventController = transform.GetChild(0).GetComponent<AnimationEventController>();
        }

        public void UpdatePawnWork()
        {
            SelectResourceType();
            PawnHandStatus();
        }
        public void SelectResourceType()
        {
            // UPDATE İLE ÇALIŞIR
            if (kC.isSeleceted) // Eğer köylü seçiliyse ve hedefe tıkladıysa, seçili köylünün hedefi seçili hedeftir
            {
                if (workOnce) // Köylü seçiliyken onu durdurur
                {
                    pF.agent.ResetPath(); // Yol bulma işlemi sıfırlanır
                    workOnce = false;
                }

                if (InteractManager.Instance.interactedMine != null) // Maden
                {
                    targetResource = InteractManager.Instance.interactedMine;
                    mineControler = targetResource.GetComponent<MineController>();
                    kC.isSeleceted = false;
                    workOnce = true;
                }

                else if (InteractManager.Instance.interactedRepo != null) // Depo
                {
                    repo = InteractManager.Instance.interactedRepo;
                    repoController = repo.GetComponent<RepoController>();
                    resourceDropPoint = repo.transform.GetChild(0);
                    kC.isSeleceted = false;
                    workOnce = true;
                    //if (goldIdle.activeSelf || rockIdle.activeSelf || woodIdle.activeSelf || meatIdle.activeSelf)
                    //{
                    //    kC.isSeleceted = false;
                    //    goRepo = true;
                    //}
                }

                else if (InteractManager.Instance.interactedObj == null && ınput.GetButtonUp0()) // Hedef yoksa
                {
                    targetResource = null;   // Hedefi sıfırla
                    mineControler = null;    // Maden kontrolcüsünü sıfırla
                    kC.isSeleceted = false;  // Seçili köylüyü sıfırla
                    workOnce = true;         // Tek seferlik çalıştırma işlemini sıfırla
                    goRepo = false;          // Depoya gitme işlemini sıfırla
                }
            }
        }
        public void CollectOre() // Maden Topla
        {
            if (targetResource != null && !goRepo && !kC.isSeleceted && mineControler.currentMineAmount != 0) // Hedef varsa ve depoya gitmiyorsa ve köylü seçili değilse ve madenin miktarı 0 değilse
            {
                if ((targetResource.transform.position - transform.position).magnitude > .1f) // Hedefe ulaşınca dur
                {
                    print("Go Mine");
                    pF.MoveAI(targetResource.transform.position, 0);
                }
                else
                {
                    pawnSpriteRenderer.enabled = false;

                    currentTime += 1;
                    if (currentTime >= miningTime)
                    {
                        // Madenden alınan kaynakları eksilt
                        if (targetResource.CompareTag("GoldMine"))
                        {
                            mineControler.currentMineAmount -= ResourcesManager.Instance.collectGoldAmount;
                            mineControler.mineAmountFillValue.fillAmount = mineControler.currentMineAmount / mineControler.mineAmount;
                        }
                        if (targetResource.CompareTag("RockMine"))
                        {
                            mineControler.currentMineAmount -= ResourcesManager.Instance.collectRockAmount;
                            mineControler.mineAmountFillValue.fillAmount = mineControler.currentMineAmount / mineControler.mineAmount;
                        }

                        currentTime = 0;
                        goRepo = true;
                    }
                }
            }
        }
        public void GoRepo() // Depoya git
        {
            if (goRepo && !kC.isSeleceted)
            {
                print("Go Repo0");
                pawnSpriteRenderer.enabled = true;
                PawnHandResourceVisibility(true);
                pF.MoveAI(resourceDropPoint.position);
                AnimationManager.Instance.RunCarryAnim(animator, 1);

                if ((resourceDropPoint.position - transform.position).magnitude < 0.1f)  // Depaya ulaşıldığında kaynakları bırak
                {
                    DropResourceToRepo();
                    goRepo = false;
                }
            }
        }
        public void PawnHandStatus()
        {
            //if (ınput.GetButtonDown0() && kC.isSeleceted)
            //{
            //    DropAnyResources();
            //}
        }
        public void PawnIdleCarryStateAnim() // Köylü dururken kaynak taşıma animasyonu
        {
            if (goldIdle.activeSelf || rockIdle.activeSelf || woodIdle.activeSelf || meatIdle.activeSelf)
            {
                if (kC.isSeleceted)
                    AnimationManager.Instance.IdleCarryAnim(animator);
            }
        }
        void PawnHandResourceVisibility(bool visibility) // Köylünün elindeki kaynakların görünürlüğünü ayarla
        {
            if (mineControler != null)
            {
                if (mineControler.CompareTag("GoldMine"))
                    goldIdle.SetActive(visibility);
                if (mineControler.CompareTag("RockMine"))
                    rockIdle.SetActive(visibility);
            }
        }
        void DropResourceToRepo() // Kaynakları depola
        {
            if (goldIdle.activeSelf)
            {
                ResourcesManager.Instance.totalGold += ResourcesManager.Instance.collectGoldAmount; // Toplam altın miktarını arttır
                repoController.gold += ResourcesManager.Instance.collectGoldAmount; // Depodaki altın miktarını arttır
                DropGold(resourceDropPoint.position, dropResourceLifeTime);
                goldIdle.SetActive(false);
            }
            if (rockIdle.activeSelf)
            {
                ResourcesManager.Instance.totalRock += ResourcesManager.Instance.collectRockAmount;
                repoController.rock += ResourcesManager.Instance.collectRockAmount;
                DropRock(resourceDropPoint.position, dropResourceLifeTime);
                rockIdle.SetActive(false);
            }
            //if (woodIdle.activeSelf)
            //{
            //    ResourcesManager.Instance.totalWood += ResourcesManager.Instance.collectWoodAmount;
            //    repoController.wood += ResourcesManager.Instance.collectWoodAmount;
            //    DropWood(repo.transform.GetChild(0).position, dropResourceLifeTime);
            //    woodIdle.SetActive(false);
            //}
            //if (meatIdle.activeSelf)
            //{
            //    ResourcesManager.Instance.totalMeat += sheepController.currentMeatAmount;
            //    repoController.meat += ResourcesManager.Instance.collectMeatAmount;
            //    DropMeat(repo.transform.GetChild(0).position, dropResourceLifeTime);
            //    meatIdle.SetActive(false);
            //    isSheep = false;
            //}
            CheckRepo();
            AnimationManager.Instance.IdleAnim(animator);
            ResourcesManager.Instance.DisplayResources();

        }
        void DropAnyResources() // Elindeki kaynakları yere bırak
        {
            if (goldIdle.activeSelf)
            {
                DropGold(transform.position, dropResourceLifeTime);
                goldIdle.SetActive(false);
            }
            else if (rockIdle.activeSelf)
            {
                DropRock(transform.position, dropResourceLifeTime);
                rockIdle.SetActive(false);
            }
            else if (woodIdle.activeSelf)
            {
                DropWood(transform.position, dropResourceLifeTime);
                woodIdle.SetActive(false);
            }
            else if (meatIdle.activeSelf)
            {
                DropMeat(transform.position, dropResourceLifeTime);
                meatIdle.SetActive(false);
            }
            AnimationManager.Instance.IdleAnim(animator);
        }
        void DropWood(Vector3 pos, float lifeTime) // Odun bırak
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject wood = Instantiate(resourceWood, pos + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0), Quaternion.identity);
                Destroy(wood, lifeTime);
            }
        }
        void DropGold(Vector3 pos, float lifeTime) // Altın bırak
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject gold = Instantiate(resourceGold, pos + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0), Quaternion.identity);
                Destroy(gold, lifeTime);
            }
        }
        void DropRock(Vector3 pos, float lifeTime) // Taş bırak
        {
            GameObject rock = Instantiate(resourceRock, pos, Quaternion.identity);
            Destroy(rock, lifeTime);
        }
        void DropMeat(Vector3 pos, float lifeTime) // Et bırak
        {
            GameObject meat = Object.Instantiate(resourceMeat, pos, Quaternion.identity);
            Destroy(meat, lifeTime);
        }
        void CheckRepo() // Depo kullanılabiliyor mu kontrol et
        {
            if (!repoController.CanUseRepo())
            {
                repo = null;
                repoController = null;
            }
        }
    }
}