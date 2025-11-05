using System;
using UI.QTE;
using UnityEngine;
using UnityEngine.UI;

namespace QTESystem.TestQTE
{
    public class SliderReader : QteButtonView
    {
        [SerializeField] private Slider _slider; //Тут нужно еще добавить отображение времени qte

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
        
            _targetTime = Unit.UnitAnimatorController.GetAnimationLength();  
        }

        private void Update()
        {
            if (_isFinished == false)
                _currentTime += TimeService.UnscaledDeltaTime;
                //_currentTime += Time.deltaTime * Time.timeScale * 10; //Вот тут вопросики
            
            if (_currentTime >= _targetTime)
            {
                _isFinished = true;
                ShowResult();
            }
            
            if (Mathf.Abs(_targetSliderValue - _slider.value) <= 0.1)
            {
                Successed?.Invoke(this);
            }
        }

        private void ShowResult()
        {
            Debug.Log(Mathf.Abs(_targetSliderValue - _slider.value));
            
            if (Mathf.Abs(_targetSliderValue - _slider.value) <= 0.1)
            {
                Successed?.Invoke(this);
                return;
            }

            Invalided?.Invoke(this);
        }
    }
}