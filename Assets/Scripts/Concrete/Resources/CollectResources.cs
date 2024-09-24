using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Managers;
using UnityEngine;

namespace Assets.Scripts.Concrete.Resources
{
    internal class CollectResources
    {
        CollectResourcesController cR;

        public CollectResources(CollectResourcesController collectResourcesController)
        {
            cR = collectResourcesController;
        }

        public void SelectResourceType()
        {
            // UPDATE İLE ÇALIŞIR
            // Eğer köylü seçiliyse ve hedef kaynağa tıkladıysa, seçili köylünün hedefi seçili kaynaktır
            if (cR.uC.isSeleceted)
            {
                cR.targetResource = null;
                cR.nearestTree = null;
                cR.villagerSpriteRenderer.enabled = true;
                cR.isMine = false;
                cR.isMineEmpty = false;
                cR.isTree = false;
                cR.isSheep = false;

                // Maden
                if (InteractManager.Instance.interactedMine != null)
                {
                    // SADECE 1 KEZ ÇALIŞIR
                    cR.isTree = false;
                    cR.isSheep = false;
                    cR.returnHome = false;
                    cR.targetResource = InteractManager.Instance.interactedMine;
                    cR.isMine = true;
                    cR.uC.isSeleceted = false;

                }
                // Ağaç
                if (InteractManager.Instance.interactedTree != null)
                {
                    // SADECE 1 KEZ ÇALIŞIR
                    cR.isMine = false;
                    cR.isSheep = false;
                    cR.returnHome = false;
                    cR.targetResource = InteractManager.Instance.interactedTree;
                    cR.isTree = true;
                    cR.workOnce3 = true;
                    cR.uC.isSeleceted = false;
                }
                //Koyun
                if (InteractManager.Instance.interactedSheep != null)
                {
                    // SADECE 1 KEZ ÇALIŞIR

                    cR.isMine = false;
                    cR.isTree = false;
                    cR.targetResource = InteractManager.Instance.interactedSheep;
                    cR.sheep = cR.targetResource.GetComponent<Sheep>();
                    cR.isSheep = true;
                    cR.uC.isSeleceted = false;
                }
                if (InteractManager.Instance.interactedFences != null)
                {
                    // SADECE 1 KEZ ÇALIŞIR
                    cR.isMine = false;
                    cR.isTree = false;
                    cR.isSheep = false;
                    cR.fence = InteractManager.Instance.interactedFences;

                }
            }
        }
        public void GoToHome()
        {
            // Köylüyü madende çalışırken maden biterse, tekarar görünür olur
            if (cR.isMineEmpty)
                cR.villagerSpriteRenderer.enabled = true;

            if (cR.returnHome)
            {
                // Eve ulaşınca dur
                if (Vector2.Distance(cR.transform.position, cR.homePos) > .5f)
                {
                    cR.tCollect += Time.deltaTime;
                    if (cR.workOnce)
                    {
                        // Kaynak al
                        if (cR.tCollect > cR.collectTime && cR.isTree || cR.tCollect > cR.collectTime && cR.sheep)
                        {
                            cR.pF2D.AIGetMoveCommand(cR.homePos);
                            AnimationManager.Instance.RunCarryAnim(cR.animator, 1);
                            CollectResource();
                            cR.workOnce = false;
                            cR.tCollect = 0;

                        }
                        if (cR.isMine)
                        {
                            cR.pF2D.AIGetMoveCommand(cR.homePos);
                            AnimationManager.Instance.RunCarryAnim(cR.animator, 1);
                            CollectResource();
                            cR.workOnce = false;
                        }
                    }
                }

                // Eve ulaşıldı
                else
                {
                    DropResourceToHome();
                    cR.returnHome = false;
                    cR.workOnce3 = true;
                }
            }
        }
        public void CollectResource()
        {
            if (cR.isMine)
            {
                Mine mine = cR.targetResource.GetComponent<Mine>();
                if (mine.CompareTag("GoldMine"))
                    cR.goldIdle.SetActive(true);
                if (mine.CompareTag("RockMine"))
                    cR.rockIdle.SetActive(true);
            }
            if (cR.isTree)
                cR.woodIdle.SetActive(true);
            if (cR.isSheep)
                cR.meatIdle.SetActive(true);

        }
        void DropResourceToHome() // Kaynakları depola
        {
            if (cR.workOnce2)
            {
                if (cR.goldIdle.activeSelf)
                {
                    ResourcesManager.gold += cR.collectGoldAmount;
                    DropGold(cR.homePos, cR.dropResourceLifeTime);
                    cR.goldIdle.SetActive(false);
                }
                if (cR.rockIdle.activeSelf)
                {
                    ResourcesManager.rock += cR.collectRockAmount;
                    DropRock(cR.homePos, cR.dropResourceLifeTime);
                    cR.rockIdle.SetActive(false);
                }
                if (cR.woodIdle.activeSelf)
                {
                    ResourcesManager.wood += cR.collectWoodAmount;
                    DropWood(cR.homePos, cR.dropResourceLifeTime);
                    cR.woodIdle.SetActive(false);
                }
                if (cR.meatIdle.activeSelf)
                {
                    ResourcesManager.meat += cR.collectMeatCount;
                    DropMeat(cR.homePos, cR.dropResourceLifeTime);
                    cR.meatIdle.SetActive(false);
                }
                cR.workOnce2 = false;
            }
        }
        public void ReadyToNextCommand()
        {
            // Eğer elinde kaynak varken seçip, başka bir yere gönderirsen. Kaynak yere düşer.
            if (cR.ıInput.GetButtonDown0 && cR.uC.isSeleceted)
            {
                if (cR.goldIdle.activeSelf)
                {
                    DropGold(cR.transform.position, cR.dropResourceLifeTime);
                    cR.goldIdle.SetActive(false);
                }
                if (cR.rockIdle.activeSelf)
                {
                    DropRock(cR.transform.position, cR.dropResourceLifeTime);
                    cR.rockIdle.SetActive(false);
                }
                if (cR.woodIdle.activeSelf)
                {
                    DropWood(cR.transform.position, cR.dropResourceLifeTime);
                    cR.woodIdle.SetActive(false);
                }
                if (cR.meatIdle.activeSelf)
                {
                    DropMeat(cR.transform.position, cR.dropResourceLifeTime);
                    cR.meatIdle.SetActive(false);
                }
                // Köylü seçili iken, etkileşimli olmayan bir nesne seçildiğinde seçim kalkar 
                if (InteractManager.Instance.interactedObj == null)
                {
                    cR.uC.isSeleceted = false;
                }
            }
        }
        void DropWood(Vector3 pos, float lifeTime)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject wood = Object.Instantiate(cR.resourceWood, pos + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0), Quaternion.identity);
                Object.Destroy(wood, lifeTime);
            }
        }
        void DropGold(Vector3 pos, float lifeTime)
        {
            GameObject gold = Object.Instantiate(cR.resourceGold, pos, Quaternion.identity);
            Object.Destroy(gold, lifeTime);
        }
        void DropRock(Vector3 pos, float lifeTime)
        {
            GameObject rock = Object.Instantiate(cR.resourceRock, pos, Quaternion.identity);
            Object.Destroy(rock, lifeTime);
        }
        void DropMeat(Vector3 pos, float lifeTime)
        {
            GameObject meat = Object.Instantiate(cR.resourceMeat, pos, Quaternion.identity);
            Object.Destroy(meat, lifeTime);
        }
    }
}
