using System;
using UI.QTE;
using UnityEngine;
using UnityEngine.UI;

namespace QteSystem.TestQte
{
    public class OneTapQte : QteButtonView
    {
        [SerializeField] private Button _button;

        public override event Action<QteButtonView> Successed;
        public override event Action<QteButtonView> Invalided;

        private void OnEnable() => 
            _button.onClick.AddListener(OnClick);

        private void OnDisable() => 
            _button.onClick.RemoveListener(OnClick);

        private void OnClick() => 
            Successed?.Invoke(this);
    }
}