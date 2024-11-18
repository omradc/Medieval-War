using Assets.Scripts.Concrete.GoblinBuildings;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    public class GoblinHouseController : MonoBehaviour
    {

        [SerializeField] GameObject goblin;
        [SerializeField] Transform goblins;

        [SerializeField] int spawnTime;
        [SerializeField] int maxGoblin;

        GoblinHouse gHouse;
        private void Awake()
        {
            gHouse = new GoblinHouse();
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            gHouse.GoblinSpawner(transform, goblin, goblins, spawnTime, maxGoblin);
        }
    }
}