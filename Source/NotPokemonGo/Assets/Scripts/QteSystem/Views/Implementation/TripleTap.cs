using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using QteSystem.Core;
using QteSystem.Views.Base;
using QteSystem.Views.Widgets;
using UI.QTE;
using UnityEngine;
using UnityEngine.UI;

namespace QteSystem.Views.Implementation
{
    public sealed class TripleTap : QteInteractiveView, IHasQteDuration, IHasQteOutcomeMode
    {
        private const float RadialLifetimeMultiplier = 2f;

        [SerializeField] private List<RadialQte> _radialQtes;
        [SerializeField] private Button _button;

        private readonly List<QteResult> _results = new();

        private float _delay;
        private int _currentRadialQte;
        private QteOutcomeMode _outcomeMode = QteOutcomeMode.Ternary;
        private CancellationTokenSource _lifetimeCts;

        public void SetDuration(float duration)
        {
            _delay = _radialQtes == null || _radialQtes.Count == 0
                ? duration
                : duration / _radialQtes.Count;
        }

        public void SetOutcomeMode(QteOutcomeMode mode)
        {
            _outcomeMode = mode;
        }

        private void OnEnable()
        {
            if (_button == null)
            {
                Complete(QteResult.Fail);
                return;
            }

            if (_radialQtes == null || _radialQtes.Count == 0)
            {
                Complete(QteResult.Fail);
                return;
            }

            _button.onClick.AddListener(OnClick);

            foreach (var radial in _radialQtes)
            {
                if (radial == null)
                    continue;

                radial.gameObject.SetActive(false);
                radial.ResultAction += OnQteResult;
            }

            _results.Clear();
            _currentRadialQte = 0;

            _lifetimeCts = new CancellationTokenSource();
            PlayAsync(_lifetimeCts.Token).Forget();
        }

        private void OnDisable()
        {
            if (_button != null)
                _button.onClick.RemoveListener(OnClick);

            if (_radialQtes != null)
            {
                foreach (var radial in _radialQtes)
                {
                    if (radial == null)
                        continue;

                    radial.ResultAction -= OnQteResult;
                }
            }

            if (_lifetimeCts != null)
            {
                _lifetimeCts.Cancel();
                _lifetimeCts.Dispose();
                _lifetimeCts = null;
            }
        }

        private async UniTaskVoid PlayAsync(CancellationToken token)
        {
            try
            {
                for (int i = 0; i < _radialQtes.Count; i++)
                {
                    var radial = _radialQtes[i];
                    if (radial == null)
                        continue;

                    radial.SetTargetTime(_delay * RadialLifetimeMultiplier);
                    radial.gameObject.SetActive(true);

                    await UniTask.Delay(TimeSpan.FromSeconds(_delay), cancellationToken: token);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void OnClick()
        {
            if (_currentRadialQte >= _radialQtes.Count)
                return;

            var radial = _radialQtes[_currentRadialQte];
            if (radial == null || !radial.gameObject.activeInHierarchy)
                return;

            radial.Check();
            _currentRadialQte++;
        }

        private void OnQteResult(QteResult result, QteButtonView view)
        {
            if (_results.Count >= _radialQtes.Count)
                return;

            _results.Add(result);

            if (view != null)
                view.gameObject.SetActive(false);

            if (_results.Count < _radialQtes.Count)
                return;

            Complete(CalculateFinalResult());
        }

        private QteResult CalculateFinalResult()
        {
            int perfectCount = 0;
            int successCount = 0;
            int totalCount = _radialQtes.Count;

            foreach (var result in _results)
            {
                if (result == QteResult.Perfect)
                    perfectCount++;

                if (result == QteResult.Perfect || result == QteResult.Normal)
                    successCount++;
            }

            if (_outcomeMode == QteOutcomeMode.Binary)
                return successCount == totalCount ? QteResult.Normal : QteResult.Fail;

            if (perfectCount == totalCount)
                return QteResult.Perfect;

            if (successCount == totalCount)
                return QteResult.Normal;

            return QteResult.Fail;
        }
    }
}