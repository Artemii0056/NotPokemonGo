using System;
using UnityEngine;
using UnityEngine.UI;

namespace QteSystem.TestQte
{
    public class TapToButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public event Action Click;

        private void OnEnable() => 
            _button.onClick.AddListener(OnClick);

        private void OnDisable() => 
            _button.onClick.RemoveListener(OnClick);

        private void OnClick() => 
            Click?.Invoke();
    }
}