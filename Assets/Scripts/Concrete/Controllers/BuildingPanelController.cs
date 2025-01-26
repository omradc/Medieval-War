using Assets.Scripts.Concrete.Managers;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class BuildingPanelController : MonoBehaviour
    {
        public GameObject interactablePanel;
        public GameObject destructPanel;
        [SerializeField] GameObject trainTimePanel;
        BuildingController bC;

        private void Awake()
        {
            bC = GetComponent<BuildingController>();
        }

        //Event trigger bileşeni ile tetiklenir
        public void InteractablePanelVisibility(bool visibility)
        {
            if (bC.isFull || InteractManager.Instance.selectedUnits.Count > 0)
                interactablePanel.SetActive(false); // üzerinde birim varsa veya en az 1 birim seçili ise panelleri kapat
            else if (!bC.destruct) // yıkılmadıysa paneli aç
                interactablePanel.SetActive(visibility);
            else
                interactablePanel.SetActive(false);
        }

        //Event trigger bileşeni ile tetiklenir
        public void DestructPanelVisiblity(bool visibility)
        {
            if (bC.isFull) // üzerinde birim varsa, panelleri kapat
                destructPanel.SetActive(false);
            else if (bC.destruct) // yıkıldıysa paneli aç
                destructPanel.SetActive(visibility);
            else
                destructPanel.SetActive(false);
        }
        public void TimerPanelVisibility(bool visibility)
        {
            trainTimePanel.SetActive(visibility);
        }

    }

}
