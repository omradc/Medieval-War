using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectResources
    {
        readonly PawnController pawnController;
        readonly PathFinding pF;
        IInput ınput;
        public CollectResources(PawnController pawnController, PathFinding pF)
        {
            new PcInput();
            this.pawnController = pawnController;
            this.pF = pF;
        }
        public void SelectResourceType()
        {
            // UPDATE İLE ÇALIŞIR
            if (pawnController.kC.isSeleceted) // Eğer köylü seçiliyse ve hedefe tıkladıysa, seçili köylünün hedefi seçili hedeftir
            {
                pawnController.targetResource = null;
                pawnController.nearestTree = null;
                pawnController.constructionObj = null;
                pawnController.villagerSpriteRenderer.enabled = true;
                pawnController.isMine = false;
                pawnController.isMineEmpty = false;
                pawnController.isTree = false;
                pawnController.isSheep = false;
                pawnController.returnFences = false;
                pawnController.isFirstTree = false;

                if (InteractManager.Instance.interactedObj != null) // Bir yere tıklandıysa
                {
                    pawnController.kC.isSeleceted = false;
                }
                if (InteractManager.Instance.interactedMine != null) // Maden
                {
                    pawnController.isTree = false;
                    pawnController.isSheep = false;
                    pawnController.returnHome = false;
                    pawnController.targetResource = InteractManager.Instance.interactedMine;
                    pawnController.mine = pawnController.targetResource.GetComponent<MineController>();
                    pawnController.isMine = true;
                    pawnController.kC.isSeleceted = false;
                }
                if (InteractManager.Instance.interactedTree != null) // Ağaç
                {
                    pawnController.isMine = false;
                    pawnController.isSheep = false;
                    pawnController.returnHome = false;
                    pawnController.targetResource = InteractManager.Instance.interactedTree;
                    pawnController.isTree = true;
                    pawnController.workOnceForTree = true;
                    pawnController.tCollect = 0;
                    pawnController.kC.isSeleceted = false;
                    pawnController.isFirstTree = true;
                }
                if (InteractManager.Instance.interactedSheep != null) // Koyun
                {
                    pawnController.isMine = false;
                    pawnController.isTree = false;
                    pawnController.targetResource = InteractManager.Instance.interactedSheep;
                    pawnController.sheepController = pawnController.targetResource.GetComponent<SheepController>();
                    pawnController.isSheep = true;
                    pawnController.tCollect = 0;
                    pawnController.kC.isSeleceted = false;
                }
                if (InteractManager.Instance.interactedFences != null) // Çit
                {
                    pawnController.isMine = false;
                    pawnController.isTree = false;
                    pawnController.fenceObj = InteractManager.Instance.interactedFences;
                    pawnController.fence = pawnController.fenceObj.GetComponent<FenceController>();

                }
                if (InteractManager.Instance.interactedConstruction != null) // İnşaat
                {
                    pawnController.constructionObj = InteractManager.Instance.interactedConstruction;
                    pawnController.constructController = pawnController.constructionObj.GetComponent<ConstructController>();
                }
                if (InteractManager.Instance.interactedRepo != null)
                {
                    pawnController.repo = InteractManager.Instance.interactedRepo;
                }
            }
        }
        public void GoToHome()
        {
            // Köylüyü madende çalışırken maden biterse, tekarar görünür olur
            if (pawnController.isMineEmpty)
                pawnController.villagerSpriteRenderer.enabled = true;

            if (pawnController.returnHome)
            {
                //FindRepo();
                // Eve ulaşınca dur
                if (Vector2.Distance(pawnController.transform.position, pawnController.repo.transform.GetChild(0).position) > .5f)
                {
                    // Kaynak al
                    CollectResource();
                }

                // Eve ulaşıldı
                else
                {
                    DropResourceToHome();
                    pawnController.returnHome = false;
                    pawnController.workOnceForTree = true;
                }
            }
        }
        public void CollectResource()
        {
            if (pawnController.isTree)
                pawnController.tCollect += 1;
            if (pawnController.isSheep)
                pawnController.tCollect += 1;

            if (pawnController.workOnce)
            {
                if (pawnController.isMine)
                {
                    MineController mine = pawnController.targetResource.GetComponent<MineController>();
                    if (mine.CompareTag("GoldMine"))
                        pawnController.goldIdle.SetActive(true);
                    if (mine.CompareTag("RockMine"))
                        pawnController.rockIdle.SetActive(true);
                    pF.MoveAI(pawnController.repo.transform.GetChild(0).position, 0);
                    AnimationManager.Instance.RunCarryAnim(pawnController.animator, 1);
                    pawnController.workOnce = false;
                }
                if (pawnController.tCollect > pawnController.woodCollectTime && pawnController.isTree)
                {
                    pawnController.woodIdle.SetActive(true);
                    pF.MoveAI(pawnController.repo.transform.GetChild(0).position, 0);
                    AnimationManager.Instance.RunCarryAnim(pawnController.animator, 1);
                    pawnController.workOnce = false;
                    pawnController.tCollect = 0;

                }
                if (pawnController.tCollect > pawnController.meatCollectTime && pawnController.sheepController)
                {
                    pawnController.meatIdle.SetActive(true);
                    pF.MoveAI(pawnController.repo.transform.GetChild(0).position, 0);
                    AnimationManager.Instance.RunCarryAnim(pawnController.animator, 1);
                    pawnController.workOnce = false;
                    pawnController.tCollect = 0;
                }
            }

            if (!pawnController.workOnce && !pawnController.rockIdle.activeSelf && !pawnController.goldIdle.activeSelf && !pawnController.woodIdle.activeSelf && !pawnController.meatIdle.activeSelf)
                pawnController.returnHome = false;
        }
        void DropResourceToHome() // Kaynakları depola
        {
            if (pawnController.workOnce2)
            {
                RepoController repo = pawnController.repo.GetComponent<RepoController>();
                if (pawnController.goldIdle.activeSelf)
                {
                    ResourcesManager.Instance.totalGold += ResourcesManager.Instance.collectGoldAmount;
                    repo.gold += ResourcesManager.Instance.collectGoldAmount;
                    DropGold(pawnController.repo.transform.GetChild(0).position, pawnController.dropResourceLifeTime);
                    pawnController.goldIdle.SetActive(false);
                }
                if (pawnController.rockIdle.activeSelf)
                {
                    ResourcesManager.Instance.totalRock += ResourcesManager.Instance.collectRockAmount;
                    repo.rock += ResourcesManager.Instance.collectRockAmount;
                    DropRock(pawnController.repo.transform.GetChild(0).position, pawnController.dropResourceLifeTime);
                    pawnController.rockIdle.SetActive(false);
                }
                if (pawnController.woodIdle.activeSelf)
                {
                    ResourcesManager.Instance.totalWood += ResourcesManager.Instance.collectWoodAmount;
                    repo.wood += ResourcesManager.Instance.collectWoodAmount;
                    DropWood(pawnController.repo.transform.GetChild(0).position, pawnController.dropResourceLifeTime);
                    pawnController.woodIdle.SetActive(false);
                }
                if (pawnController.meatIdle.activeSelf)
                {
                    ResourcesManager.Instance.totalMeat += pawnController.sheepController.currentMeatAmount;
                    repo.meat += ResourcesManager.Instance.collectMeatAmount;
                    DropMeat(pawnController.repo.transform.GetChild(0).position, pawnController.dropResourceLifeTime);
                    pawnController.meatIdle.SetActive(false);
                    pawnController.isSheep = false;
                }
                CheckRepo();
                AnimationManager.Instance.IdleAnim(pawnController.animator);
                ResourcesManager.Instance.DisplayResources();
                pawnController.workOnce2 = false;
            }
        }
        public void ReadyToNextCommand()
        {
            // Eğer elinde kaynak varken seçip, başka bir yere gönderirsen. Kaynak yere düşer.
            if (ınput.GetButtonDown0() && pawnController.kC.isSeleceted)
            {
                DropAnyResources();
            }

        }
        public void DropAnyResources()
        {
            if (pawnController.goldIdle.activeSelf)
            {
                DropGold(pawnController.transform.position, pawnController.dropResourceLifeTime);
                pawnController.goldIdle.SetActive(false);
            }
            if (pawnController.rockIdle.activeSelf)
            {
                DropRock(pawnController.transform.position, pawnController.dropResourceLifeTime);
                pawnController.rockIdle.SetActive(false);
            }
            if (pawnController.woodIdle.activeSelf)
            {
                DropWood(pawnController.transform.position, pawnController.dropResourceLifeTime);
                pawnController.woodIdle.SetActive(false);
            }
            if (pawnController.meatIdle.activeSelf)
            {
                DropMeat(pawnController.transform.position, pawnController.dropResourceLifeTime);
                pawnController.meatIdle.SetActive(false);
            }
            AnimationManager.Instance.IdleAnim(pawnController.animator);
        }
        void DropWood(Vector3 pos, float lifeTime)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject wood = Object.Instantiate(pawnController.resourceWood, pos + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0), Quaternion.identity);
                Object.Destroy(wood, lifeTime);
            }
        }
        void DropGold(Vector3 pos, float lifeTime)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject gold = Object.Instantiate(pawnController.resourceGold, pos + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0), Quaternion.identity);
                Object.Destroy(gold, lifeTime);
            }
        }
        void DropRock(Vector3 pos, float lifeTime)
        {
            GameObject rock = Object.Instantiate(pawnController.resourceRock, pos, Quaternion.identity);
            Object.Destroy(rock, lifeTime);
        }
        void DropMeat(Vector3 pos, float lifeTime)
        {
            GameObject meat = Object.Instantiate(pawnController.resourceMeat, pos, Quaternion.identity);
            Object.Destroy(meat, lifeTime);
        }
        void CheckRepo()
        {
            if (!pawnController.repo.GetComponent<RepoController>().CanUseRepo())
                pawnController.repo = null;
        }
    }
}
