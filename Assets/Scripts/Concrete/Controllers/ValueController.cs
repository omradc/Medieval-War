using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.Names;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Concrete.Controllers
{
    public class ValueController : MonoBehaviour
    {
        [HideInInspector] public Image goldPanel;
        [HideInInspector] public Image rockPanel;
        [HideInInspector] public Image woodPanel;
        [HideInInspector] public Image meatPanel;
        [HideInInspector] public TextMeshProUGUI goldText;
        [HideInInspector] public TextMeshProUGUI rockText;
        [HideInInspector] public TextMeshProUGUI woodText;
        [HideInInspector] public TextMeshProUGUI meatText;

        int gold, wood, rock, meat;
        public string itemName; // İsmi değiştirme, oyun içinde heryerden değişmesi gerekir
        private void Awake()
        {
            goldPanel = transform.GetChild(0).GetComponent<Image>();
            rockPanel = transform.GetChild(1).GetComponent<Image>();
            woodPanel = transform.GetChild(2).GetComponent<Image>();
            meatPanel = transform.GetChild(3).GetComponent<Image>();

            goldText = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
            rockText = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
            woodText = transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
            meatText = transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            ResourcesManager.Instance.FirstItemValuesDisplay(itemName, out gold, out rock, out wood, out meat);
            goldText.text = gold.ToString();
            rockText.text = rock.ToString();
            woodText.text = wood.ToString();
            meatText.text = meat.ToString();
        }
    }
}