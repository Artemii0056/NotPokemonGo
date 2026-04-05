using UnityEngine;

namespace QteSystem.TestQte
{
    public sealed class TimingBarQte : QteInteractiveView, IHasQteDuration, IHasQteOutcomeMode
    {
        [SerializeField] private TimingBar _timingBar;
        [SerializeField] private KeyCode _inputKey = KeyCode.Space;

        private bool _isRunning;
        private float _duration;
        private QteOutcomeMode _outcomeMode;

        public void SetDuration(float duration)
        {
            _duration = duration;
        }

        public void SetOutcomeMode(QteOutcomeMode mode)
        {
            _outcomeMode = mode;
        }

        private void Start()
        {
            _timingBar.Result += OnResult;
            _timingBar.Play(_duration);
            _isRunning = true;
        }

        private void Update()
        {
            if (!_isRunning)
                return;

            if (Input.GetKeyDown(_inputKey))
            {
                _isRunning = false;
                _timingBar.Evaluate();
            }
        }

        private void OnDisable()
        {
            _timingBar.Result -= OnResult;
        }

        private void OnResult(QteResult result)
        {
            if (_outcomeMode == QteOutcomeMode.Binary && result == QteResult.Perfect)
                result = QteResult.Normal;

            Complete(result);
        }
    }
}