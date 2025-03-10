﻿using Assets.Scripts.Concrete.Controllers;
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
        public GameObject repoConstructionBlue;
        public GameObject fence2x2;
        public GameObject wallHorizontal;
        public GameObject wallVertical;
        public GameObject wallOne;
        public GameObject wallDoor;


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
                    case "Preview_Fence2x2(Clone)":
                        Build(fence2x2);
                        break;
                    case "Preview_Repo(Clone)":
                        Build(repoConstructionBlue);
                        break;
                    case "Preview_Wall(Clone)":
                        switch (UIManager.Instance.previewObj.GetComponent<BuildPreviewController>().index)
                        {
                            case 0:
                                Build(wallHorizontal);
                                break;
                            case 1:
                                Build(wallVertical);
                                break;
                            case 2:
                                Build(wallOne);
                                break;
                            case 3:
                                Build(wallDoor);
                                break;
                        }
                        break;
                    default:
                        Debug.Log("Building Not Found");
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