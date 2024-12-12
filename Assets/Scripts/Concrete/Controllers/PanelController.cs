using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class PanelController : MonoBehaviour
    {
        public GameObject interactablePanel;
        public GameObject destructPanel;
        [SerializeField] GameObject trainTimePanel;

        public void InteractablePanelVisibility(bool visibility)
        {
            interactablePanel.SetActive(visibility);
        }

        public void TimerPanelVisibility(bool visibility)
        {
            trainTimePanel.SetActive(visibility);
        }

        public void DestructPanelVisiblity(bool visibility)
        {
            destructPanel.SetActive(visibility);
        }

    }

}
