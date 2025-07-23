using System.Collections.Generic;
using Characters.Configs;
using Infrastructure.StateMachines.States.Interfaces;
using Services;
using Services.BattleUnitContainers;
using Services.StaticDataServices;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class LoadingBattleState : IPayloadedState<LoadingBattleStatePayload>, IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IBattlefieldFactory _battlefieldFactory;
        private readonly ILevelProgressService _levelProgressService;
        
        private LoadingBattleStatePayload _payload;

        public LoadingBattleState(
            IGameStateMachine gameStateMachine,
            IStaticDataService staticDataService,
            IBattlefieldFactory battlefieldFactory, 
            ILevelProgressService levelProgressService)
        {
            _battlefieldFactory = battlefieldFactory;
            _levelProgressService = levelProgressService;
            _staticDataService = staticDataService;
            _gameStateMachine = gameStateMachine;
        }

        public void Enter(LoadingBattleStatePayload levelData)
        {
            _payload = levelData;
             
             List<UnitType> units = levelData.UnitTypes;
            
            LevelRuntimeDataPayload levelRuntimeDataPayload = new LevelRuntimeDataPayload(levelData.LevelConfig, units);
            
            _levelProgressService.Set(levelRuntimeDataPayload);
            
            _gameStateMachine.Enter<WaveProgressionState>();
        }

        public void Enter()
        {
            List<UnitType> units = _payload.UnitTypes;
            
            LevelRuntimeDataPayload levelRuntimeDataPayload = new LevelRuntimeDataPayload(_payload.LevelConfig, units);
            
            _levelProgressService.Set(levelRuntimeDataPayload);
            
            _gameStateMachine.Enter<WaveProgressionState>();
        }
        
        public void Exit()
        {
        }
    }
}