using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace TwinTower
{
    public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Action OnClickHandler = null;
        public Action OnDragHandler = null;
        public Action OnEndDragHandler = null;
        public Action OnEnterHandler = null;
        public Action OnExitHandler = null;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (OnClickHandler != null)
                OnClickHandler.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (OnDragHandler != null)
                OnDragHandler.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (OnEndDragHandler != null)
                OnEndDragHandler.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (OnEnterHandler != null)
                OnEnterHandler.Invoke();
            Debug.Log("ㅇㅇ");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (OnExitHandler != null)
                OnExitHandler.Invoke();
        }
    }
}