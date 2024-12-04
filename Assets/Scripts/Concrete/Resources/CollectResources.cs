using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectResources
    {
        VillagerController vC;

        public CollectResources(VillagerController collectResourcesController)
        {
            vC = collectResourcesController;
        }

        public void SelectResourceType()
        {
            // UPDATE İLE ÇALIŞIR
            // Eğer köylü seçiliyse ve hedefe tıkladıysa, seçili köylünün hedefi seçili hedeftir
            if (vC.uC.isSeleceted)
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
                // Bir yere tıklandıysa
                if (InteractManager.Instance.interactedObj != null)
                {
                    vC.uC.isSeleceted = false;
                }
                // Maden
                if (InteractManager.Instance.interactedMine != null)
                {
                    // SADECE 1 KEZ ÇALIŞIR
                    vC.isTree = false;
                    vC.isSheep = false;
                    vC.returnHome = false;
                    vC.targetResource = InteractManager.Instance.interactedMine;
                    vC.mine = vC.targetResource.GetComponent<Mine>();
                    vC.isMine = true;
                    vC.uC.isSeleceted = false;
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
                    vC.uC.isSeleceted = false;
                    vC.isFirstTree = true;
                }
                //Koyun
                if (InteractManager.Instance.interactedSheep != null)
                {
                    // SADECE 1 KEZ ÇALIŞIR

                    vC.isMine = false;
                    vC.isTree = false;
                    vC.targetResource = InteractManager.Instance.interactedSheep;
                    vC.sheep = vC.targetResource.GetComponent<Sheep>();
                    vC.isSheep = true;
                    vC.tCollect = 0;
                    vC.uC.isSeleceted = false;
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
                    Mine mine = vC.targetResource.GetComponent<Mine>();
                    if (mine.CompareTag("GoldMine"))
                        vC.goldIdle.SetActive(true);
                    if (mine.CompareTag("RockMine"))
                        vC.rockIdle.SetActive(true);
                    vC.pF2D.AIGetMoveCommand(vC.homePos);
                    AnimationManager.Instance.RunCarryAnim(vC.animator, 1);
                    vC.workOnce = false;
                }
                if (vC.tCollect > vC.woodCollectTime && vC.isTree)
                {
                    vC.woodIdle.SetActive(true);
                    vC.pF2D.AIGetMoveCommand(vC.homePos);
                    AnimationManager.Instance.RunCarryAnim(vC.animator, 1);
                    vC.workOnce = false;
                    vC.tCollect = 0;

                }
                if (vC.tCollect > vC.meatCollectTime && vC.sheep)
                {
                    vC.meatIdle.SetActive(true);
                    vC.pF2D.AIGetMoveCommand(vC.homePos);
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
                    ResourcesManager.gold += vC.collectGoldAmount;
                    DropGold(vC.homePos, vC.dropResourceLifeTime);
                    vC.goldIdle.SetActive(false);
                }
                if (vC.rockIdle.activeSelf)
                {
                    ResourcesManager.rock += vC.collectRockAmount;
                    DropRock(vC.homePos, vC.dropResourceLifeTime);
                    vC.rockIdle.SetActive(false);
                }
                if (vC.woodIdle.activeSelf)
                {
                    ResourcesManager.wood += vC.collectWoodAmount;
                    DropWood(vC.homePos, vC.dropResourceLifeTime);
                    vC.woodIdle.SetActive(false);
                }
                if (vC.meatIdle.activeSelf)
                {
                    ResourcesManager.meat += vC.collectMeatCount;
                    DropMeat(vC.homePos, vC.dropResourceLifeTime);
                    vC.meatIdle.SetActive(false);
                    vC.isSheep = false;
                }
                vC.workOnce2 = false;
            }
        }
        public void ReadyToNextCommand()
        {
            // Eğer elinde kaynak varken seçip, başka bir yere gönderirsen. Kaynak yere düşer.
            if (vC.ıInput.GetButtonDown0 && vC.uC.isSeleceted)
            {
                DropAnyResources();

                // köylü seçili iken, etkileşimli olmayan bir nesne seçildiğinde seçim kalkar 
                if (InteractManager.Instance.interactedObj == null)
                {
                    vC.uC.isSeleceted = false;
                }
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
