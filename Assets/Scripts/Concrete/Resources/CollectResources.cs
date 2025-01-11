using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectResources
    {
        VillagerController vC;
        PathFindingController pF;
        public CollectResources(VillagerController collectResourcesController, PathFindingController pF)
        {
            vC = collectResourcesController;
            this.pF = pF;
        }

        public void SelectResourceType()
        {
            // UPDATE İLE ÇALIŞIR
            // Eğer köylü seçiliyse ve hedefe tıkladıysa, seçili köylünün hedefi seçili hedeftir
            if (vC.kC.isSeleceted)
            {
                vC.targetResource = null;
                vC.nearestTree = null;
                vC.constructionObj = null;
                vC.villagerSpriteRenderer.enabled = true;
                vC.isMine = false;
                vC.isMineEmpty = false;
                vC.isTree = false;
                vC.isSheep = false;
                vC.returnFences = false;
                vC.isFirstTree = false;
                if (vC.constructController != null)
                    vC.constructController.isFull = false;
                // Bir yere tıklandıysa
                if (InteractManager.Instance.interactedObj != null)
                {
                    vC.kC.isSeleceted = false;
                }
                // Maden
                if (InteractManager.Instance.interactedMine != null)
                {
                    // SADECE 1 KEZ ÇALIŞIR
                    vC.isTree = false;
                    vC.isSheep = false;
                    vC.returnHome = false;
                    vC.targetResource = InteractManager.Instance.interactedMine;
                    vC.mine = vC.targetResource.GetComponent<MineController>();
                    vC.isMine = true;
                    vC.kC.isSeleceted = false;
                }
                // Ağaç
                if (InteractManager.Instance.interactedTree != null)
                {
                    // SADECE 1 KEZ ÇALIŞIR
                    vC.isMine = false;
                    vC.isSheep = false;
                    vC.returnHome = false;
                    vC.targetResource = InteractManager.Instance.interactedTree;
                    vC.isTree = true;
                    vC.workOnceForTree = true;
                    vC.tCollect = 0;
                    vC.kC.isSeleceted = false;
                    vC.isFirstTree = true;
                }
                //Koyun
                if (InteractManager.Instance.interactedSheep != null)
                {
                    // SADECE 1 KEZ ÇALIŞIR

                    vC.isMine = false;
                    vC.isTree = false;
                    vC.targetResource = InteractManager.Instance.interactedSheep;
                    vC.sheepController = vC.targetResource.GetComponent<SheepController>();
                    vC.isSheep = true;
                    vC.tCollect = 0;
                    vC.kC.isSeleceted = false;
                }
                if (InteractManager.Instance.interactedFences != null)
                {
                    // SADECE 1 KEZ ÇALIŞIR
                    vC.isMine = false;
                    vC.isTree = false;
                    vC.fenceObj = InteractManager.Instance.interactedFences;
                    vC.fence = vC.fenceObj.GetComponent<FenceController>();

                }
                if (InteractManager.Instance.interactedConstruction != null)
                {
                    // SADECE 1 KEZ ÇALIŞIR
                    vC.constructionObj = InteractManager.Instance.interactedConstruction;
                    vC.constructController = vC.constructionObj.GetComponent<ConstructController>();

                }

            }
        }
        public void GoToHome()
        {
            // Köylüyü madende çalışırken maden biterse, tekarar görünür olur
            if (vC.isMineEmpty)
                vC.villagerSpriteRenderer.enabled = true;

            if (vC.returnHome)
            {
                // Eve ulaşınca dur
                if (Vector2.Distance(vC.transform.position, vC.homePos) > .5f)
                {
                    // Kaynak al
                    CollectResource();
                }

                // Eve ulaşıldı
                else
                {
                    DropResourceToHome();
                    vC.returnHome = false;
                    vC.workOnceForTree = true;
                }
            }
        }
        public void CollectResource()
        {
            if (vC.isTree)
                vC.tCollect += 1;
            if (vC.isSheep)
                vC.tCollect += 1;

            if (vC.workOnce)
            {
                if (vC.isMine)
                {
                    MineController mine = vC.targetResource.GetComponent<MineController>();
                    if (mine.CompareTag("GoldMine"))
                        vC.goldIdle.SetActive(true);
                    if (mine.CompareTag("RockMine"))
                        vC.rockIdle.SetActive(true);
                    pF.MoveAI(vC.homePos);
                    AnimationManager.Instance.RunCarryAnim(vC.animator, 1);
                    vC.workOnce = false;
                }
                if (vC.tCollect > vC.woodCollectTime && vC.isTree)
                {
                    vC.woodIdle.SetActive(true);
                    pF.MoveAI(vC.homePos);
                    AnimationManager.Instance.RunCarryAnim(vC.animator, 1);
                    vC.workOnce = false;
                    vC.tCollect = 0;

                }
                if (vC.tCollect > vC.meatCollectTime && vC.sheepController)
                {
                    vC.meatIdle.SetActive(true);
                    pF.MoveAI(vC.homePos);
                    AnimationManager.Instance.RunCarryAnim(vC.animator, 1);
                    vC.workOnce = false;
                    vC.tCollect = 0;
                }
            }

            if (!vC.workOnce && !vC.rockIdle.activeSelf && !vC.goldIdle.activeSelf && !vC.woodIdle.activeSelf && !vC.meatIdle.activeSelf)
                vC.returnHome = false;
        }
        void DropResourceToHome() // Kaynakları depola
        {
            if (vC.workOnce2)
            {
                if (vC.goldIdle.activeSelf)
                {
                    ResourcesManager.Instance.totalGold += ResourcesManager.Instance.collectGoldAmount;
                    DropGold(vC.homePos, vC.dropResourceLifeTime);
                    vC.goldIdle.SetActive(false);
                }
                if (vC.rockIdle.activeSelf)
                {
                    ResourcesManager.Instance.totalRock += ResourcesManager.Instance.collectRockAmount;
                    DropRock(vC.homePos, vC.dropResourceLifeTime);
                    vC.rockIdle.SetActive(false);
                }
                if (vC.woodIdle.activeSelf)
                {
                    ResourcesManager.Instance.totalWood += ResourcesManager.Instance.collectWoodAmount;
                    DropWood(vC.homePos, vC.dropResourceLifeTime);
                    vC.woodIdle.SetActive(false);
                }
                if (vC.meatIdle.activeSelf)
                {
                    ResourcesManager.Instance.totalMeat += vC.sheepController.currentMeatAmount;
                    DropMeat(vC.homePos, vC.dropResourceLifeTime);
                    vC.meatIdle.SetActive(false);
                    vC.isSheep = false;
                }

                AnimationManager.Instance.IdleAnim(vC.animator);
                ResourcesManager.Instance.DisplayResources();
                vC.workOnce2 = false;
            }
        }
        public void ReadyToNextCommand()
        {
            // Eğer elinde kaynak varken seçip, başka bir yere gönderirsen. Kaynak yere düşer.
            if (vC.ıInput.GetButtonDown0 && vC.kC.isSeleceted)
            {
                DropAnyResources();
            }

        }

        public void DropAnyResources()
        {
            if (vC.goldIdle.activeSelf)
            {
                DropGold(vC.transform.position, vC.dropResourceLifeTime);
                vC.goldIdle.SetActive(false);
            }
            if (vC.rockIdle.activeSelf)
            {
                DropRock(vC.transform.position, vC.dropResourceLifeTime);
                vC.rockIdle.SetActive(false);
            }
            if (vC.woodIdle.activeSelf)
            {
                DropWood(vC.transform.position, vC.dropResourceLifeTime);
                vC.woodIdle.SetActive(false);
            }
            if (vC.meatIdle.activeSelf)
            {
                DropMeat(vC.transform.position, vC.dropResourceLifeTime);
                vC.meatIdle.SetActive(false);
            }
            AnimationManager.Instance.IdleAnim(vC.animator);
        }

        void DropWood(Vector3 pos, float lifeTime)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject wood = Object.Instantiate(vC.resourceWood, pos + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0), Quaternion.identity);
                Object.Destroy(wood, lifeTime);
            }
        }
        void DropGold(Vector3 pos, float lifeTime)
        {
            GameObject gold = Object.Instantiate(vC.resourceGold, pos, Quaternion.identity);
            Object.Destroy(gold, lifeTime);
        }
        void DropRock(Vector3 pos, float lifeTime)
        {
            GameObject rock = Object.Instantiate(vC.resourceRock, pos, Quaternion.identity);
            Object.Destroy(rock, lifeTime);
        }
        void DropMeat(Vector3 pos, float lifeTime)
        {
            GameObject meat = Object.Instantiate(vC.resourceMeat, pos, Quaternion.identity);
            Object.Destroy(meat, lifeTime);
        }
    }
}
