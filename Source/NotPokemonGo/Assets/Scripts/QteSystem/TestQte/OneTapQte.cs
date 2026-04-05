using UnityEngine;
using UnityEngine.UI;

namespace QteSystem.TestQte
{
    public sealed class OneTapQte : QteInteractiveView, IHasQteOutcomeMode
    {
        [SerializeField] private Button _button;

        private QteOutcomeMode _outcomeMode;

        public void SetOutcomeMode(QteOutcomeMode mode)
        {
            _outcomeMode = mode;
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
            Complete(QteResult.Normal);
        }
    }
}