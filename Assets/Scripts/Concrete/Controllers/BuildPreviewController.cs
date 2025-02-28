using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Movements;
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

        SpriteRenderer visualSprite;
        SpriteRenderer visualRedSprite;
        SpriteRenderer wallHorizontalSprite;
        SpriteRenderer wallHorizontalRedSprite;
        SpriteRenderer wallVerticalTopSprite;
        SpriteRenderer wallVerticalTopRedSprite;
        SpriteRenderer wallVerticalDownSprite;
        SpriteRenderer wallVerticalDownRedSprite;
        SpriteRenderer wallOneSprite;
        SpriteRenderer wallOneRedSprite;
        SpriteRenderer wallDoorSprite;
        SpriteRenderer wallDoorRedSprite;
        SpriteRenderer woodenDoorSprite;
        SpriteRenderer woodenDoorRedSprite;
        Vector2 firstPos;
        BoxCollider2D coll;
        List<Vector3> wallsPos;
        DynamicOrderInLayer dynamicOrderInLayer;
        ObjNames names;
        [HideInInspector] public int index;
        ValueController valueController;

        bool obj;
        bool anyObj;
        bool value = true;
        private void Awake()
        {
            names = new(gameObject.name);
            dynamicOrderInLayer = new();
            coll = GetComponent<BoxCollider2D>();
            wallsPos = new List<Vector3>();
        }
        private void Start()
        {
            valueController = UIManager.Instance.valuePanel.GetComponent<ValueController>();
            if (gameObject.name == "Preview_Wall(Clone)")
            {
                wallHorizontalSprite = wallHorizontal.GetComponent<SpriteRenderer>();
                wallHorizontalRedSprite = wallHorizontalRed.GetComponent<SpriteRenderer>();
                wallVerticalTopSprite = wallVertical.transform.GetChild(0).GetComponent<SpriteRenderer>();
                wallVerticalTopRedSprite = wallVerticalRed.transform.GetChild(0).GetComponent<SpriteRenderer>();
                wallVerticalDownSprite = wallVertical.transform.GetChild(1).GetComponent<SpriteRenderer>();
                wallVerticalDownRedSprite = wallVerticalRed.transform.GetChild(1).GetComponent<SpriteRenderer>();
                wallOneSprite = wallOne.GetComponent<SpriteRenderer>();
                wallOneRedSprite = wallOneRed.GetComponent<SpriteRenderer>();
                wallDoorSprite = wallDoor.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
                wallDoorRedSprite = wallDoorRed.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
                woodenDoorSprite = wallDoor.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
                woodenDoorRedSprite = wallDoorRed.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
            }

            else
            {
                visualSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
                visualRedSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
            }

            InitializeWallPreview();
        }
        void Update()
        {
            SetOrderInLayer();
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
            if (collision.gameObject.layer == 11 || collision.CompareTag("Buildable"))// duvar ise, inşa edilebilir
            {
                obj = true;
            }
            else
            {
                obj = false;
                anyObj = true;
            }
            if (!anyObj && obj) return;

            // İnşaa edilemez
            buildConfirmButton.interactable = false;
            visual.gameObject.SetActive(false);
            visualRed.gameObject.SetActive(true);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            anyObj = false;
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
            if (ResourcesManager.Instance.Buy(names.PrewiewBuildingName(index), valueController))
            {
                UIManager.Instance.canBuild = true;

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
        void SetOrderInLayer()
        {
            if (gameObject.name == "Preview_Wall(Clone)")
            {
                dynamicOrderInLayer.OrderInLayerUpdatePreview(OrderInLayerSpriteAnchor, wallHorizontalSprite);
                dynamicOrderInLayer.OrderInLayerUpdatePreview(OrderInLayerSpriteAnchor, wallHorizontalRedSprite);
                dynamicOrderInLayer.OrderInLayerUpdatePreview(OrderInLayerSpriteAnchor, wallVerticalTopSprite);
                dynamicOrderInLayer.OrderInLayerUpdatePreview(OrderInLayerSpriteAnchor, wallVerticalTopRedSprite);
                dynamicOrderInLayer.OrderInLayerUpdatePreview(OrderInLayerSpriteAnchor, wallVerticalDownSprite, -1);
                dynamicOrderInLayer.OrderInLayerUpdatePreview(OrderInLayerSpriteAnchor, wallVerticalDownRedSprite, -1);
                dynamicOrderInLayer.OrderInLayerUpdatePreview(OrderInLayerSpriteAnchor, wallOneSprite);
                dynamicOrderInLayer.OrderInLayerUpdatePreview(OrderInLayerSpriteAnchor, wallOneRedSprite);
                dynamicOrderInLayer.OrderInLayerUpdatePreview(OrderInLayerSpriteAnchor, wallDoorSprite);
                dynamicOrderInLayer.OrderInLayerUpdatePreview(OrderInLayerSpriteAnchor, wallDoorRedSprite);
                dynamicOrderInLayer.OrderInLayerUpdatePreview(OrderInLayerSpriteAnchor, woodenDoorSprite, -1);
                dynamicOrderInLayer.OrderInLayerUpdatePreview(OrderInLayerSpriteAnchor, woodenDoorRedSprite, -1);
            }

            else
            {
                dynamicOrderInLayer.OrderInLayerUpdate(OrderInLayerSpriteAnchor, visualSprite);
                dynamicOrderInLayer.OrderInLayerUpdate(OrderInLayerSpriteAnchor, visualRedSprite);
            }
        }

    }
}