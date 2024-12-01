using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.Managers;
using System.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace Assets.Scripts.Concrete.Controllers
{
    public class BuildPreviewController : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;
        GameObject visual;
        GameObject visualRed;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            visual = transform.GetChild(0).gameObject;
            visualRed = transform.GetChild(1).gameObject;
        }
        void Update()
        {
            if (!UIManager.Instance.buildPreview)
                Destroy(gameObject);
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = pos;
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