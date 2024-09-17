using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class PanelController : MonoBehaviour
    {
        [SerializeField] GameObject trainUnitPanel;
        [SerializeField] GameObject trainTimePanel;

        public void TrainUnitVisibility(bool visibility)
        {
            trainUnitPanel.SetActive(visibility);
        }

        public void TrainTimeVisibility(bool visibility)
        {
            trainTimePanel.SetActive(visibility);
        }


    }
}
