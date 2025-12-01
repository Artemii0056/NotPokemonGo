using System;
using UI.QTE;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace QTESystem.TestQTE
{
    public class RadialQte : QteButtonView
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _circle;
        [SerializeField] private float _targetTime;
        [SerializeField] private float _currentTime;

        [FormerlySerializedAs("_circleColor")] [SerializeField]
        private Color _targetColor;

        private Vector3 _halfCircleScale;

        private Vector3 _startCircleScale;

        public override event Action<QteButtonView> Successed;
        public override event Action<QteButtonView> Invalided;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void Start()
        {
            _startCircleScale = 2f * Vector3.one;
            _circle.transform.localScale = _startCircleScale;
            _halfCircleScale = _startCircleScale / 2;
            _currentTime = 0;
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void Update()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _targetTime)
            {
                _circle.transform.localScale = Vector3.zero;
                Invalided?.Invoke(this);
                return;
            }

            float progress = 1f - (_currentTime / _targetTime);
            _circle.transform.localScale = _startCircleScale * progress;

            if (_circle.transform.localScale.x > _halfCircleScale.x)
                return;

            if (_circle.color == _targetColor)
                return;

            _circle.color = _targetColor;
        }

        public void Check()
        {
            if (_circle.transform.localScale.x <= _halfCircleScale.x)
            {
                Successed?.Invoke(this);
                gameObject.SetActive(false);
            }
        }

        private void OnClick()
        {
            // if (_isFulled)
            //     return;
            //
            // _currentValue += _valueToTap;
            //
            // if (_currentValue >= _maxValue)
            // {
            //     _currentValue = _maxValue;
            //     _image.fillAmount = 1f;
            //
            //     Successed?.Invoke(this);
            //     _isFulled = true;
            // }
        }
    }
}