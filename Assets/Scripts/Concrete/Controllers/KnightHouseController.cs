using Assets.Scripts.Concrete.KnightBuildings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Concrete.Controllers
{
    internal class KnightHouseController : MonoBehaviour
    {
        [SerializeField] GameObject unit;
        public Image timerFillImage;
        public float trainingTime;

        Transform trainedUnitPos;
        InteractableObjUIController interactableObjUIController;
        KnightHouse knightHouse;
        
        private void Awake()
        {
            interactableObjUIController = GetComponent<InteractableObjUIController>();
            trainedUnitPos = transform.GetChild(0);

        }
        private void Start()
        {
            knightHouse = new KnightHouse(unit, trainedUnitPos.position, interactableObjUIController, this);
        }
        private void Update()
        {           
            knightHouse.TrainUnit();
        }





    }
}
