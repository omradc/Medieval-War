using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Inputs;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera")]
        private Camera cam;
        public float minZoom = 2;
        public float maxZoom = 20;

        [Header("Mobile")]
        [SerializeField] float dragMoveSpeed = 0.1f;
        [SerializeField] float touchMoveSpeed = 0.1f;
        [SerializeField] float pinchZoomSpeed = 0.02f;
        public float mobileBorderThickness = 150;
        public bool touchMove;
        public bool dragMove;


        [Header("PC")]
        public float keyMoveSpeed = 30;
        public float mouseMoveSpeed = 30;
        public float keyZoomSpeed = 30;
        public float scrollZoomSpeed = 150;
        public float pCBorderThickness = 10;
        
        //Setup
        float pCZoom;
        bool fixedCamera;
        float prevDistance;
        float currentDistance;
        PcInput pcInput;
        MobileInput mobileInput;
        Vector3 firstPos = new Vector3(0, 0, -10);
        
        private void Awake()
        {
            cam = GetComponent<Camera>();
            mobileInput = new();
            pcInput = new();
        }
        void Start()
        {
            pCZoom = Camera.main.orthographicSize;
        }
        void Update()
        {
            PCControl();
            //MobileControl();
        }

        #region Mobile
        void MobileControl()
        {
            PinchZoom();
            DragMove();
            TouchMove();
        }
        void PinchZoom()
        {
            if (mobileInput.Pinch(out Touch touch0, out Touch touch1))
            {
                prevDistance = (touch0.position - touch0.deltaPosition - (touch1.position - touch1.deltaPosition)).magnitude;
                currentDistance = (touch0.position - touch1.position).magnitude;
                cam.orthographicSize += (prevDistance - currentDistance) * pinchZoomSpeed * Time.deltaTime;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
            }
        }
        void DragMove()
        {
            if (mobileInput.DragMove(out Touch touch0, 2) && dragMove)
            {
                if (touch0.phase == TouchPhase.Moved)
                    transform.Translate(-touch0.deltaPosition.x * SetSpeedByZoom(dragMoveSpeed) * Time.deltaTime, -touch0.deltaPosition.y * SetSpeedByZoom(dragMoveSpeed) * Time.deltaTime, 0);
            }
        }
        void TouchMove()
        {
            if (mobileInput.DragMove(out Touch touch0, 1) && touchMove)
            {
                if (touch0.phase == TouchPhase.Stationary)
                {
                    if (touch0.position.y >= Screen.height - mobileBorderThickness * 2) //Up
                        transform.Translate(Vector2.up * SetSpeedByZoom(touchMoveSpeed) * Time.deltaTime);
                    if (touch0.position.y <= mobileBorderThickness * 2) //Down
                        transform.Translate(Vector2.down * SetSpeedByZoom(touchMoveSpeed) * Time.deltaTime);
                    if (touch0.position.x >= Screen.width - mobileBorderThickness) //Right
                        transform.Translate(Vector2.right * SetSpeedByZoom(touchMoveSpeed) * Time.deltaTime);
                    if (touch0.position.x <= mobileBorderThickness) //Left
                        transform.Translate(Vector2.left * SetSpeedByZoom(touchMoveSpeed) * Time.deltaTime);
                }
            }
        }
        #endregion
        #region PC
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

            //UP
            if (Input.mousePosition.y >= Screen.height - pCBorderThickness)
                transform.Translate(Vector2.up * SetSpeedByZoom(mouseMoveSpeed) * Time.deltaTime);
            //DOWN
            if (Input.mousePosition.y <= pCBorderThickness)
                transform.Translate(Vector2.down * SetSpeedByZoom(mouseMoveSpeed) * Time.deltaTime);
            //RIGHT
            if (Input.mousePosition.x >= Screen.width - pCBorderThickness)
                transform.Translate(Vector2.right * SetSpeedByZoom(mouseMoveSpeed) * Time.deltaTime);
            //LEFT
            if (Input.mousePosition.x <= pCBorderThickness)
                transform.Translate(Vector2.left * SetSpeedByZoom(mouseMoveSpeed) * Time.deltaTime);
        }
        void MovementByKeys()
        {
            //UP
            if (Input.GetKey(KeyCode.W))
                transform.Translate(Vector2.up * SetSpeedByZoom(keyMoveSpeed) * Time.deltaTime);
            //DOWN
            if (Input.GetKey(KeyCode.S))
                transform.Translate(Vector2.down * SetSpeedByZoom(keyMoveSpeed) * Time.deltaTime);
            //RIGHT
            if (Input.GetKey(KeyCode.D))
                transform.Translate(Vector2.right * SetSpeedByZoom(keyMoveSpeed) * Time.deltaTime);
            //LEFT
            if (Input.GetKey(KeyCode.A))
                transform.Translate(Vector2.left * SetSpeedByZoom(keyMoveSpeed) * Time.deltaTime);
        }
        void ScrollZoom()
        {
            float scroll = pcInput.Scroll();

            // Zoom In Wheel
            if (scroll > 0)
                pCZoom -= scrollZoomSpeed * Time.deltaTime;
            // Zoom In Key
            if (Input.GetKey(KeyCode.Q))
                pCZoom -= keyZoomSpeed * Time.deltaTime;
            // Zoom Out Wheel
            if (scroll < 0)
                pCZoom += scrollZoomSpeed * Time.deltaTime;
            // Zoom Out Key
            if (Input.GetKey(KeyCode.E))
                pCZoom += keyZoomSpeed * Time.deltaTime;

            pCZoom = Mathf.Clamp(pCZoom, minZoom, maxZoom);
            Camera.main.orthographicSize = pCZoom;
        }
        void GoWorldCenterWithKey()
        {
            if (Input.GetKeyDown(KeyCode.O))
                transform.position = firstPos;
        }
        #endregion
        float SetSpeedByZoom(float speed)
        {
            return cam.orthographicSize * speed;
        }
    }
}