using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class PanelController : MonoBehaviour
    {
        [SerializeField] GameObject interactablePanel;
        [SerializeField] GameObject trainTimePanel;
        [SerializeField] GameObject destructPanel;

        public void InteractablePanelVisibility(bool visibility)
        {
            interactablePanel.SetActive(visibility);
        }

        public void TimerPanelVisibility(bool visibility)
        {
            trainTimePanel.SetActive(visibility);
        }

        public void DestructPanelVisibility(bool visibility)
        {
            destructPanel.SetActive(visibility);
        }


    }
}
