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
        public int CurrentRepoCapacity()
        {
            return gold + rock + wood + meat;
        }
    }
}