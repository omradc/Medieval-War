using UnityEngine;

namespace Assets.Scripts.Concrete.Managers
{
    public class JoystickManager : MonoBehaviour
    {
        public static JoystickManager Instance;
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