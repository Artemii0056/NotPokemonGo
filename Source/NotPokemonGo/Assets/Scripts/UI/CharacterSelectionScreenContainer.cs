using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CharacterSelectionScreenContainer : MonoBehaviour
    {
        [field: SerializeField] public CharacterPreviewPanel CharacterPreviewPanel { get; private set; }
        [field: SerializeField] public UnitContainerPanel UnitContainerPanel { get; private set; }
        [field: SerializeField] public UnitStatsPanel UnitStatsPanel { get; private set; }
        
        [SerializeField] private Button _exitButton;

        public Action ExitClicked;

        private void OnEnable() => 
            _exitButton.onClick.AddListener(() => ExitClicked?.Invoke());

        private void OnDisable() => 
            _exitButton.onClick.RemoveAllListeners();

        public void Show() => 
            gameObject.SetActive(true);

        public void Hide() => 
            gameObject.SetActive(false);
    }
}