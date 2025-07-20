using Characters;
using Infrastructure.StateMachines.States.Interfaces;
using UI;
using UnityEngine;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class ShowHeroState : IPayloadedState<ShowHeroPayload>
    {
        private IGameStateMachine _gameStateMachine;
        private CharacterSelectionScreenContainer _characterSelectionScreenContainer;

        private CharacterPreviewPanel _characterPreviewPanel;
        private UnitContainerPanel _unitContainerPanel;
        private UnitStatsPanel _unitStatsPanel;
        
        private UnitSkinItemView _currentSkinItemView;
        private GameObject _currentUnitSkin;

        public ShowHeroState(IGameStateMachine gameStateMachine) => 
            _gameStateMachine = gameStateMachine;

        public void Enter(ShowHeroPayload payload)
        {
            _characterSelectionScreenContainer = payload.CharacterSelectionScreenContainer;
            _characterPreviewPanel = _characterSelectionScreenContainer.CharacterPreviewPanel;
            _unitContainerPanel = _characterSelectionScreenContainer.UnitContainerPanel;
            _unitStatsPanel = _characterSelectionScreenContainer.UnitStatsPanel;

            _characterSelectionScreenContainer.gameObject.SetActive(true);
            _characterSelectionScreenContainer.Show();
            _unitContainerPanel.Show();

            _characterSelectionScreenContainer.ExitClicked += OnExitClicked;
            _unitContainerPanel.Clicked += OnUnitSelected;
        }

        private void OnUnitSelected(UnitSkinItemView itemView)
        {
            if (_currentSkinItemView != null) 
                _currentSkinItemView.SetFree();
            
            _currentSkinItemView = itemView;
            _currentSkinItemView.SetBusy();
            
            GameObject characterPreviewPanel = Object.Instantiate(itemView.UnitItemConfig.CharacterModel);
            characterPreviewPanel.transform.rotation = new Quaternion(0, 180, 0, 0);
            
            _currentUnitSkin = characterPreviewPanel;
            _characterPreviewPanel.Setup(characterPreviewPanel);

            _unitStatsPanel.CreateItemViews(itemView.UnitItemConfig);
        }

        private void OnExitClicked()
        {
            _gameStateMachine.Enter<StartScreenState>();
        }

        public void Exit()
        {
            _characterSelectionScreenContainer.ExitClicked -= OnExitClicked;
            _characterSelectionScreenContainer.gameObject.SetActive(false);
            _unitContainerPanel.Clicked -= OnUnitSelected;
            
            _unitContainerPanel.Hide();
            _characterSelectionScreenContainer.Hide();

            if (_currentUnitSkin != null) 
                Object.Destroy(_currentUnitSkin);
        }
    }
}