using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    public class CameraController : MonoBehaviour
    {
        public float cameraSpeed = 30;
        public float currentCameraSpeed;
        public float zoomSpeedWheel = 150;
        public float zoomSpeedKey = 30;
        public float minZoom = 2;
        public float maxZoom = 20;
        public Vector3 firstPos = new Vector3(0, 0, -10);
        public float borderThickness = 10;
        bool fixedCamera;
        float zoom;
        // Use this for initialization
        void Start()
        {
            currentCameraSpeed = cameraSpeed;
            zoom = Camera.main.orthographicSize;
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.O))
                transform.position = firstPos;

            Zoom();
            MovementByKeys();

            //// Esc Toggle
            //if (Input.GetKeyDown(KeyCode.Escape))
            //    fixedCamera = !fixedCamera;
            //if (!fixedCamera)
            //    return;

            //MovementByMouse();

        }

        void MovementByMouse()
        {
            CalculateSpeedFactor();
            //UP
            if (Input.mousePosition.y >= Screen.height - borderThickness)
                transform.Translate(Vector2.up * currentCameraSpeed * Time.deltaTime);
            //DOWN
            if (Input.mousePosition.y <= borderThickness)
                transform.Translate(Vector2.down * currentCameraSpeed * Time.deltaTime);
            //RIGHT
            if (Input.mousePosition.x >= Screen.width - borderThickness)
                transform.Translate(Vector2.right * currentCameraSpeed * Time.deltaTime);
            //LEFT
            if (Input.mousePosition.x <= borderThickness)
                transform.Translate(Vector2.left * currentCameraSpeed * Time.deltaTime);
        }

        void MovementByKeys()
        {
            CalculateSpeedFactor();
            //UP
            if (Input.GetKey(KeyCode.W))
                transform.Translate(Vector2.up * currentCameraSpeed * Time.deltaTime);
            //DOWN
            if (Input.GetKey(KeyCode.S))
                transform.Translate(Vector2.down * currentCameraSpeed * Time.deltaTime);
            //RIGHT
            if (Input.GetKey(KeyCode.D))
                transform.Translate(Vector2.right * currentCameraSpeed * Time.deltaTime);
            //LEFT
            if (Input.GetKey(KeyCode.A))
                transform.Translate(Vector2.left * currentCameraSpeed * Time.deltaTime);
        }

        void CalculateSpeedFactor()
        {
            currentCameraSpeed = zoom * cameraSpeed;
        }
        void Zoom()
        {
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

            // Zoom In Wheel
            if (scrollWheel > 0)
                zoom -= zoomSpeedWheel * Time.deltaTime;
            // Zoom In Key
            if (Input.GetKey(KeyCode.Q))
                zoom -= zoomSpeedKey * Time.deltaTime;
            // Zoom Out Wheel
            if (scrollWheel < 0)
                zoom += zoomSpeedWheel * Time.deltaTime;
            // Zoom Out Key
            if (Input.GetKey(KeyCode.E))
                zoom += zoomSpeedKey * Time.deltaTime;

            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
            Camera.main.orthographicSize = zoom;
        }
    }
}