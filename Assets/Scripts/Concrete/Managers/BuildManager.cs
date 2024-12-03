using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Inputs;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Concrete.Managers
{
    public class BuildManager : MonoBehaviour
    {
        public static BuildManager Instance { get; private set; }
        IInput ıInput;
        public GameObject worriorHouse;
        public GameObject archerHouse;
        public GameObject pawnHouse;
        public GameObject tower;
        public GameObject castle;
        public GameObject fence;

        private void Awake()
        {
            Singelton();
            ıInput = new PcInput();
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
            if (UIManager.Instance.buildConfirm)
            {
                UIManager.Instance.buildConfirm = false;
                if (UIManager.Instance.canBuild)
                {
                    switch (UIManager.Instance.previewObj.tag)
                    {
                        case "WorriorHousePreview":
                            Build(worriorHouse);
                            break;
                        case "ArcherHousePreview":
                            Build(archerHouse);
                            break;
                        case "PawnHousePreview":
                            Build(pawnHouse);
                            break;
                        case "TowerPreview":
                            Build(tower);
                            break;
                        case "CastlePreview":
                            Build(castle);
                            break;
                        case "FencePreview":
                            Build(fence);
                            break;
                    }
                }

            }


        }

        void Build(GameObject building)
        {
            Vector2 pos = UIManager.Instance.previewObj.transform.position;
            Instantiate(building, pos, Quaternion.identity);
        }

    }
}