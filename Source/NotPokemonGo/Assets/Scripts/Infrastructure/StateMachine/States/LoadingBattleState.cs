using Abilities;
using Characters;
using Infrastructure.StateMachine.States.Interfaces;
using InputServices;
using Services.AssetManagement;
using Services.StaticDataServices;
using UI.Ability;
using VContainer;
using Object = UnityEngine.Object;

namespace Infrastructure.StateMachine.States
{
    public class LoadingBattleState : IPayloadedState<SpawnPositionType>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IBattlefieldFactory _battlefieldFactory;
        private readonly IResourceLoader _resourceLoader;
        private readonly IObjectResolver _objectResolver;
        private readonly IRaycaster _raycaster;
        private readonly IAbilityApplicatorService _abilityApplicatorService;
        
        private AbilitiesPanel _abilitiesPanel;
        private Battlefield _battlefield;

        public LoadingBattleState(
            IGameStateMachine gameStateMachine,
            IStaticDataService staticDataService,
            IBattlefieldFactory battlefieldFactory,
            IResourceLoader resourceLoader,
            IObjectResolver objectResolver,
            IRaycaster raycaster,
            IAbilityApplicatorService abilityApplicatorService
            )
        {
            _abilityApplicatorService = abilityApplicatorService;
            _raycaster = raycaster;
            _objectResolver = objectResolver;
            _resourceLoader = resourceLoader;
            _battlefieldFactory = battlefieldFactory;
            _staticDataService = staticDataService;
            _gameStateMachine = gameStateMachine;
        }
        
        public void Enter(SpawnPositionType spawnPositionType)
        {
            //_raycaster.UnitSearched += OnUnitSearched;
            
            SpawnPositionConfig spawnPositionConfigFirstCommand = _staticDataService.GetSpawnPositionConfig(spawnPositionType);
            SpawnPositionConfig spawnPositionConfigSecondCommand = _staticDataService.GetSpawnPositionConfig(spawnPositionType);
            
            _battlefield = _battlefieldFactory.Create(spawnPositionConfigFirstCommand, spawnPositionConfigSecondCommand);

            AbilitiesPanel abilitiesPanel = _resourceLoader.Load<AbilitiesPanel>(Constants.AssetPath.AbilitiesPanelPath);
            
            _abilitiesPanel = Object.Instantiate(abilitiesPanel);
            _objectResolver.Inject(_abilitiesPanel);
            AbilityPanelPresenter abilityPanelPresenter = new AbilityPanelPresenter(_abilitiesPanel);
            
            abilityPanelPresenter.Enable();
            
            //_gameStateMachine.Enter<BattleLoopState>();

           // BattleLoopPayload battleLoopPayload = new BattleLoopPayload(_abilitiesPanel, _battlefield);
            _gameStateMachine.Enter<BattleLoopState, BattleLoopPayload>(new BattleLoopPayload(_abilitiesPanel, _battlefield));  //ВОТ ТУТ
        }

        public void Exit()
        {
            //_raycaster.UnitSearched -= OnUnitSearched;
        }

        // public void Update(float deltaTime)
        // {
        //     _battlefield?.Tick(Time.deltaTime);
        // }

        // private void OnUnitSearched(Unit unit)
        // {
        //     switch (unit.PlatoonType)
        //     {
        //         case PlatoonType.Friends:
        //             ShowAbilityInfos(unit.AbilityModels);
        //             _abilityApplicatorService.RememberSource(unit);
        //             break;
        //         
        //         case PlatoonType.Enemies:
        //             _abilityApplicatorService.Apply(unit);
        //             break;
        //         
        //         default:
        //             throw new ArgumentOutOfRangeException();
        //     }
        //     
        //     // if (unit.PlatoonType == PlatoonType.Friends)
        //     // {
        //     //     
        //     // }
        //     // else if (unit.PlatoonType == PlatoonType.Enemies)
        //     // {
        //     //     
        //     // }
        // }

        // private void ShowAbilityInfos(List<AbilityModel> abilityModels)
        // {
        //     Debug.Log("Ability Infos");
        //     _abilitiesPanel.SetAbilities(abilityModels);
        // }
    }
}