using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.Managers;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Concrete.Controllers
{
    public class BuildPreviewController : MonoBehaviour
    {
        GameObject visual;
        GameObject visualRed;
        IInput ıInput;
        Vector2 firstPos;
        public Button buildConfirmButton;
        bool value = true;
        private void Awake()
        {
            ıInput = new PcInput();
        }
        private void Start()
        {
            visual = transform.GetChild(0).gameObject;
            visualRed = transform.GetChild(1).gameObject;
        }
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!InteractManager.Instance.CheckUIElements() || UIManager.Instance.canDragPreviewObj)
                {
                    value = true;
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    firstPos = transform.position - mousePos;
                }

            }

            if (Input.GetMouseButton(0) && value)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = firstPos + mousePos;
            }

            if (Input.GetMouseButtonUp(0))
            {
                value = false;
                UIManager.Instance.canDragPreviewObj = false;
            }

        }

        public void BuildConfirmButton()
        {
            if (ResourcesManager.Instance.Buy(BuildingName()))
            {
                UIManager.Instance.canBuild = true;
                UIManager.Instance.BuildPanelVisibility(true);
                Destroy(gameObject);
            }

            else
            {
                Debug.Log("CAN'T BUY");
            }
        }
        public void BuildCancelButton()
        {
            UIManager.Instance.BuildPanelVisibility(true);
            Destroy(gameObject);
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            // İnşaa edilemez
            buildConfirmButton.interactable = false;
            visual.gameObject.SetActive(false);
            visualRed.gameObject.SetActive(true);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            // İnşaa edilebilir
            buildConfirmButton.interactable = true;
            visual.gameObject.SetActive(true);
            visualRed.gameObject.SetActive(false);
        }
        string BuildingName()
        {
            if (gameObject.name == "Preview_PawnHouse(Clone)")
                return "pawnHouseLvl1";
            if (gameObject.name == "Preview_WarriorHouse(Clone)")
                return "warriorHouseLvl1";
            if (gameObject.name == "Preview_ArcherHouse(Clone)")
                return "archerHouseLvl1";
            if (gameObject.name == "Preview_Tower(Clone)")
                return "towerLvl1";
            if (gameObject.name == "Preview_Castle(Clone)")
                return "castleLvl1";
            if (gameObject.name == "Preview_Fence4x4(Clone)")
                return "fence4x4";
            else
                return "";
        }
    }
}