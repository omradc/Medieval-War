using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.Managers;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace Assets.Scripts.Concrete.Controllers
{
    public class BuildPreviewController : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;
        GameObject visual;
        GameObject visualRed;
        IInput ıInput;
        Vector2 firstPos;

        private void Awake()
        {
            ıInput = new PcInput();
        }
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            visual = transform.GetChild(0).gameObject;
            visualRed = transform.GetChild(1).gameObject;
        }
        void Update()
        {
            if (ıInput.GetButtonDown0)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                firstPos = transform.position - mousePos;
            }
            if (ıInput.GetButton0)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = firstPos + mousePos;
            }
        }

        public void BuildConfirmButton()
        {
            UIManager.Instance.buildConfirm = true;
            Destroy(gameObject);
        }
        public void BuildCancelButton()
        {
            Destroy(gameObject);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            UIManager.Instance.canBuild = false;
            visual.gameObject.SetActive(false);
            visualRed.gameObject.SetActive(true);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            UIManager.Instance.canBuild = true;
            visual.gameObject.SetActive(true);
            visualRed.gameObject.SetActive(false);
        }
    }
}