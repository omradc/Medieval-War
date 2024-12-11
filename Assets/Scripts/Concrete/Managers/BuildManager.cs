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
        public GameObject houseConstructionPawnBlue;
        public GameObject houseConstructionWarriorBlue;
        public GameObject houseConstructionArcherBlue;
        public GameObject towerConstructionBlue;
        public GameObject castleConstructionBlue;
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
                        Build(houseConstructionPawnBlue);
                        break;
                    case "Preview_WarriorHouse(Clone)":
                        Build(houseConstructionWarriorBlue);
                        break;
                    case "Preview_ArcherHouse(Clone)":
                        Build(houseConstructionArcherBlue);
                        break;
                    case "Preview_Tower(Clone)":
                        Build(towerConstructionBlue);
                        break;
                    case "Preview_Castle(Clone)":
                        Build(castleConstructionBlue);
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