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
            if (UIManager.Instance.buildPreview && ıInput.GetButtonUp0)
            {
                UIManager.Instance.buildPreview = false;
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
                            Build(archerHouse);
                            break;
                        case "TowerPreview":
                            Build(archerHouse);
                            break;
                        case "CastlePreview":
                            Build(archerHouse);
                            break;
                        case "FencePreview":
                            Build(archerHouse);
                            break;
                    }
                }

            }


        }

        void Build(GameObject building)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(building, pos, Quaternion.identity);
        }

    }
}