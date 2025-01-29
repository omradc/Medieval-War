using Assets.Scripts.Concrete.Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class BuildingPanelController : MonoBehaviour
    {
        public GameObject interactablePanel;
        public GameObject destructPanel;
        [SerializeField] GameObject trainTimePanel;
        BuildingController bC;
        float time;
        private void Awake()
        {
            bC = GetComponent<BuildingController>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && interactablePanel.activeSelf)
                if (!InteractManager.Instance.CheckUIElements())
                    interactablePanel.SetActive(false);

        }

        //Event trigger bileşeni ile tetiklenir
        public void PanelVisibility()
        {
            if(!bC.destruct) // Bina yıkılmadıysa
            {
                if (bC.isFull || InteractManager.Instance.selectedUnits.Count > 0)  // üzerinde birim varsa, yıkıldıysa veya 1 birim seçili ise panelleri kapat
                    interactablePanel.SetActive(false);
                else
                    interactablePanel.SetActive(true);
            }

            if(bC.destruct) // Bina yıkıldıysa
            {
                if (InteractManager.Instance.selectedUnits.Count > 0) // 1 birim seçili ise panelleri kapat
                    destructPanel.SetActive(false);
                else
                    destructPanel.SetActive(true);
            }
        }

        public void TimerPanelVisibility(bool visibility)
        {
            trainTimePanel.SetActive(visibility);
        }

    }

}
