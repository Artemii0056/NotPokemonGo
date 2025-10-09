using Infrastructure.StateMachines.States.Interfaces;
using LevelSetting;
using Platoons;
using Services;
using Services.BattleSessionService;
using Services.BattleUnitContainers;
using UnityEngine;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class WaveProgressionState : IPayloadedState<Platoon>, IState
    {
        private readonly IBattlefieldFactory _battlefieldFactory;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ILevelProgressService _levelProgressService;
        private readonly IBattlefieldSessionService _battlefieldSessionService;

        public WaveProgressionState(
            IBattlefieldFactory battlefieldFactory, 
            IGameStateMachine gameStateMachine, 
            ILevelProgressService levelProgressService, 
            IBattlefieldSessionService battlefieldSessionService)
        {
            _battlefieldFactory = battlefieldFactory;
            _gameStateMachine = gameStateMachine;
            _levelProgressService = levelProgressService;
            _battlefieldSessionService = battlefieldSessionService;
        }
        
        public void Enter(Platoon platoon) 
        {
            var levelData = _levelProgressService.LevelData;
            
            if (levelData.HasNextWave == false)
            {
                //Переход в стейт финиш уровня
                Debug.Log("Финиш");
                return;
            }
            
            LevelPartSetup levelPartSetup = levelData.NextWave();
            
            Battlefield battlefield =
                _battlefieldSessionService.StartNewBattle(levelData,levelPartSetup, platoon);
            
            _gameStateMachine.Enter<BattleLoopState, Battlefield>(battlefield);
        }

        public void Enter()
        {
            var levelData = _levelProgressService.LevelData;
            
            if (levelData.HasNextWave == false)
            {
                Debug.Log("Финиш");
                return;
            }
            
            Battlefield battlefield =
                _battlefieldSessionService.StartNewBattle(levelData, levelData.NextWave());
            
            _gameStateMachine.Enter<BattleLoopState, Battlefield>(battlefield);
        }
        
        public void Exit()
        {
        }
    }
}