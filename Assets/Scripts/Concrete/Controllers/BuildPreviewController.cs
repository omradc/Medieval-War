using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Names;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Concrete.Controllers
{
    public class BuildPreviewController : MonoBehaviour
    {
        [SerializeField] float gridSize = 0.2f;
        [SerializeField] Button buildConfirmButton;
        [SerializeField] Transform OrderInLayerSpriteAnchor;

        [Header("Sprites")]
        [SerializeField] GameObject visual;
        [SerializeField] GameObject visualRed;
        [SerializeField] GameObject wallHorizontal;
        [SerializeField] GameObject wallHorizontalRed;
        [SerializeField] GameObject wallVertical;
        [SerializeField] GameObject wallVerticalRed;
        [SerializeField] GameObject wallOne;
        [SerializeField] GameObject wallOneRed;
        [SerializeField] GameObject wallDoor;
        [SerializeField] GameObject wallDoorRed;

        Vector2 firstPos;
        BoxCollider2D coll;
        List<Vector3> wallsPos;
        ObjNames names;
        [HideInInspector] public int index;
        ValueController valueController;
        IInput ınput;
        bool obj;
        bool value = true;
        private void Awake()
        {
            names = new(gameObject.name);
            ınput = new MobileInput();
            coll = GetComponent<BoxCollider2D>();
            wallsPos = new List<Vector3>();
        }
        private void Start()
        {
            valueController = UIManager.Instance.valuePanel.GetComponent<ValueController>();

            InitializeWallPreview();
        }
        void Update()
        {
            if (ınput.GetButtonDown0())
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

            if (ınput.GetButtonDown0() && value)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0;
                float gridX = Mathf.Round(mouseWorldPos.x / gridSize) * gridSize;
                float gridY = Mathf.Round(mouseWorldPos.y / gridSize) * gridSize;
                transform.position = firstPos + new Vector2(gridX, gridY);
            }

            if (ınput.GetButtonUp0())
            {
                value = false;
                UIManager.Instance.canDragPreviewObj = false;
            }

        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 11 || collision.CompareTag("Buildable"))// duvar ise, inşa edilebilir
                obj = true;
            else
                obj = false;
            if (!obj)
            {
                // İnşaa edilemez
                buildConfirmButton.interactable = false;
                visual.gameObject.SetActive(false);
                visualRed.gameObject.SetActive(true);
            }

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
            wallHorizontal.SetActive(false);
            wallVertical.SetActive(false);
            wallOne.SetActive(false);
            wallDoor.SetActive(false);
            wallHorizontalRed.SetActive(false);
            wallVerticalRed.SetActive(false);
            wallOneRed.SetActive(false);
            wallDoorRed.SetActive(false);
            index++;

            if (index == 4)
                index = 0;

            switch (index)
            {
                case 0:
                    wallHorizontal.SetActive(true);
                    wallHorizontalRed.SetActive(true);
                    SetColliderSizeForWalls(1.15f, 0.1f, 0, 0.4f);
                    break;
                case 1:
                    wallVertical.SetActive(true);
                    wallVerticalRed.SetActive(true);
                    SetColliderSizeForWalls(0.35f, 0.73f, 0, 0.9f);
                    break;
                case 2:
                    wallOne.SetActive(true);
                    wallOneRed.SetActive(true);
                    SetColliderSizeForWalls(0.35f, 0.1f, 0, 0.4f);
                    break;
                case 3:
                    wallDoor.SetActive(true);
                    wallDoorRed.SetActive(true);
                    SetColliderSizeForWalls(1.15f, 0.1f, 0, 0.4f);
                    break;
            }
        }
        void InitializeWallPreview()
        {
            if (gameObject.name == "Preview_Wall(Clone)")
            {
                switch (index)
                {
                    case 0: // down_3x
                        wallHorizontal.SetActive(true);
                        wallHorizontalRed.SetActive(true);
                        SetColliderSizeForWalls(1.175f, 0.15f, 0, 0.374f);
                        break;
                    case 1: // top_1x
                        wallVertical.SetActive(true);
                        wallVerticalRed.SetActive(true);
                        SetColliderSizeForWalls(0.375f, .95f, 0, 0.8f);
                        break;
                    case 2: // down_1x
                        wallOne.SetActive(true);
                        wallOneRed.SetActive(true);
                        SetColliderSizeForWalls(.375f, .15f, 0, 0.375f);
                        break;
                    case 3: // door
                        wallDoor.SetActive(true);
                        wallDoorRed.SetActive(true);
                        SetColliderSizeForWalls(1.175f, 0.95f);
                        break;
                }
            }
        }
        public void BuildConfirmButton()
        {
            if (ResourcesManager.Instance.Buy(names.PreviewBuildingName(index), valueController))
            {
                UIManager.Instance.canBuild = true;

                wallsPos.Add(transform.position);
                if (wallsPos.Count == 3)
                    wallsPos.Remove(wallsPos[0]);

                StartCoroutine(WallBuildDelay()); // Butona basınca hareket edip yeni bir konuma inşaa yapılmasını engellemek için

                if (gameObject.name != "Preview_Wall(Clone)")
                {
                    UIManager.Instance.BuildPanelVisibility(true);
                    Destroy(gameObject);
                }
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
                case 0: // Yatay Duvar
                    if (wallsPos.Count == 2) //En az 2 duvar varsa
                        CalculateNextWallPos(1.2f, .2f);
                    break;
                case 1: // Dikey Duvar
                    if (wallsPos.Count == 2) //En az 2 duvar varsa
                        CalculateNextWallPos(.4f, 1);
                    break;
                case 2: // Tekli Duvar
                    if (wallsPos.Count == 2) //En az 2 duvar varsa
                    {
                        CalculateNextWallPos(.4f, .2f);
                    }
                    break;
                case 3: // Kapı
                    if (wallsPos.Count == 2) //En az 2 duvar varsa
                        CalculateNextWallPos(1.2f, .2f);
                    break;
            }

        }
        void CalculateNextWallPos(float distanceX, float distanceY)
        {
            if (wallsPos[0].x < wallsPos[1].x)
                transform.position += new Vector3(distanceX, 0, 0);
            else if (wallsPos[0].x > wallsPos[1].x)
                transform.position -= new Vector3(distanceX, 0, 0);
            else if (wallsPos[0].y < wallsPos[1].y)
                transform.position += new Vector3(0, distanceY, 0);
            else if (wallsPos[0].y > wallsPos[1].y)
                transform.position -= new Vector3(0, distanceY, 0);
        }
    }
}