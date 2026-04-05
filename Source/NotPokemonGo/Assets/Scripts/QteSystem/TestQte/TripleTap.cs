using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UI.QTE;

namespace QteSystem.TestQte
{
    public sealed class TripleTap : QteInteractiveView, IHasQteDuration, IHasQteOutcomeMode
    {
        [SerializeField] private List<RadialQte> _radialQtes;
        [SerializeField] private Button _button;

        private readonly List<QteResult> _results = new();

        private float _delay;
        private int _currentRadialQte;
        private QteOutcomeMode _outcomeMode = QteOutcomeMode.Ternary;
        private CancellationTokenSource _lifetimeCts;

        public void SetDuration(float duration)
        {
            _delay = _radialQtes.Count == 0 ? duration : duration / _radialQtes.Count;
        }

        public void SetOutcomeMode(QteOutcomeMode mode)
        {
            _outcomeMode = mode;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);

            foreach (var radial in _radialQtes)
                radial.ResultAction += OnQteResult;

            _results.Clear();
            _currentRadialQte = 0;

            _lifetimeCts = new CancellationTokenSource();
            PlayAsync(_lifetimeCts.Token).Forget();
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);

            foreach (var radial in _radialQtes)
                radial.ResultAction -= OnQteResult;

            if (_lifetimeCts != null)
            {
                _lifetimeCts.Cancel();
                _lifetimeCts.Dispose();
                _lifetimeCts = null;
            }
        }

        private async UniTaskVoid PlayAsync(CancellationToken token)
        {
            if (_radialQtes == null || _radialQtes.Count == 0)
            {
                Complete(QteResult.Fail);
                return;
            }

            for (int i = 0; i < _radialQtes.Count; i++)
            {
                _radialQtes[i].SetTargetTime(_delay * 2f);
                _radialQtes[i].gameObject.SetActive(true);

                await UniTask.Delay(TimeSpan.FromSeconds(_delay), cancellationToken: token);
            }
        }

        private void OnClick()
        {
            if (_currentRadialQte >= _radialQtes.Count)
                return;

            _radialQtes[_currentRadialQte].Check();
            _currentRadialQte++;
        }

        private void OnQteResult(QteResult result, QteButtonView view)
        {
            _results.Add(result);
            view.gameObject.SetActive(false);

            if (_results.Count < _radialQtes.Count)
                return;

            Complete(CalculateFinalResult());
        }

        private QteResult CalculateFinalResult()
        {
            int perfectCount = 0;
            int successCount = 0;

            foreach (var result in _results)
            {
                if (result == QteResult.Perfect)
                    perfectCount++;

                if (result == QteResult.Perfect || result == QteResult.Normal)
                    successCount++;
            }

            if (_outcomeMode == QteOutcomeMode.Binary)
                return successCount > 0 ? QteResult.Normal : QteResult.Fail;

            if (perfectCount == _radialQtes.Count)
                return QteResult.Perfect;

            if (successCount > 0)
                return QteResult.Normal;

            return QteResult.Fail;
        }
    }
}