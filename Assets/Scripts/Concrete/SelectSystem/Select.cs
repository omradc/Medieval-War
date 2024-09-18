using Assets.Scripts.Abstracts.Inputs;
using Assets.Scripts.Concrete.Controllers;
using Assets.Scripts.Concrete.Inputs;
using Assets.Scripts.Concrete.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Concrete.SelectSystem
{
    internal class Select : ISelect
    {

        IInput ıInput;
        public List<GameObject> objs;
        LayerMask unitLayer;
        GameObject currentObj;
        public Vector2 startPos;
        Vector2 endPos;
        UnitManager unitManager;
        public bool isDragging;
        public Select(LayerMask unitLayer, UnitManager unitManager)
        {
            ıInput = new PcInput();
            this.unitLayer = unitLayer;
            this.unitManager = unitManager;
            objs = new List<GameObject>();
        }
        public void SelectOneByOne()
        {
            if (ıInput.GetButtonDown1)
            {
                // Ekran koordinatlarını dünya koordinatlarına çevir
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, 1, unitLayer);
                if (hit.collider != null)
                {
                    currentObj = hit.collider.gameObject;
                    UnitController uC = currentObj.GetComponent<UnitController>();
                    uC.workOnce = true;
                    uC.isSeleceted = true;
                    // Aynı nesneyi tekrar diziye atma
                    if (!objs.Contains(currentObj))
                        objs.Add(currentObj);

                    SelectedObjColor(0.5f, currentObj);

                }

            }
            if (ıInput.GetButtonDown0)
            {
                for (int i = 0; i < objs.Count; i++)
                {
                    objs[i].GetComponent<UnitController>().unitOrderEnum = unitManager.unitOrderEnum;
                    SelectedObjColor(1f, objs[i]);
                }

            }


        }
        public void SelectMultiple()
        {
            if (ıInput.GetButtonDown1)
            {
                startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                isDragging = true;
            }


            if (ıInput.GetButtonUp1)
            {
                endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                isDragging = false;
                Collider2D[] hits = Physics2D.OverlapAreaAll(startPos, endPos, unitLayer);
                for (int i = 0; i < hits.Length; i++)
                {

                    currentObj = hits[i].gameObject;

                    // Aynı nesneyi tekrar diziye atma
                    if (!objs.Contains(currentObj))
                        objs.Add(currentObj);
                    UnitController uC = objs[i].gameObject.GetComponent<UnitController>();
                    uC.unitOrderEnum = unitManager.unitOrderEnum;
                    uC.workOnce = true;
                    uC.isSeleceted = true;
                    SelectedObjColor(0.5f, currentObj);
                }

            }

            if (ıInput.GetButtonDown0)
            {
                for (int i = 0; i < objs.Count; i++)
                {
                    objs[i].GetComponent<UnitController>().unitOrderEnum = unitManager.unitOrderEnum;
                }

            }
        }
        public void ClearSelectedObjs()
        {
            if (ıInput.GetButtonUp0)
            {
                for (int i = 0; i < objs.Count; i++)
                {
                    objs[i].GetComponent<UnitController>().isSeleceted = false;
                }

                if (!ıInput.GetButton1)
                    objs.Clear();
            }
        }
        void SelectedObjColor(float alphaValue, GameObject obj)
        {
            Color color = obj.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            color.a = alphaValue;
            obj.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
        }

    }
}
