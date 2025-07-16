using System.Collections.Generic;
using Infrastructure.StateMachines.States.Interfaces;
using Map;
using UI;
using UnityEngine;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class ChooseMapState : IPayloadedState<ChoosePlatoonPayload>
    {
        private ChooseMapUI _characterSelectionScreen; 
        private UnitSelectionController _unitSelectionController;
        private IGameStateMachine _gameStateMachine;
        private List<MapLevel> _maps;
        
        private MapLevel _currentMap;

        public void Enter(ChoosePlatoonPayload payload)
        {
            _characterSelectionScreen = payload.ChooseMapUI;
            _gameStateMachine = payload.GameStateMachine;
            _unitSelectionController = _characterSelectionScreen.UnitSelectionController;
            
            _characterSelectionScreen.gameObject.SetActive(true);

            _characterSelectionScreen.MapChoosed += OnMapChoosed;

            _maps = _characterSelectionScreen.MapLevels;
        }

        private void OnMapChoosed(MapType type)
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
            Debug.Log("OnPlayButtonClicked");
            _unitSelectionController.gameObject.SetActive(true); //перенести в стейт
            _gameStateMachine.Enter<ChooseUnitToFightState>();
        }

        public void Exit()
        {
            _currentMap.OnPlayButtonClicked -= OnPlayButtonClicked;
            _currentMap.ExitButtonClicked -= OnExitButtonClicked;
        }
    }
}