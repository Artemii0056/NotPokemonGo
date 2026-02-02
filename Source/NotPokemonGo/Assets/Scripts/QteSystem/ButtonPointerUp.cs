using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace QteSystem
{
    public class ButtonPointerUp : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        public event Action Up;
        public event Action Downed;
        
        public void OnPointerUp(PointerEventData eventData) => 
            Up?.Invoke();

        public void OnPointerDown(PointerEventData eventData) => 
            Downed?.Invoke();
    }
}