using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera")]
        private Camera cam;
        [SerializeField] float dragMoveSpeed = 0.1f;
        [SerializeField] float touchMoveSpeed = 0.1f;
        [SerializeField] float pinchZoomSpeed = 0.02f;
        public float minZoom = 2;
        public float maxZoom = 20;

        [Header("Setup")]
        public bool dragMove;
        public bool touchMove;
        public float borderThickness = 10;
        float prevDistance;
        float currentDistance;
        Touch touch0;
        Touch touch1;
        Touch touch2;

        //[Header("PC")]
        //public float zoomSpeedWheel = 150;
        //public float zoomSpeedKey = 30;
        //bool fixedCamera;
        //float zoom;
        //Vector3 firstPos = new Vector3(0, 0, -10);
        //public float cameraSpeed = 30;
        private void Awake()
        {
            cam = GetComponent<Camera>();
        }
        void Start()
        {
            //zoom = Camera.main.orthographicSize;
        }
        void Update()
        {
            //PCControl();
            MobileControl();
        }


        void MobileControl()
        {
            PinchZoom();
            DragMove();
            TouchMove();
        }
        void PinchZoom()
        {
            if (Input.touchCount == 2) // Eğer iki parmak ekrana dokunuyorsa
            {
                touch0 = Input.GetTouch(0);
                touch1 = Input.GetTouch(1);

                prevDistance = (touch0.position - touch0.deltaPosition - (touch1.position - touch1.deltaPosition)).magnitude;
                currentDistance = (touch0.position - touch1.position).magnitude;
                cam.orthographicSize += (prevDistance - currentDistance) * pinchZoomSpeed * Time.deltaTime;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
            }
        }
        void DragMove()
        {
            if (Input.touchCount == 2 && dragMove)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                    transform.Translate(-touch.deltaPosition.x * SetSpeedByZoom(dragMoveSpeed) * Time.deltaTime, -touch.deltaPosition.y * SetSpeedByZoom(dragMoveSpeed) * Time.deltaTime, 0);
            }
        }
        void TouchMove()
        {
            if (Input.touchCount == 1 && touchMove)
            {
                touch2 = Input.GetTouch(0);

                if (touch2.phase == TouchPhase.Stationary)
                {
                    //Up
                    if (touch2.position.y >= Screen.height - borderThickness * 2)
                        transform.Translate(Vector2.up * SetSpeedByZoom(touchMoveSpeed) * Time.deltaTime);
                    //Down
                    if (touch2.position.y <= borderThickness * 2)
                        transform.Translate(Vector2.down * SetSpeedByZoom(touchMoveSpeed) * Time.deltaTime);
                    //Right
                    if (touch2.position.x >= Screen.width - borderThickness)
                        transform.Translate(Vector2.right * SetSpeedByZoom(touchMoveSpeed) * Time.deltaTime);
                    //Left
                    if (touch2.position.x <= borderThickness)
                        transform.Translate(Vector2.left * SetSpeedByZoom(touchMoveSpeed) * Time.deltaTime);
                }



            }
        }
        float SetSpeedByZoom(float speed)
        {
            print(cam.orthographicSize * speed);
            return cam.orthographicSize * speed;
        }
        #region PC
        //void PCControl()
        //{
        //    ScrollZoom();
        //    MovementByKeys();
        //    MovementByMouse();
        //    GoWorldCenterWithKey();
        //}
        //void MovementByMouse()
        //{
        //    //// Esc Toggle
        //    //if (Input.GetKeyDown(KeyCode.Escape))
        //    //    fixedCamera = !fixedCamera;
        //    //if (!fixedCamera)
        //    //    return;

        //    //UP
        //    if (Input.mousePosition.y >= Screen.height - borderThickness)
        //        transform.Translate(Vector2.up * SetSpeedByZoom() * Time.deltaTime);
        //    //DOWN
        //    if (Input.mousePosition.y <= borderThickness)
        //        transform.Translate(Vector2.down * SetSpeedByZoom() * Time.deltaTime);
        //    //RIGHT
        //    if (Input.mousePosition.x >= Screen.width - borderThickness)
        //        transform.Translate(Vector2.right * SetSpeedByZoom() * Time.deltaTime);
        //    //LEFT
        //    if (Input.mousePosition.x <= borderThickness)
        //        transform.Translate(Vector2.left * SetSpeedByZoom() * Time.deltaTime);
        //}
        //void MovementByKeys()
        //{
        //    SetSpeedByZoom();
        //    //UP
        //    if (Input.GetKey(KeyCode.W))
        //        transform.Translate(Vector2.up * SetSpeedByZoom() * Time.deltaTime);
        //    //DOWN
        //    if (Input.GetKey(KeyCode.S))
        //        transform.Translate(Vector2.down * SetSpeedByZoom() * Time.deltaTime);
        //    //RIGHT
        //    if (Input.GetKey(KeyCode.D))
        //        transform.Translate(Vector2.right * SetSpeedByZoom() * Time.deltaTime);
        //    //LEFT
        //    if (Input.GetKey(KeyCode.A))
        //        transform.Translate(Vector2.left * SetSpeedByZoom() * Time.deltaTime);
        //}
        //void ScrollZoom()
        //{
        //    float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        //    // Zoom In Wheel
        //    if (scrollWheel > 0)
        //        zoom -= zoomSpeedWheel * Time.deltaTime;
        //    // Zoom In Key
        //    if (Input.GetKey(KeyCode.Q))
        //        zoom -= zoomSpeedKey * Time.deltaTime;
        //    // Zoom Out Wheel
        //    if (scrollWheel < 0)
        //        zoom += zoomSpeedWheel * Time.deltaTime;
        //    // Zoom Out Key
        //    if (Input.GetKey(KeyCode.E))
        //        zoom += zoomSpeedKey * Time.deltaTime;

        //    zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        //    Camera.main.orthographicSize = zoom;
        //}
        //void GoWorldCenterWithKey()
        //{
        //    if (Input.GetKeyDown(KeyCode.O))
        //        transform.position = firstPos;
        //}
        #endregion
    }
}