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

        }
    }
}