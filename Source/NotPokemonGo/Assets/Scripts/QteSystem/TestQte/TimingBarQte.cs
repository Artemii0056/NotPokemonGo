using System;
using UI.QTE;
using UnityEngine;

namespace QteSystem.TestQte
{
    public class TimingBarQte : QteButtonView, IHasQteDuration, IProvidesQteResult
    {
        [SerializeField] private PartTimingBar _partTimingBar;  
        [SerializeField] private TimingBar _timingBar;

        [SerializeField] private KeyCode _inputKey = KeyCode.Space;

        private bool _isRun;
        private float _duration;

        public override event Action<QteButtonView> Successed;
        public override event Action<QteButtonView> Invalided;
        
        public event Action<QteResult> OnReached;

        public void Start()
        {
            _isRun = true;

            _timingBar.Play(_duration);
            _timingBar.Result += OnResult;
        }

        private void Update()
        {
            if (_isRun == false)
                return;

            if (Input.GetKeyDown(_inputKey))
            {
                _timingBar.Evaluate();
                _isRun = false;
            }
        }

        private void OnDisable() => 
            _timingBar.Result -= OnResult;

        private void OnResult(QteResult result) => 
            OnReached?.Invoke(result);

        public void SetDuration(float duration) => 
            _duration = duration;
    }
}