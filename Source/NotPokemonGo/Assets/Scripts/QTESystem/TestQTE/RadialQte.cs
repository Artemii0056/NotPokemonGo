using System;
using UI.QTE;
using UnityEngine;
using UnityEngine.UI;

namespace QTESystem.TestQTE
{
    public class RadialQte : QteButtonView
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _circle;
        [SerializeField] private float _targetTime;
        [SerializeField] private float _currentTime;

        [SerializeField] private Color _targetColor;

        private Vector3 _halfCircleScale;

        private Vector3 _startCircleScale;
        
        public event Action<QteResult, QteButtonView> ResultAction;

        public override event Action<QteButtonView> Successed;
        public override event Action<QteButtonView> Invalided;

        private void Start()
        {
            _startCircleScale = 2f * Vector3.one;

            _circle.transform.localScale = _startCircleScale;
            _halfCircleScale = _startCircleScale / 2;
            _currentTime = 0;
        }

        private void Update()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _targetTime)
            {
                _circle.transform.localScale = Vector3.zero;
                ResultAction?.Invoke(QteResult.Fail, this);
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

        public void SetTargetTime(float targetTime)
        {
            _targetTime = targetTime;
        }

        public void Check()
        {
            if (_circle.transform.localScale.x <= _halfCircleScale.x)
                ResultAction?.Invoke(QteResult.Perfect, this);
            else
                ResultAction?.Invoke(QteResult.Fail, this);

            gameObject.SetActive(false);
        }
    }
}