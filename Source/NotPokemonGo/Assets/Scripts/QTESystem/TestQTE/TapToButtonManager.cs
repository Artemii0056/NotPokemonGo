using System;
using UI.QTE;
using UnityEngine;
using UnityEngine.UI;

namespace QTESystem.TestQTE
{
    public class TapToButtonManager : QTEButtonView
    {
        [SerializeField] private TapToButton _tapToButton;
        [SerializeField] private Image _image;

        private int _valueToTap = 10;
        private float _maxValue = 100;
        private float _decaySpeed = 40;

        private float _currentValue = 0;
        
        private bool _isFulled = false;
        
        public override event Action<QTEButtonView> Successed;
        public override event Action<QTEButtonView> Invalided;

        private void OnEnable()
        {
            _tapToButton.Click += OnClick;
        }

        private void OnDisable()
        {
            _tapToButton.Click -= OnClick;
        }

        private void Update()
        {
            if (_isFulled)
                return;

            if (_currentValue > 0)
                _currentValue = Mathf.MoveTowards(_currentValue, 0, _decaySpeed * Time.deltaTime);

            _image.fillAmount = _currentValue / _maxValue;
        }

        private void OnClick()
        {
            if (_isFulled)
                return;

            _currentValue += _valueToTap;

            if (_currentValue >= _maxValue)
            {
                _currentValue = _maxValue;
                _image.fillAmount = 1f;
                Successed?.Invoke(this);
                _isFulled = true;
            }
        }
    }
}