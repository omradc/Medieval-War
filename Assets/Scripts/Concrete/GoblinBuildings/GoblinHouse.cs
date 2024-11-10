using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Concrete.GoblinBuildings
{
    public class GoblinHouse : GoblinBuilding
    {
        float time;

        public void GoblinSpawner(Transform thisObj, GameObject spawnObj, Transform goblins, int spawnTime, int maxGoblin)
        {
            time += Time.deltaTime;
            if (time > spawnTime && goblins.childCount < maxGoblin)
            {
                Object.Instantiate(spawnObj, thisObj.position + new Vector3(0, -1, 0), thisObj.transform.rotation, goblins);
                time = 0;
            }
        }
    }
}