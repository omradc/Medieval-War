using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Concrete.Controllers
{
    public class GoblinHouseController : MonoBehaviour
    {

        [SerializeField] GameObject spawnedGoblin;
        [SerializeField] Transform spawnPoint;
        [SerializeField] int spawnTime;
        [SerializeField] int maxGoblin;
        public List<GameObject> goblins;
        Transform allGoblins;
        float time;

        private void Awake()
        {
            allGoblins = GameObject.FindWithTag("Goblins").transform;
        }

        void Update()
        {
            GoblinSpawner();
        }


        public void GoblinSpawner()
        {
            time += Time.deltaTime;
            if (time > spawnTime && allGoblins.childCount < maxGoblin)
            {
                goblins.Add(Instantiate(spawnedGoblin, spawnPoint.position, transform.rotation, allGoblins));

                time = 0;
            }
        }
    }
}