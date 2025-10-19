using System;
using Stats;
using UI.QTE;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace QTESystem.TestQTE
{
    public class TapToButtonManager : QteButtonView
    {
        [SerializeField] private TapToButton _tapToButton;
        [SerializeField] private Image _image;

        private int _valueToTap = 10;
        private float _maxValue = 100;
        private float _decaySpeed = 40;

        private float _currentValue = 0;

        private bool _isFulled = false;

        private Unit _unit;

        public override event Action<QteButtonView> Successed;
        public override event Action<QteButtonView> Invalided;

        private void OnEnable()
        {
            _tapToButton.Click += OnClick;
        }

        private void OnDisable()
        {
            _tapToButton.Click -= OnClick;

            if (Unit != null)
            {
                Unit.SetStatValue(StatType.QteDamageModifier, 1); //Странно, что вызвалось в начале боя
            }
        }

        private void Update()
        {
            if (_isFulled)
                return;

            if (_currentValue > 0)
                _currentValue = Mathf.MoveTowards(_currentValue, 0, _decaySpeed * Time.deltaTime);

            Unit.SetStatValue(StatType.QteDamageModifier, _currentValue / 100);

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

                Debug.Log(_currentValue + " currentValue");
                Unit.SetStatValue(StatType.QteDamageModifier, _currentValue);
                Successed?.Invoke(this);
                _isFulled = true;
            }
        }
    }
}