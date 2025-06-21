using Characters;
using Infrastructure.StateMachine.States.Interfaces;
using Services.StaticDataServices;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public class LoadingBattleState : IPayloadedState<SpawnPositionType>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IBattlefieldFactory _battlefieldFactory;

        public LoadingBattleState(IGameStateMachine gameStateMachine, IStaticDataService staticDataService, IBattlefieldFactory battlefieldFactory)
        {
            _battlefieldFactory = battlefieldFactory;
            _staticDataService = staticDataService;
            _gameStateMachine = gameStateMachine;
        }
        
        public void Enter(SpawnPositionType spawnPositionType)
        {
            SpawnPositionConfig spawnPositionConfigFirstCommand = _staticDataService.GetSpawnPositionConfig(spawnPositionType);
            SpawnPositionConfig spawnPositionConfigSecondCommand = _staticDataService.GetSpawnPositionConfig(spawnPositionType);
            
            Battlefield battlefield = _battlefieldFactory.Create(spawnPositionConfigFirstCommand, spawnPositionConfigSecondCommand);
            
            //_gameStateMachine.Enter<BattleLoopState>();
            
            Debug.Log($"Entering {nameof(LoadingBattleState)}");
            
            battlefield.Tick(Time.deltaTime);
        }

        public void Exit()
        {
        }
    }
}