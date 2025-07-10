using System.Collections.Generic;
using LevelSetting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartScreenUI : MonoBehaviour
    {
        [SerializeField] private Button _choosePlatoonButton;
        [SerializeField] private Button _showHeroesButton;
        
        [SerializeField] private List<LevelConfig> _levelConfigs;
        
        private CharacterSelectionScreenPanel _characterSelectionScreenPanel;
        private ChooseMapUI _chooseMapUI;

        public void Initialize(CharacterSelectionScreenPanel characterSelectionScreenPanel, ChooseMapUI chooseMapUI)
        {
            _characterSelectionScreenPanel = characterSelectionScreenPanel;
            _chooseMapUI = chooseMapUI;
            
            _choosePlatoonButton.onClick.AddListener(OnChoosePlatoonButtonClicked);
            _showHeroesButton.onClick.AddListener(OnShowHeroesButtonClicked);
            
            _characterSelectionScreenPanel.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _choosePlatoonButton.onClick.RemoveListener(OnChoosePlatoonButtonClicked);
            _showHeroesButton.onClick.RemoveListener(OnShowHeroesButtonClicked);
        }

        private void OnShowHeroesButtonClicked()
        {
            _characterSelectionScreenPanel.gameObject.SetActive(true);
            _characterSelectionScreenPanel.ExitButton.onClick.AddListener(OnExitButtonClicked);
            _characterSelectionScreenPanel.Show();
        }

        private void OnExitButtonClicked()
        {
            _characterSelectionScreenPanel.gameObject.SetActive(false);
            _characterSelectionScreenPanel.ExitButton.onClick.RemoveListener(OnExitButtonClicked);
            _characterSelectionScreenPanel.Hide();
        }

        private void OnChoosePlatoonButtonClicked()
        {
            _characterSelectionScreenPanel.gameObject.SetActive(false);
            gameObject.SetActive(false);
            _chooseMapUI.gameObject.SetActive(true);
        }
    }
}