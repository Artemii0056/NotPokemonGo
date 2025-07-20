using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.QTE
{
    public class QTEBacgroundPanel : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public event Action Clicked;
        
        private void OnEnable() => 
            _button.onClick.AddListener(Click);

        private void OnDisable() => 
            _button.onClick.RemoveListener(Click);

        private void Click() => 
            Clicked?.Invoke();
    }
}