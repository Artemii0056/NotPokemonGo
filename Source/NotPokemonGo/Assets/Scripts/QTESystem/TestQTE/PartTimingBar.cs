using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace QTESystem.TestQTE
{
    public class PartTimingBar : MonoBehaviour
    {
        [field: SerializeField] public Image DefaultImage { get; private set; }
        
        [Header("References")] 
        [SerializeField] private RectTransform _cursor;

        [SerializeField] private RectTransform _bar;

        [SerializeField] private QteDifficulty _difficulty;

        private Tween _tween;
        private float _timer;

        public event Action Finished;

        public void Play()
        {
            _cursor.gameObject.SetActive(true);
            SetCursor(0);
            StartQte();
        }

        public void DeactivateCursor() => 
            _cursor.gameObject.SetActive(false);

        private void StopQte()
        {
            _tween?.Kill();
            Finished?.Invoke();
        }

        private void StartQte()
        {
            _timer = 0f;

            _tween?.Kill();

            _tween = DOTween.To(
                    () => _timer,
                    SetCursor,
                    1f,
                    _difficulty.duration
                ).SetEase(Ease.Linear)
                .OnComplete(StopQte);
        }

        private void SetCursor(float value)
        {
            float difficultyDuration = _difficulty.duration / 2;
            _timer = value;

            _cursor.anchorMin = new Vector2(_timer, difficultyDuration);
            _cursor.anchorMax = new Vector2(_timer, difficultyDuration);
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }

        private void OnValidate()
        {
            if (DefaultImage != null)
                SetupZones();

            if (_cursor != null)
                SetCursor(_timer);
        }

        private void SetupZones() =>
            SetZone(DefaultImage.rectTransform, _difficulty.OkStart, _difficulty.OkEnd);

        private void SetZone(RectTransform zone, float start, float end)
        {
            zone.SetParent(_bar, false);
            zone.anchorMin = new Vector2(start, 0f);
            zone.anchorMax = new Vector2(end, 1f);
            zone.offsetMin = Vector2.zero;
            zone.offsetMax = Vector2.zero;
        }
    }
}