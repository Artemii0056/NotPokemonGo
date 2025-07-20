using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.QTE
{
    public class QTEButtonView : MonoBehaviour
    {
        public RectTransform rectTransform;
        
        public float TargetTime;
        public float Offset;
        public Button Button;
        public Image TargetImage;

        public Image Halo;
        public Image End;

        public float CurrentTime;

        public float Speed;

        private Vector2 _initialSize;
        private Vector2 _endSize;
        private Vector2 _targetSize;

        private Vector2 _visualTargetSize;
        private Vector2 _visualEndSize;

        public event Action<QTEButtonView> Successed;
        public event Action<QTEButtonView> Invalided;
        public event Action<QTEButtonView> Released;
        
        private bool _isSuccesTime => CurrentTime >= TargetTime - Offset && CurrentTime <= TargetTime;


        private void OnEnable() => 
            Button.onClick.AddListener(Clicked);

        private void OnDisable() => 
            Button.onClick.RemoveListener(Clicked);

        public void Initialize(float offset, float targetTime, Vector2 position)
        {
            rectTransform.anchoredPosition = position;
            
            TargetTime = targetTime;
            Offset = offset;
            
            _targetSize = TargetImage.rectTransform.sizeDelta;
            _endSize = End.rectTransform.sizeDelta;
            
            // Halo должен достичь targetSize в момент (TargetTime - Offset)
            float progressDuration = TargetTime - Offset;
            float scaleMultiplier = 1f + Offset / progressDuration;
            _initialSize = _targetSize * scaleMultiplier;

            Halo.rectTransform.sizeDelta = _initialSize;
            CurrentTime = 0;
        }

        private void Update()
        {
            CurrentTime += Time.deltaTime * Speed;

            if (_isSuccesTime)
            {
                TargetImage.color = Color.green;
            }
            else
            {
                TargetImage.color = Color.red;
            }

            if (CurrentTime <= TargetTime - Offset)
            {
                // Фаза 1: от начального до targetSize
                float progress = Mathf.Clamp01(CurrentTime / (TargetTime - Offset));
                Halo.rectTransform.sizeDelta = Vector2.Lerp(_initialSize, _targetSize, progress);
            }
            else if (CurrentTime <= TargetTime)
            {
                // Фаза 2: от targetSize до endSize
                float progress = Mathf.Clamp01((CurrentTime - (TargetTime - Offset)) / Offset);
                Halo.rectTransform.sizeDelta = Vector2.Lerp(_targetSize, _endSize, progress);
            }
            else
            {
                // После TargetTime — остаётся маленьким
                Halo.rectTransform.sizeDelta = _endSize;
            }
        }

        private void Clicked()
        {
            if (_isSuccesTime) 
                Successed?.Invoke(this);
            else
                Invalided?.Invoke(this);
        }

        public void Reset()
        {
            CurrentTime = 0;
            rectTransform.anchoredPosition = Vector2.zero;
        }

        public void ReleaseToPool()
        {
            Released?.Invoke(this);
        }
    }
}