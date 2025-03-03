using UnityEngine;
using Assets.Scripts.Concrete.Managers;

namespace Assets.Scripts.Concrete.Controllers
{
    public class RepoController : MonoBehaviour
    {
        public int maxRepoCapacity;
        public int gold;
        public int rock;
        public int wood;
        public int meat;

        public bool resetList;
        void Start()
        {
            ResourcesManager.Instance.repos.Add(gameObject);
        }

        public bool CanUseRepo()
        {
            if (gold + rock + wood + meat >= maxRepoCapacity)
                return false;
            else
                return true;
        }
    }
}