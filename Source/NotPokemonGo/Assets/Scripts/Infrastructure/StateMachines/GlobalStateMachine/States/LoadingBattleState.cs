using Battlefields;
using Characters;
using Infrastructure.StateMachines.GlobalStateMachine.Payloads;
using Infrastructure.StateMachines.States;
using Infrastructure.StateMachines.States.Interfaces;
using Services.AssetManagement;
using Services.StaticDataServices;
using UI.Ability;
using VContainer;
using Object = UnityEngine.Object;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class LoadingBattleState : IPayloadedState<SpawnPositionType>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IBattlefieldFactory _battlefieldFactory;
        private readonly IResourceLoader _resourceLoader;
        private readonly IObjectResolver _objectResolver;

        public LoadingBattleState(
            IGameStateMachine gameStateMachine,
            IStaticDataService staticDataService,
            IBattlefieldFactory battlefieldFactory,
            IResourceLoader resourceLoader,
            IObjectResolver objectResolver
        )
        {
            _objectResolver = objectResolver;
            _resourceLoader = resourceLoader;
            _battlefieldFactory = battlefieldFactory;
            _staticDataService = staticDataService;
            _gameStateMachine = gameStateMachine;
        }

        public void Enter(SpawnPositionType spawnPositionType)
        {
            SpawnPositionConfig spawnPositionConfigFirstCommand = _staticDataService.GetSpawnPositionConfig(spawnPositionType);
            SpawnPositionConfig spawnPositionConfigSecondCommand = _staticDataService.GetSpawnPositionConfig(spawnPositionType);

            Battlefield battlefield =
                _battlefieldFactory.Create(spawnPositionConfigFirstCommand, spawnPositionConfigSecondCommand);

            AbilitiesPanel abilitiesPanelAsset =
                _resourceLoader.Load<AbilitiesPanel>(Constants.AssetPath.AbilitiesPanelPath);

            AbilitiesPanel abilitiesPanel = Object.Instantiate(abilitiesPanelAsset);
            _objectResolver.Inject(abilitiesPanel);
            AbilityPanelPresenter abilityPanelPresenter = new AbilityPanelPresenter(abilitiesPanel);

            abilityPanelPresenter.Enable();

            _gameStateMachine.Enter<BattleLoopState, BattleLoopPayload>(new BattleLoopPayload(abilitiesPanel,
                battlefield));
        }

        public void Exit()
        {
        }
    }
}