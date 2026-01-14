using System;
using UI.QTE;
using UnityEngine;

namespace QTESystem.TestQTE
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

            // _partTimingBar.Play();
            // _partTimingBar.Finished += OnPartFinished;
            
            _timingBar.Play(_duration);
            _timingBar.Result += OnResult;
        }

        public void InitializeTime(float time)
        {
            _duration = time;
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

        private void OnPartFinished()
        {
            _partTimingBar.Finished -= OnPartFinished;
            _partTimingBar.DeactivateCursor();
            _timingBar.Play(_duration);

            _timingBar.Result += OnResult;
        }

        public void SetDuration(float duration)
        {
            _duration = duration;
        }

    }
}