using System;
using Statuses;
using Statuses.Services;
using UI.QTE;
using UnityEngine;
using UnityEngine.UI;

namespace QteSystem.TestQTE
{
    public class OneTapQte : QteButtonView
    {
        [SerializeField] private Button _button;
        [SerializeField] private StatusSetup _statusSetup;

        private IStatusFactory _statusFactory;
        private IStatusResolver _statusResolver;

        public override event Action<QteButtonView> Successed;
        public override event Action<QteButtonView> Invalided;

        public void Initialize(IStatusFactory statusFactory, IStatusResolver statusResolver)
        {
            _statusFactory = statusFactory;
            _statusResolver = statusResolver;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            Debug.Log("OnClick");
            _statusResolver.Resolve(_statusFactory.Create(_statusSetup, Unit),Unit );
        }
    }
}