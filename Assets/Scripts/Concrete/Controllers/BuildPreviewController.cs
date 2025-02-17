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
        [SerializeField] GameObject visual;
        [SerializeField] GameObject visualRed;
        [SerializeField] GameObject down_3x;
        [SerializeField] GameObject top_1x;
        [SerializeField] GameObject down_1x;
        [SerializeField] GameObject door;
        [SerializeField] GameObject down_3xRed;
        [SerializeField] GameObject top_1xRed;
        [SerializeField] GameObject down_1xRed;
        [SerializeField] GameObject doorRed;

        SpriteRenderer visualSprite;
        SpriteRenderer visualRedSprite;
        SpriteRenderer down_3xSprite;
        SpriteRenderer top_1xSprite;
        SpriteRenderer top_1xDownSprite;
        SpriteRenderer down_1xSprite;
        SpriteRenderer wallDoorSprite;
        SpriteRenderer doorSprite;
        SpriteRenderer down_3xRedSprite;
        SpriteRenderer top_1xRedSprite;
        SpriteRenderer top_1xRedDownSprite;
        SpriteRenderer down_1xRedSprite;
        SpriteRenderer wallDoorRedSprite;
        SpriteRenderer doorRedSprite;
        Vector2 firstPos;
        BoxCollider2D coll;
        List<Vector3> wallsPos;
        DynamicOrderInLayer dynamicOrderInLayer;
        ObjNames names;
        [HideInInspector] public int index;
        public ValueController valueController;

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
                down_3xSprite = down_3x.GetComponent<SpriteRenderer>();
                top_1xSprite = top_1x.transform.GetChild(0).GetComponent<SpriteRenderer>();
                top_1xDownSprite = top_1x.transform.GetChild(1).GetComponent<SpriteRenderer>();
                down_1xSprite = down_1x.GetComponent<SpriteRenderer>();
                wallDoorSprite = door.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
                doorSprite = door.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
                down_3xRedSprite = down_3xRed.GetComponent<SpriteRenderer>();
                top_1xRedSprite = top_1xRed.transform.GetChild(0).GetComponent<SpriteRenderer>();
                top_1xRedDownSprite = top_1xRed.transform.GetChild(1).GetComponent<SpriteRenderer>();
                down_1xRedSprite = down_1xRed.GetComponent<SpriteRenderer>();
                wallDoorRedSprite = doorRed.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
                doorRedSprite = doorRed.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
            }

            else
            {
                visualSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
                visualRedSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
            }

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
                    SetColliderSizeForWalls(1.175f, 0.15f, 0, 0.374f);
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
        void SetOrderInLayer()
        {


            if (gameObject.name == "Preview_Wall(Clone)")
            {
                dynamicOrderInLayer.OrderInLayerWithYPos(down_3xSprite.transform, down_3xSprite);
                dynamicOrderInLayer.OrderInLayerWithYPos(down_3xRedSprite.transform, down_3xRedSprite);
                dynamicOrderInLayer.OrderInLayerWithYPos(top_1xSprite.transform, top_1xSprite);
                dynamicOrderInLayer.OrderInLayerWithNumber(top_1xSprite.transform, top_1xDownSprite, -1);
                dynamicOrderInLayer.OrderInLayerWithNumber(top_1xRedSprite.transform, top_1xRedDownSprite, -1);
                dynamicOrderInLayer.OrderInLayerWithYPos(top_1xRedSprite.transform, top_1xRedSprite);
                dynamicOrderInLayer.OrderInLayerWithYPos(down_1xSprite.transform, down_1xSprite);
                dynamicOrderInLayer.OrderInLayerWithYPos(down_1xRedSprite.transform, down_1xRedSprite);
                dynamicOrderInLayer.OrderInLayerWithYPos(wallDoorSprite.transform, wallDoorSprite);
                dynamicOrderInLayer.OrderInLayerWithYPos(doorSprite.transform, doorSprite);
                dynamicOrderInLayer.OrderInLayerWithYPos(wallDoorRedSprite.transform, wallDoorRedSprite);
                dynamicOrderInLayer.OrderInLayerWithYPos(doorRedSprite.transform, doorRedSprite);
            }

            else
            {
                dynamicOrderInLayer.OrderInLayerWithYPos(visual.transform, visualSprite);
                dynamicOrderInLayer.OrderInLayerWithYPos(visualRed.transform, visualRedSprite);
            }
        }

    }
}