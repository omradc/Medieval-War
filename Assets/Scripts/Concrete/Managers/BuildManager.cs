using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Inputs;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Concrete.Managers
{
    public class BuildManager : MonoBehaviour
    {
        public static BuildManager Instance { get; private set; }
        public GameObject houseConstruct;
        public GameObject towerConstruct;
        public GameObject castleConstruct;

        public GameObject warriorHouse;
        public GameObject archerHouse;
        public GameObject pawnHouse;
        public GameObject tower;
        public GameObject castle;
        public GameObject fence4x4;


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

        // Update is called once per frame
        void Update()
        {
            if (UIManager.Instance.canBuild)
            {
                UIManager.Instance.canBuild = false;
                if (UIManager.Instance.previewObj == null) return;
                switch (UIManager.Instance.previewObj.name)
                {
                    case "Preview_PawnHouse(Clone)":
                        Build(houseConstruct).GetComponent<ConstructController>().building = pawnHouse;
                        break;
                    case "Preview_WarriorHouse(Clone)":
                        Build(houseConstruct).GetComponent<ConstructController>().building = warriorHouse;
                        break;
                    case "Preview_ArcherHouse(Clone)":
                        Build(houseConstruct).GetComponent<ConstructController>().building = archerHouse;
                        break;
                    case "Preview_Tower(Clone)":
                        Build(towerConstruct).GetComponent<ConstructController>().building = tower;
                        break;
                    case "Preview_Castle(Clone)":
                        Build(castleConstruct).GetComponent<ConstructController>().building = castle;
                        break;
                    case "Preview_Fence4x4(Clone)":
                        Build(fence4x4);
                        break;
                }


            }


        }

        GameObject Build(GameObject construct)
        {
            Vector2 pos = UIManager.Instance.previewObj.transform.position;
            return Instantiate(construct, pos, Quaternion.identity);
        }

    }
}