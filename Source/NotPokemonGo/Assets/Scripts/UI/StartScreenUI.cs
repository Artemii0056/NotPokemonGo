using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartScreenUI : MonoBehaviour
    {
        [SerializeField] private Button _choosePlatoonButton;
        [SerializeField] private Button _showHeroesButton;
        
        public CharacterSelectionScreenPanel _characterSelectionScreenPanel{ get; private set; }
        public ChooseMapUI _chooseMapUI { get; private set; }

        public event Action ShowHeroesClicked;
        public event Action ChoosePlatoonClicked;

        public void Initialize(CharacterSelectionScreenPanel characterSelectionScreenPanel, ChooseMapUI chooseMapUI)
        {
            _characterSelectionScreenPanel = characterSelectionScreenPanel;
            _chooseMapUI = chooseMapUI;
            
            // _choosePlatoonButton.onClick.AddListener(OnChoosePlatoonButtonClicked);
            // _showHeroesButton.onClick.AddListener(OnShowHeroesButtonClicked);
            
            _characterSelectionScreenPanel.gameObject.SetActive(false);
        }
        
        private void OnEnable()
        {
            _choosePlatoonButton.onClick.AddListener(() => ChoosePlatoonClicked?.Invoke());
            _showHeroesButton.onClick.AddListener(() => ShowHeroesClicked?.Invoke());
        }

        private void OnDisable()
        {
            _choosePlatoonButton.onClick.RemoveAllListeners();
            _showHeroesButton.onClick.RemoveAllListeners();
        }

        // private void OnDisable()
        // {
        //     _choosePlatoonButton.onClick.RemoveListener(OnChoosePlatoonButtonClicked);
        //     _showHeroesButton.onClick.RemoveListener(OnShowHeroesButtonClicked);
        // }
        //
        // private void OnShowHeroesButtonClicked()
        // {
        //     _characterSelectionScreenPanel.gameObject.SetActive(true);
        //     _characterSelectionScreenPanel.ExitButton.onClick.AddListener(OnExitButtonClicked);
        //     _characterSelectionScreenPanel.Show();
        // }
        //
        // private void OnExitButtonClicked()
        // {
        //     _characterSelectionScreenPanel.gameObject.SetActive(false);
        //     _characterSelectionScreenPanel.ExitButton.onClick.RemoveListener(OnExitButtonClicked);
        //     _characterSelectionScreenPanel.Hide();
        // }
        //
        // private void OnChoosePlatoonButtonClicked()
        // {
        //     _characterSelectionScreenPanel.gameObject.SetActive(false);
        //     gameObject.SetActive(false);
        //     _chooseMapUI.gameObject.SetActive(true);
        // }
    }
}