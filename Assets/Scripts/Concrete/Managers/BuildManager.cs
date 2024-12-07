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

        public GameObject worriorHouse;
        public GameObject archerHouse;
        public GameObject pawnHouse;
        public GameObject tower;
        public GameObject castle;
        public GameObject fence;


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
                switch (UIManager.Instance.previewObj.tag)
                {
                    case "WorriorHousePreview":
                        Build(houseConstruct).GetComponent<ConstructController>().building = worriorHouse;
                        break;
                    case "ArcherHousePreview":
                        Build(houseConstruct).GetComponent<ConstructController>().building = archerHouse;
                        break;
                    case "PawnHousePreview":
                        Build(houseConstruct).GetComponent<ConstructController>().building = pawnHouse;
                        break;
                    case "TowerPreview":
                        Build(towerConstruct).GetComponent<ConstructController>().building = tower;
                        break;
                    case "CastlePreview":
                        Build(castleConstruct).GetComponent<ConstructController>().building = castle;
                        break;
                    case "FencePreview":
                        Build(fence);
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