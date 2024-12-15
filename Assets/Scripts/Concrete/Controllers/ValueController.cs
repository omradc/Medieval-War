using Assets.Scripts.Concrete.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Concrete.Controllers
{
    public class ValueController : MonoBehaviour
    {
        public string objName;
        public GameObject valuePanel;
        Image goldPanel;
        Image rockPanel;
        Image woodPanel;
        Image meatPanel; 
        TextMeshProUGUI goldText;
        TextMeshProUGUI rockText;
        TextMeshProUGUI woodText;
        TextMeshProUGUI meatText;

        ResourcesManager rM;

        private void Awake()
        {
            goldPanel = valuePanel.transform.GetChild(0).GetComponent<Image>();
            rockPanel = valuePanel.transform.GetChild(1).GetComponent<Image>();
            woodPanel = valuePanel.transform.GetChild(2).GetComponent<Image>();
            meatPanel = valuePanel.transform.GetChild(3).GetComponent<Image>();

            goldText = valuePanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
            rockText = valuePanel.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
            woodText = valuePanel.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
            meatText = valuePanel.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();
        }
        void Start()
        {
            rM = ResourcesManager.Instance;
            DisplayResourcesValues();
            InvokeRepeating(nameof(Optimum), 0.1f, 1f);
        }

        void Optimum()
        {
            DisplayPanelColor();
        }

        void DisplayPanelColor()
        {
            rM.CheckResources(objName);
            if (rM.goldIsEnough)
                goldPanel.color = Color.white;
            if (!rM.goldIsEnough)
                goldPanel.color = Color.red;

            if (rM.rockIsEnough)
                rockPanel.color = Color.white;
            if (!rM.rockIsEnough)
                rockPanel.color = Color.red;

            if (rM.woodIsEnough)
                woodPanel.color = Color.white;
            if (!rM.woodIsEnough)
                woodPanel.color = Color.red;

            if (rM.meatIsEnough)
                meatPanel.color = Color.white;
            if (!rM.meatIsEnough)
                meatPanel.color = Color.red;
        }

        void DisplayResourcesValues()
        {
            rM.ResourcesValues(objName);
            goldText.text = rM.goldPrice.ToString();
            rockText.text = rM.rockPrice.ToString();
            woodText.text = rM.woodPrice.ToString();
            meatText.text = rM.meatPrice.ToString();
        }
    }
}