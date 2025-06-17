using Characters;
using Infrastructure.StateMachine.States.Interfaces;
using Services.StaticDataServices;

namespace Infrastructure.StateMachine.States
{
    public class LoadingBattleState : IPayloadedState<LocationTypeInfo>
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

        public void Enter(LocationTypeInfo payload)
        {
            // пока получаем только "аля сколько бойцов" в группе через enum. 
            SpawnPositionConfig spawnPositionConfigFirstCommand = _staticDataService.GetLocationTypeConfig(payload.SpawnPositionTypeFirstCommand);
            SpawnPositionConfig spawnPositionConfigSecondCommand = _staticDataService.GetLocationTypeConfig(payload.SpawnPositionTypeSecondCommand);
            
            _battlefieldFactory.Create(spawnPositionConfigFirstCommand, spawnPositionConfigSecondCommand);
            
            _gameStateMachine.Enter<BattleLoopState>();
        }

        public void Exit()
        {
        }
    }
}