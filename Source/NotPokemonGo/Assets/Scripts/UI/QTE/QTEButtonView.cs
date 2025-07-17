using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.QTE
{
    public class QTEButtonView : MonoBehaviour
    {
        private float _targetTime;
        private float _offset;
        private Button _button;
        private Image TargetImage;
        private Image Halo;
        private float CurrentTime;

        private float _speed = 2f;
        
        public event Action<bool> IsSuccessed;
        
        private bool IsSuccess => CurrentTime <= _targetTime + _offset && CurrentTime >= _targetTime - _offset;

        public void Initialize(float offset, float targetTime, Vector2 position)
        {
            _offset = offset;
            _targetTime = targetTime;
            _button.onClick.AddListener(Clicked);
            CurrentTime = 0;
        }
        
        private void OnDisable()
        {
            _button.onClick.RemoveListener(Clicked);
        }

        private void Update()
        {
            CurrentTime +=  Time.deltaTime * _speed;

            if (IsSuccess)
                TargetImage.color = Color.green;
            else
                TargetImage.color = Color.red;
        }

        private void Clicked() => 
            IsSuccessed?.Invoke(IsSuccess);
    }
}