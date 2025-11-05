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

        private int _valueToTap = 20;
        private float _maxValue = 100;
        private float _decaySpeed = 20;

        private float _currentValue = 0;

        private bool _isFulled = false;

        private float _currentTime = 0;
        private float _targetTime;

        private Unit _unit;

        public override event Action<QteButtonView> Successed;
        public override event Action<QteButtonView> Invalided;

        private void OnEnable() =>
            _tapToButton.Click += OnClick;

        private void Start()
        {
            _targetTime = Unit.UnitAnimatorController.GetAnimationLength();
            Debug.Log(_targetTime);
        }

        private void OnDisable()
        {
            _tapToButton.Click -= OnClick;

            Unit.SetStatValue(StatType.QteDamageModifier, 1);
        }

        private void Update()
        {
            _currentTime += TimeService.UnscaledDeltaTime;

            if (_currentTime >= _targetTime)
                Invalided?.Invoke(this);

            if (_isFulled)
                return;

            if (_currentValue > 0)
                _currentValue = Mathf.MoveTowards(_currentValue, 0, _decaySpeed * TimeService.UnscaledDeltaTime);

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

                Unit.SetStatValue(StatType.QteDamageModifier, _currentValue);
                Successed?.Invoke(this);
                _isFulled = true;
            }
        }
    }
}