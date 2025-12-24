using System;
using Stats;
using UI.QTE;
using UnityEngine;

namespace QTESystem.TestQTE
{
    public class TimingBarQte : QteButtonView
    {
        [SerializeField] private PartTimingBar _partTimingBar; //TODO нужно получить скорость полета из точки в точку 
        [SerializeField] private TimingBar _timingBar;

        [SerializeField] private KeyCode _inputKey = KeyCode.Space;

        private bool _isRun;

        public override event Action<QteButtonView> Successed;
        public override event Action<QteButtonView> Invalided;

        public void Start()
        {
            _isRun = true;

            _partTimingBar.Play();
            _partTimingBar.Finished += OnPartFinished;
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

        private void OnDisable()
        {
            _timingBar.Fail -= OnFail;
            _timingBar.Ok -= OnOk;
            _timingBar.Perfect -= OnPerfect;
        }

        private void OnPartFinished()
        {
            _partTimingBar.DeactivateCursor();
            _partTimingBar.Finished -= OnPartFinished;
            _timingBar.Play();

            _timingBar.Fail += OnFail;
            _timingBar.Ok += OnOk;
            _timingBar.Perfect += OnPerfect;
        }

        private void OnPerfect()
        {
            Unit.ChangeStatValue(1, StatType.DodgeFlag);

            Debug.Log("Perfect");
        }

        private void OnOk() =>
            Debug.Log("OnOk");

        private void OnFail() =>
            Debug.Log("OnFail");
    }
}