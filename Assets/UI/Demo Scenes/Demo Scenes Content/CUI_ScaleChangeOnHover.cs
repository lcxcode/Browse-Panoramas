using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

namespace CurvedUI
{
    public class CUI_ScaleChangeOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static CUI_ScaleChangeOnHover instance;
        [HideInInspector]
        public float restScale = 1;
        public float OnHoverScale = 1.1f;

       public bool Zoomed = false;

        void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
        }

        // Update is called once per frame
        void Update()
        {
            (transform as RectTransform).localScale = Zoomed ? new Vector3(OnHoverScale, OnHoverScale, restScale) : new Vector3(restScale, restScale, restScale);
            //(transform as RectTransform).localScale = Zoomed ? Vector3.Lerp(Vector3.one,new Vector3 (OnHoverScale, OnHoverScale, restScale),0.5f) : new Vector3(restScale, restScale, restScale);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Zoomed = true;
            Debug.Log("true");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("false");
            Zoomed = false;
        }


    }
}
