using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ChooseUnitToFightPanel : MonoBehaviour
    {
        [field: SerializeField] public UnitSelectionPanelContainer UnitSelectionPanelContainer;
        [field: SerializeField] public UnitContainerPanel UnitContainerPanel { get; private set; }

        [SerializeField] private Button _startButton;
        [SerializeField] private Button _exitButton;

        public event Action StartButtonClicked;
        public event Action ExitButtonClicked;

        public void Show()
        {
            gameObject.SetActive(true);
            
            _startButton.onClick.AddListener(() => StartButtonClicked?.Invoke());
            _exitButton.onClick.AddListener(() => ExitButtonClicked?.Invoke());
        }
        
        public void Hide()
        {
            _startButton.onClick.RemoveAllListeners();
            _exitButton.onClick.RemoveAllListeners();
            
            gameObject.SetActive(false);
        }
    }
}