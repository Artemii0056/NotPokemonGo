using System;
using UI.QTE;
using UnityEngine;
using UnityEngine.UI;

namespace QTESystem.TestQTE
{
    public class SliderReader : QTEButtonView
    {
        [SerializeField] private Slider _slider;

        [SerializeField] private float _currentValue;
        [SerializeField] private float _targetValue; // Время анимации 
        
        [SerializeField] private float _currentTime; //А если реально попробовать сделать силу удара в зависимости от скорости перемещения от нуля до единицы? 
        [SerializeField] private float _targetTime; 
        
        private bool _isFinished;

        public event Action<bool> Ended;
        
        public override event Action<QTEButtonView> Successed;
        public override event Action<QTEButtonView> Invalided;

        private void Start()
        {
            _slider.value = _currentValue;
        }

        private void Update()
        {
            if (_isFinished == false)
            {
                _currentTime += Time.deltaTime;
                Debug.Log(_currentTime + " текущее");
            }

            if (_currentTime >= _targetTime)
            {
                _isFinished = true;
                ShowResult();
            }
        }

        private void ShowResult()
        {
            if (Mathf.Abs(_targetValue - _slider.value) <= 0.1)
            {
                Successed?.Invoke(this);
                 return;
            }
            
            Invalided?.Invoke(this);
        }
    }
}