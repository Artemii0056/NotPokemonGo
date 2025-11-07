using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoosePanel : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _backMainMenuButton;
    
        public event Action OnRestartButtonPressed;
        public event Action OnBackMainMenuButtonPressed;

        private void OnEnable()
        {
            _restartButton.onClick.AddListener(() => OnRestartButtonPressed?.Invoke());
            _backMainMenuButton.onClick.AddListener(() => OnBackMainMenuButtonPressed?.Invoke());
        }

        private void OnDisable()
        {
            _restartButton.onClick.RemoveAllListeners();
            _backMainMenuButton.onClick.RemoveAllListeners();
        }
    }
}