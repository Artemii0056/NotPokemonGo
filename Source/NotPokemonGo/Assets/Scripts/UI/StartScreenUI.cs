using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartScreenUI : MonoBehaviour
    {
        [SerializeField] private Button _choosePlatoonButton;
        [SerializeField] private Button _showHeroesButton;
        
        public CharacterSelectionScreenContainer CharacterSelectionScreenContainer{ get; private set; }
        public ChooseMapUI ChooseMapUI { get; private set; }

        public event Action ShowHeroesClicked;
        public event Action ChoosePlatoonClicked;

        public void Initialize(CharacterSelectionScreenContainer characterSelectionScreenContainer, ChooseMapUI chooseMapUI)
        {
            CharacterSelectionScreenContainer = characterSelectionScreenContainer;
            ChooseMapUI = chooseMapUI;
            
            CharacterSelectionScreenContainer.gameObject.SetActive(false);
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
    }
}