using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleUpgrages
{
    public class BattleUpgradeButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        public event Action UpgradeSelected;

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
            UpgradeSelected?.Invoke();
        }
    }
}