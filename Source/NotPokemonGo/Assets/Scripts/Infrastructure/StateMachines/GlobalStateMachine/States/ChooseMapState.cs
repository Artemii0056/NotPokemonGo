using System.Collections.Generic;
using Infrastructure.StateMachines.States.Interfaces;
using LevelSetting;
using Map;
using UI;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class ChooseMapState : IPayloadedState<ChooseMapUI>
    {
        private readonly IGameStateMachine _gameStateMachine;
        
        private ChooseMapUI _characterSelectionScreen; 
        private ChooseUnitToFightPanel _chooseUnitToFightPanel;
        private List<MapLevel> _maps;
        
        private MapLevel _currentMap;

        public ChooseMapState(IGameStateMachine gameStateMachine) => 
            _gameStateMachine = gameStateMachine;

        public void Enter(ChooseMapUI battlefield)
        {
            _characterSelectionScreen = battlefield;
            _chooseUnitToFightPanel = _characterSelectionScreen.ChooseUnitToFightPanel;
            
            _characterSelectionScreen.gameObject.SetActive(true);

            _characterSelectionScreen.MapSelected += OnMapSelected;

            _maps = _characterSelectionScreen.MapLevels;
        }
        
        public void Exit()
        {
            _currentMap.OnPlayButtonClicked -= OnPlayButtonClicked;
            _currentMap.ExitButtonClicked -= OnExitButtonClicked;
            _currentMap.gameObject.SetActive(false);
            _currentMap = null;
            _maps = null;
            
            _characterSelectionScreen.MapSelected -= OnMapSelected;
            
            _characterSelectionScreen.gameObject.SetActive(false);
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
            LevelConfig config = _currentMap.CurrentLevelConfig;
            ChooseUnitToFightPayload chooseUnitToFightPayload = new ChooseUnitToFightPayload(_chooseUnitToFightPanel, config);
            
            _gameStateMachine.Enter<ChooseUnitToFightState, ChooseUnitToFightPayload>(chooseUnitToFightPayload); 
        }
    }
}