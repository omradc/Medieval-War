﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Concrete.Controllers
{
    public class ConstructController : MonoBehaviour
    {
        [HideInInspector] public GameObject building;
        public float hitNumber;
        public float currentHitNumber;
        public bool isFull;
        public Image constructionTimerImage;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (currentHitNumber >= hitNumber)
            {
                Build();
            }

        }

        void Build()
        {
            Instantiate(building, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        public void UpdateConstructionTimerImage()
        {
            constructionTimerImage.fillAmount = currentHitNumber / hitNumber;
        }

    }
}