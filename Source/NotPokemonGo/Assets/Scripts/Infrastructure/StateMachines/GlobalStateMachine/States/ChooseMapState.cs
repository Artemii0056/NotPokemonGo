using System.Collections.Generic;
using Infrastructure.StateMachines.States.Interfaces;
using LevelSetting;
using Map;
using UI;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class ChooseMapState : IPayloadedState<ChoosePlatoonPayload>
    {
        private ChooseMapUI _characterSelectionScreen; 
        private ChooseUnitToFightPanel _chooseUnitToFightPanel;
        private IGameStateMachine _gameStateMachine;
        private List<MapLevel> _maps;
        
        private MapLevel _currentMap;

        public void Enter(ChoosePlatoonPayload payload)
        {
            _characterSelectionScreen = payload.ChooseMapUI;
            _gameStateMachine = payload.GameStateMachine;
            _chooseUnitToFightPanel = _characterSelectionScreen.ChooseUnitToFightPanel;
            
            _characterSelectionScreen.gameObject.SetActive(true);

            _characterSelectionScreen.MapSelected += OnMapSelected;

            _maps = _characterSelectionScreen.MapLevels;
        }

        private void OnMapSelected(MapType type)
        {
            foreach (var map in _maps)
            {
                if (map.MapType == type )
                {
                    _characterSelectionScreen.gameObject.SetActive(false);

                    _currentMap = map;
                    map.gameObject.SetActive(true);
                    _currentMap.OnPlayButtonClicked += OnPlayButtonClicked;
                    _currentMap.ExitButtonClicked += OnExitButtonClicked;
                    break;
                }
            }
        }

        private void OnExitButtonClicked() => 
            _gameStateMachine.Enter<StartScreenState>();

        private void OnPlayButtonClicked() 
        {
            _chooseUnitToFightPanel.gameObject.SetActive(true);
            
            LevelConfig config = _currentMap.CurrentLevelConfig;
            ChooseUnitToFightPayload chooseUnitToFightPayload = new ChooseUnitToFightPayload(_chooseUnitToFightPanel, config);
            
            _gameStateMachine.Enter<ChooseUnitToFightState, ChooseUnitToFightPayload>(chooseUnitToFightPayload); 
        }

        public void Exit()
        {
            _currentMap.OnPlayButtonClicked -= OnPlayButtonClicked;
            _currentMap.ExitButtonClicked -= OnExitButtonClicked;
        }
    }
}