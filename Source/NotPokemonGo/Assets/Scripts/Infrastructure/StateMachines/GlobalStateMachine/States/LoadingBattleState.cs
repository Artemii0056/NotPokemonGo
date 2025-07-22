using System.Collections.Generic;
using Characters.Configs;
using Infrastructure.StateMachines.States.Interfaces;
using LevelSetting;
using Services.BattleUnitContainers;
using Services.StaticDataServices;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class LoadingBattleState : IPayloadedState<LoadingBattleStatePayload> 
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IBattlefieldFactory _battlefieldFactory;

        public LoadingBattleState(
            IGameStateMachine gameStateMachine,
            IStaticDataService staticDataService,
            IBattlefieldFactory battlefieldFactory)
        {
            _battlefieldFactory = battlefieldFactory;
            _staticDataService = staticDataService;
            _gameStateMachine = gameStateMachine;
        }

        public void Enter(LoadingBattleStatePayload payload)
        {
            List<UnitType> units = payload.UnitTypes;
            LevelConfig levelConfig = payload.LevelConfig;

            Battlefield battlefield =
                _battlefieldFactory.Create(units, levelConfig);
            
            _gameStateMachine.Enter<BattleLoopState, Battlefield>(battlefield);
        }

        public void Exit()
        {
        }
    }
}