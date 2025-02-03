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
        Vector2 firstPos;
        public Button buildConfirmButton;
        bool value = true;
        public float gridSize;

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
                    Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mouseWorldPos.z = 0;

                    float gridX = Mathf.Round(mouseWorldPos.x / gridSize) * gridSize;
                    float gridY = Mathf.Round(mouseWorldPos.y / gridSize) * gridSize;

                    firstPos = transform.position - new Vector3(gridX, gridY, 0);
                }

            }

            if (Input.GetMouseButton(0) && value)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0;
                float gridX = Mathf.Round(mouseWorldPos.x / gridSize) * gridSize;
                float gridY = Mathf.Round(mouseWorldPos.y / gridSize) * gridSize;
                transform.position = firstPos + new Vector2(gridX, gridY);
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
            if (gameObject.name == "Preview_Fence2x2(Clone)")
                return "fence2x2";
            else
            {
                Debug.Log("Preview name not found ");
                return "";
            }
        }
    }
}