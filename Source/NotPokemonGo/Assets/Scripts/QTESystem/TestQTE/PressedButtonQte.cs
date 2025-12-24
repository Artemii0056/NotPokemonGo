using System;
using Stats;
using UI.QTE;
using UnityEngine;

namespace QTESystem.TestQTE
{
    public class PressedButtonQte : QteButtonView //TODO Not Used
    {
        [SerializeField] private ButtonPointerUp _buttonPointerUp;

        public override event Action<QteButtonView> Successed;
        public override event Action<QteButtonView> Invalided;

        private void Start()
        {
            _buttonPointerUp.Up += OnPointerUp;
            _buttonPointerUp.Downed += OnPointerDown;
        }

        private void OnDisable()
        {
            _buttonPointerUp.Up -= OnPointerUp;
            _buttonPointerUp.Downed -= OnPointerDown;
        }

        private void OnPointerUp() => 
            Unit.ChangeStatValue(0, StatType.DodgeFlag);

        private void OnPointerDown() => 
            Unit.ChangeStatValue(1, StatType.DodgeFlag);
    }
}