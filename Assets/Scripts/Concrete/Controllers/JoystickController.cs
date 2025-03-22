using Assets.Scripts.Concrete.Managers;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    public class JoystickController : MonoBehaviour
    {
        public static JoystickController Instance;
        private void Awake()
        {
            Instance = this;
        }
        public FixedJoystick fixedJoystick;
        public Vector2 joyDir;
        void Update()
        {
            joyDir.x = fixedJoystick.Horizontal;
            joyDir.y = fixedJoystick.Vertical;
            if (joyDir.magnitude > 0.1f || KnightManager.Instance.move.savedDirection == Vector2.zero) // dead zone
                KnightManager.Instance.move.savedDirection = joyDir.normalized;

        }
    }
}