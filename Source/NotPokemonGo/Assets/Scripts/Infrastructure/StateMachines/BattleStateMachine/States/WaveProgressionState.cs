using Infrastructure.StateMachines.GlobalStateMachine;
using Infrastructure.StateMachines.States.Interfaces;
using LevelSetting;
using Platoons;
using Services;
using Services.BattleSessionService;
using UnityEngine;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class WaveProgressionState : IPayloadedState<Platoon>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ILevelProgressService _levelProgressService;
        private readonly IBattlefieldSessionService _battlefieldSessionService;

        public WaveProgressionState(
            IGameStateMachine gameStateMachine, 
            ILevelProgressService levelProgressService, 
            IBattlefieldSessionService battlefieldSessionService)
        {
            _gameStateMachine = gameStateMachine;
            _levelProgressService = levelProgressService;
            _battlefieldSessionService = battlefieldSessionService;
        }
        
        public void Enter(Platoon platoon) 
        {
            var levelData = _levelProgressService.LevelData;
            
            if (levelData.HasNextWave == false)
            {
                _gameStateMachine.Enter<WinLevelState>();
                return;
            }
            
            LevelPartSetup levelPartSetup = levelData.NextWave();
            
            Battlefield battlefield =
                _battlefieldSessionService.StartNewBattle(levelData,levelPartSetup, platoon);
            
            levelData.BattleInfoUI.SetValue(_levelProgressService.LevelData.CurrentWaveIndex + 1);
            
            _gameStateMachine.Enter<BattleLoopState, Battlefield>(battlefield);
        }
        
        public void Exit()
        {
        }
    }
}