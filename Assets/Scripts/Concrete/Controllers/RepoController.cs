using UnityEngine;
using Assets.Scripts.Concrete.Managers;
using Assets.Scripts.Concrete.PowerStats;

namespace Assets.Scripts.Concrete.Controllers
{
    public class RepoController : MonoBehaviour
    {
        int maxRepoCapacity;
        public int gold;
        public int rock;
        public int wood;
        public int meat;
        RepoStats repoStats;

        private void Awake()
        {
            repoStats = GetComponent<RepoStats>();
        }
        void Start()
        {
            ResourcesManager.Instance.repos.Add(gameObject);
            maxRepoCapacity = repoStats.maxRepoCapacity;
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