using System;
using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Animations;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.GlobalStateMachine.Payloads;
using Infrastructure.StateMachines.States.Interfaces;
using InputServices;
using Services;
using Services.SceneServices;
using Services.StaticDataServices;
using UI.Ability;
using UI.Factory;
using Units;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class BattleLoopState : IUpdateState, IPayloadedState<BattleLoopPayload>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;
        private readonly IRaycaster _raycaster;
        private ISourceProvider _sourceProvider;
        private IAbilityProvider _abilityProvider;
        
        private IAbilityApplicatorService _abilityApplicatorService;
        private readonly IBattleStateMachine _battleStateMachine;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IStaticDataService _staticDataService;

        private Battlefield _battlefield;
        private AbilitiesPanel _abilitiesPanel;
        private ITargetSelector _targetSelector;

        public BattleLoopState(
            IGameStateMachine gameStateMachine, 
            ISceneLoader sceneLoader, 
            IUIFactory uiFactory, 
            IRaycaster raycaster, 
            IAbilityApplicatorService abilityApplicatorService,
            IBattleStateMachine battleStateMachine, 
            ISourceProvider sourceProvider,
            IAbilityProvider abilityProvider,
            ICoroutineRunner coroutineRunner,
            IStaticDataService staticDataService, ITargetSelector targetSelector)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _raycaster = raycaster;
            _abilityApplicatorService = abilityApplicatorService;
            _battleStateMachine = battleStateMachine;
            _sourceProvider = sourceProvider;
            _abilityProvider = abilityProvider;
            _coroutineRunner = coroutineRunner;
            _staticDataService = staticDataService;
            _targetSelector = targetSelector;
        }

        public void Enter(BattleLoopPayload payload)
        {
            _battlefield = payload.Battlefield;
            _abilitiesPanel = payload.AbilitiesPanel;
            _raycaster.UnitSearched += OnUnitSearched;
            
            _targetSelector.SetPlatoons(_battlefield.EnemyPlatoon, _battlefield.Platoon2);
        }
        
        public void Update(float deltaTime)
        {
            _battlefield?.Tick(deltaTime);
        }

        public void Exit()
        {
            _raycaster.UnitSearched -= OnUnitSearched;
        }
        
        private void OnUnitSearched(Unit unit)
        {
            switch (unit.PlatoonType)
            {
                case PlatoonType.Friends:
                    ShowAbilityInfos(unit.AbilityModels);
                    _sourceProvider.Remember(unit);
                    //_abilityApplicatorService.RememberSource(unit);
                    break;
                
                case PlatoonType.Enemies: //Вот по ходу атсюдава дернуть
                    AnimationProcessingService animationProcessingService = new AnimationProcessingService();
                    animationProcessingService.PlayAnimation(_sourceProvider.Source, _abilityProvider.AbilityModel);
                    _targetSelector.Remember(unit, _abilityProvider.AbilityModel.TargetMode);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void ShowAbilityInfos(List<AbilityModel> abilityModels)
        {
            _abilitiesPanel.SetAbilities(abilityModels);
        }
    }
}