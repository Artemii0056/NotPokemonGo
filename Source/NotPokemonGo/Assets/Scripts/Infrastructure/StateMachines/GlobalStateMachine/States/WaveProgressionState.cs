using Infrastructure.StateMachines.States.Interfaces;
using LevelSetting;
using Services;
using Services.BattleUnitContainers;
using UnityEngine;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class WaveProgressionState : IState
    {
        private readonly IBattlefieldFactory _battlefieldFactory;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ILevelProgressService _levelProgressService;

        public WaveProgressionState(
            IBattlefieldFactory battlefieldFactory, 
            IGameStateMachine gameStateMachine, 
            ILevelProgressService levelProgressService)
        {
            _battlefieldFactory = battlefieldFactory;
            _gameStateMachine = gameStateMachine;
            _levelProgressService = levelProgressService;
        }
        
        public void Enter() 
        {
            Debug.Log("Entering WaveProgressionState");
            
            var levelData = _levelProgressService.LevelData;
            
            if (levelData.HasNextWave == false)
            {
                //Переход в стейт финиш уровня
                Debug.Log("Финиш");
                return;
            }
            
            LevelPartSetup levelPartSetup = levelData.NextWave();
            
            Battlefield battlefield =
                _battlefieldFactory.Create(levelData.Units, levelPartSetup);
            
            _gameStateMachine.Enter<BattleLoopState, Battlefield>(battlefield);
        }

        public void Exit()
        {
        }
    }
}