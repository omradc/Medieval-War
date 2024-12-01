using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Inputs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Concrete.Managers
{
    internal class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        private void Awake()
        {
            Singelton();
        }
        void Singelton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
                Destroy(this);
        }

        private void Update()
        {
            Restart();
        }

        void Restart()
        {
            if (Input.GetMouseButtonDown(2))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
