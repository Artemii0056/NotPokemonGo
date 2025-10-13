using UnityEngine;
using UnityEngine.UI;

namespace QTE.Script
{
    class QTEHaloController : MonoBehaviour
    {
        [Header("Timing")] public float TargetTime = 2f;
        public float Offset = 0.5f;
        public float Speed = 1f;

        [Header("References")]
        public Image Halo;
        public Image TargetImage; // обычно зелёный круг
        public Image EndImage;    // например, маленькая точка в центре
        public Button Button;     // пользовательская кнопка

        [Header("Visuals")]
        public Color DefaultColor = Color.red;
        public Color ActiveColor = Color.green;

        [Header("Debug")]
        public float CurrentTime;
        public bool AutoStart = true;

        private Vector2 _startSize;
        private Vector2 _targetSize;
        private Vector2 _endSize;
        private bool _active;

        private void Start()
        {
            _startSize = Halo.rectTransform.sizeDelta;
            _targetSize = TargetImage.rectTransform.sizeDelta;
            _endSize = EndImage.rectTransform.sizeDelta;

            if (AutoStart)
                StartQTE();
        }

        public void StartQTE()
        {
            Debug.Log("StartQTE");
            CurrentTime = 0f;
            Halo.rectTransform.sizeDelta = _startSize;
            _active = true;
        }

        public void StopQTE()
        {
            _active = false;
        }

        private void Update()
        {
            if (!_active) return;

            CurrentTime += Time.deltaTime * Speed;

            // Update color
            if (CurrentTime >= TargetTime - Offset && CurrentTime <= TargetTime + Offset)
                TargetImage.color = ActiveColor;
            else
                TargetImage.color = DefaultColor;

            // Resize Halo
            if (CurrentTime <= TargetTime)
            {
                float t = CurrentTime / TargetTime;
                Halo.rectTransform.sizeDelta = Vector2.Lerp(_startSize, _targetSize, t);
            }
            else if (CurrentTime <= TargetTime + Offset)
            {
                float t = (CurrentTime - TargetTime) / Offset;
                Halo.rectTransform.sizeDelta = Vector2.Lerp(_targetSize, _endSize, t);
            }
            else
            {
                Halo.rectTransform.sizeDelta = _endSize;
            }
        }

        public bool IsInSuccessZone()
        {
            return CurrentTime >= TargetTime - Offset && CurrentTime <= TargetTime + Offset;
        }

        public void OnButtonClicked()
        {
            Debug.Log(IsInSuccessZone() ? "✅ SUCCESS" : "❌ FAIL");
            StopQTE();
        }
    }
}