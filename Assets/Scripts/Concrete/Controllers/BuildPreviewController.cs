using Assets.Scripts.Concrete.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Concrete.Controllers
{
    public class BuildPreviewController : MonoBehaviour
    {
        public Button buildConfirmButton;
        public float gridSize;
        bool value = true;
        Vector2 firstPos;
        GameObject visual;
        GameObject visualRed;
        GameObject down_3x;
        GameObject top_1x;
        GameObject down_1x;
        GameObject door;
        GameObject down_3xRed;
        GameObject top_1xRed;
        GameObject down_1xRed;
        GameObject doorRed;
        [HideInInspector] public int index;
        BoxCollider2D coll;
        public List<Vector3> wallsPos;

        private void Start()
        {
            coll = GetComponent<BoxCollider2D>();
            visual = transform.GetChild(0).gameObject;
            down_3x = transform.GetChild(0).GetChild(0).gameObject;
            top_1x = transform.GetChild(0).GetChild(1).gameObject;
            down_1x = transform.GetChild(0).GetChild(2).gameObject;
            door = transform.GetChild(0).GetChild(3).gameObject;

            visualRed = transform.GetChild(1).gameObject;
            down_3xRed = transform.GetChild(1).GetChild(0).gameObject;
            top_1xRed = transform.GetChild(1).GetChild(1).gameObject;
            down_1xRed = transform.GetChild(1).GetChild(2).gameObject;
            doorRed = transform.GetChild(1).GetChild(3).gameObject;
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
        public void BuildChangeButton()
        {
            down_3x.SetActive(false);
            top_1x.SetActive(false);
            down_1x.SetActive(false);
            door.SetActive(false);
            down_3xRed.SetActive(false);
            top_1xRed.SetActive(false);
            down_1xRed.SetActive(false);
            doorRed.SetActive(false);
            index++;

            if (index == 4)
                index = 0;

            switch (index)
            {
                case 0: // down_3x
                    down_3x.SetActive(true);
                    down_3xRed.SetActive(true);
                    SetColliderSizeForWalls(1.175f, 0.95f);
                    break;
                case 1: // top_1x
                    top_1x.SetActive(true);
                    top_1xRed.SetActive(true);
                    SetColliderSizeForWalls(0.375f, .95f, 0, 0.8f);
                    break;
                case 2: // down_1x
                    down_1x.SetActive(true);
                    down_1xRed.SetActive(true);
                    SetColliderSizeForWalls(.375f, .15f, 0, 0.375f);
                    break;
                case 3: // door
                    door.SetActive(true);
                    doorRed.SetActive(true);
                    SetColliderSizeForWalls(1.175f, 0.95f);
                    break;
            }
        }
        public void BuildConfirmButton()
        {
            if (ResourcesManager.Instance.Buy(BuildingName()))
            {
                UIManager.Instance.canBuild = true;
                UIManager.Instance.BuildPanelVisibility(true);

                wallsPos.Add(transform.position);
                if (wallsPos.Count == 3)
                    wallsPos.Remove(wallsPos[0]);

                StartCoroutine(WallBuildDelay()); // Butona basınca hareket edip yeni bir konuma inşaa yapılmasını engellemek için
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
            if (gameObject.name == "Preview_Wall(Clone)")
                switch (index)
                {
                    case 0:
                        return "Down_3x";
                    case 1:
                        return "Top_1x";
                    case 2:
                        return "Down_1x";
                    case 3:
                        return "Door";
                    default:
                        return "";
                }
            else
            {
                Debug.Log("Preview name not found ");
                return "";
            }
        }
        void SetColliderSizeForWalls(float sizeX, float sizeY, float offsetX = 0, float offsetY = 0)
        {
            coll.size = new Vector2(sizeX, sizeY);
            coll.offset = new Vector2(offsetX, offsetY);
        }
        IEnumerator WallBuildDelay()
        {
            yield return new WaitForSeconds(.1f);
            SetWallPosAfterBuild();
        }
        void SetWallPosAfterBuild()
        {
            switch (index)
            {
                case 0: // Yatay Duvar 3x
                    if (wallsPos.Count == 2) //En az 2 duvar varsa
                    {
                        float distance = 1.2f;
                        if (wallsPos[0].x < wallsPos[1].x)
                            transform.position += new Vector3(distance, 0, 0);
                        if (wallsPos[0].x > wallsPos[1].x)
                            transform.position -= new Vector3(distance, 0, 0);
                    }
                    break;
                case 1: // Dikey Duvar
                    if (wallsPos.Count == 2) //En az 2 duvar varsa
                    {
                        float distanceY = 1f;
                        if (wallsPos[0].y < wallsPos[1].y)
                            transform.position += new Vector3(0, distanceY, 0);
                        if (wallsPos[0].y > wallsPos[1].y)
                            transform.position -= new Vector3(0, distanceY, 0);
                    }
                    break;
                case 2: // Tekli Duvar
                    if (wallsPos.Count == 2) //En az 2 duvar varsa
                    {
                        float distanceX = .4f;
                        if (wallsPos[0].x < wallsPos[1].x)
                            transform.position += new Vector3(distanceX, 0, 0);
                        if (wallsPos[0].x > wallsPos[1].x)
                            transform.position -= new Vector3(distanceX, 0, 0);
                    }
                    break;
                case 3: // Kapı
                    if (wallsPos.Count == 2) //En az 2 duvar varsa
                    {
                        float distanceX = 1.2f;
                        if (wallsPos[0].x < wallsPos[1].x)
                            transform.position += new Vector3(distanceX, 0, 0);
                        if (wallsPos[0].x > wallsPos[1].x)
                            transform.position -= new Vector3(distanceX, 0, 0);
                    }
                    break;
            }

        }
    }
}