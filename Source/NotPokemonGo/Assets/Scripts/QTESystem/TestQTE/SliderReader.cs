using System;
using UI.QTE;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace QTESystem.TestQTE
{
    public class SliderReader : QteButtonView
    {
        [SerializeField] private Slider _slider;

        [SerializeField] private float _currentSliderValue = 0.5f;
        [SerializeField] private float _targetSliderValue;

        [SerializeField] private float _currentTime;

        [SerializeField] private float _targetTime;

        private bool _isFinished;

        public override event Action<QteButtonView> Successed;
        public override event Action<QteButtonView> Invalided;

        private void Start()
        {
            _slider.value = _currentSliderValue;

            _targetTime = Unit.UnitAnimatorController.GetAnimationLenght();
        }

        private void Update()
        {
            if (_isFinished == false)
                _currentTime += Time.deltaTime;

            if (_currentTime >= _targetTime)
            {
                _isFinished = true;
                ShowResult();
            }
        }

        private void ShowResult()
        {
            if (Mathf.Abs(_targetSliderValue - _slider.value) <= 0.1)
            {
                Successed?.Invoke(this);
                return;
            }

            Invalided?.Invoke(this);
        }
    }
}