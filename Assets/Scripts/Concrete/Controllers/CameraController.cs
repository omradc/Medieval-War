using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera")]
        public float cameraSpeed = 30;
        public float currentCameraSpeed;
        
        [Header("Mobile")]
        private float prevDistance = 0f;
        private bool isPinching = false;

        [Header("PC")]
        public float zoomSpeedWheel = 150;
        public float zoomSpeedKey = 30;
        public float minZoom = 2;
        public float maxZoom = 20;
        public float borderThickness = 10;
        Vector3 firstPos = new Vector3(0, 0, -10);
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
            //PCControl();
            MobileControl();
        }

        void PCControl()
        {
            ScrollZoom();
            MovementByKeys();
            MovementByMouse();
            GoWorldCenterWithKey();
        }
        void MovementByMouse()
        {
            // Esc Toggle
            if (Input.GetKeyDown(KeyCode.Escape))
                fixedCamera = !fixedCamera;
            if (!fixedCamera)
                return;

            CalculateZoomSpeedFactor();
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
            CalculateZoomSpeedFactor();
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
        void ScrollZoom()
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
        void CalculateZoomSpeedFactor()
        {
            currentCameraSpeed = zoom * cameraSpeed;
        }
        void GoWorldCenterWithKey()
        {
            if (Input.GetKeyDown(KeyCode.O))
                transform.position = firstPos;
        }

        void MobileControl()
        {
            PinchZoom();
        }

        void PinchZoom()
        {
            if (Input.touchCount == 2) // İki parmak dokunuyorsa
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                float currentDistance = (touch0.position - touch1.position).magnitude;

                if (!isPinching) // İlk dokunma anında
                {
                    prevDistance = currentDistance; // Başlangıç mesafesini kaydet
                    isPinching = true; // Pinch işlemi başladı
                }
                else // Sürekli dokunma sırasında
                {
                    float zoomAmount = currentDistance - prevDistance; // Mesafe farkını al

                    if (zoomAmount > 0)
                        Debug.Log("Yakınlaştır! 🔍");
                    else if (zoomAmount < 0)
                        Debug.Log("Uzaklaştır! 📉");

                    prevDistance = currentDistance; // Önceki mesafeyi güncelle
                }
            }
            else
            {
                isPinching = false; // Parmaklar kaldırıldığında sıfırla
            }
        }
    }
}