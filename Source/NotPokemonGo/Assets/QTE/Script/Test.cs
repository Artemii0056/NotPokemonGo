using System;
using UnityEngine;
using UnityEngine.UI;

namespace QTE.Script
{
    public class Test : MonoBehaviour
    {
        public float TargetTime;
        public float Offset;
        public Button Button;
        public Image TargetImage;

        public Image Halo;
        public Image End;


        public Button ButtonReset;

        public float CurrentTime;

        public float Speed;

        private Vector2 _initialSize;
        private Vector2 _endSize;
        private Vector2 _targetSize;

        private Vector2 _visualTargetSize;
        private Vector2 _visualEndSize;
        
        private void OnEnable()
        {
            _targetSize = TargetImage.rectTransform.sizeDelta;
            _endSize = End.rectTransform.sizeDelta;

            // Halo должен достичь targetSize в момент (TargetTime - Offset)
            float progressDuration = TargetTime - Offset;
            float scaleMultiplier = 1f + Offset / progressDuration;
            _initialSize = _targetSize * scaleMultiplier;
            
            Reset(); // чтобы Halo сразу начал с большого
        }

        private void Update()
        {
            CurrentTime += Time.deltaTime * Speed;

            if (CurrentTime >= TargetTime - Offset && CurrentTime <= TargetTime)
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

        public void Reset()
        {
            Debug.Log("Reset");
            CurrentTime = 0;

            if (Halo != null)
                Halo.rectTransform.sizeDelta = _initialSize;
        }
    }
}