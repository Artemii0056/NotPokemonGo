using System;
using System.Collections;
using System.Collections.Generic;
using UI.QTE;
using UnityEngine;
using UnityEngine.UI;

namespace QteSystem.TestQte
{
    public class TripleTap : QteButtonView, IHasQteDuration, IProvidesQteResult
    {
        [SerializeField] private List<RadialQte> _radialQtes;
        [SerializeField] private Button _button;
        [SerializeField] private float _delay = 0.7f;
        
        private WaitForSeconds _spawnDelay;

        private int _currentRadialQte;

        private int _successRadialQte;
        private int _failedRadialQte;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);

            foreach (var radial in _radialQtes)
                radial.ResultAction += OnQteResult;
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);

            foreach (var radial in _radialQtes)
                radial.ResultAction -= OnQteResult;
        }

        private void Start()
        {
            Debug.Log(_delay);
            
            _spawnDelay = new WaitForSeconds(_delay);
            
            StartCoroutine(PlayCoroutine());
            _currentRadialQte = 0;
        }

        public void SetDuration(float duration)
        {
            _delay = duration / _radialQtes.Count; 
        }

        private void OnQteResult(QteResult result, QteButtonView view)
        {
            if (result == QteResult.Perfect)
                _successRadialQte++;
            else if (result == QteResult.Fail)
                _failedRadialQte++;

            view.gameObject.SetActive(false);

            if (_successRadialQte + _failedRadialQte == _radialQtes.Count)
                Check();
        }

        private void OnClick()
        {
            if (_currentRadialQte >= _radialQtes.Count)
            {
                Check();
                return;
            }

            _radialQtes[_currentRadialQte++].Check();
        }

        private void Check()
        {
            if (_successRadialQte == _radialQtes.Count)
                OnReached?.Invoke(QteResult.Perfect);
            else if (_successRadialQte < _radialQtes.Count && _successRadialQte > 0)
                OnReached?.Invoke(QteResult.Normal);
            else
                OnReached?.Invoke(QteResult.Fail);
        }

        private IEnumerator PlayCoroutine()
        {
            for (int i = 0; i < _radialQtes.Count; i++)
            {
                _radialQtes[i].SetTargetTime(_delay*2);
                _radialQtes[i].gameObject.SetActive(true);

                yield return _spawnDelay;
            }
        }

        public override event Action<QteButtonView> Successed;
        public override event Action<QteButtonView> Invalided;
        public event Action<QteResult> OnReached;
    }
}