using System;
using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Infrastructure.StateMachines.GlobalStateMachine.Payloads;
using Infrastructure.StateMachines.States.Interfaces;
using InputServices;
using Services.SceneServices;
using UI.Ability;
using UI.Factory;
using Units;
using UnityEngine;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class BattleLoopState : IUpdateState, IPayloadedState<BattleLoopPayload>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;
        private readonly IRaycaster _raycaster;
        
        private IAbilityApplicatorService _abilityApplicatorService;
        private readonly IBattleStateMachine _battleStateMachine;

        private Battlefield _battlefield;
        private AbilitiesPanel _abilitiesPanel;

        public BattleLoopState(
            IGameStateMachine gameStateMachine, 
            ISceneLoader sceneLoader, 
            IUIFactory uiFactory, 
            IRaycaster raycaster, 
            IAbilityApplicatorService abilityApplicatorService,
            IBattleStateMachine battleStateMachine
            )
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _raycaster = raycaster;
            _abilityApplicatorService = abilityApplicatorService;
            _battleStateMachine = battleStateMachine;
        }

        public void Enter(BattleLoopPayload payload)
        {
            //_battleStateMachine.Enter<InitializeBattleState>();
            
            
            
            _battlefield = payload.Battlefield;
            _abilitiesPanel = payload.AbilitiesPanel;
            _raycaster.UnitSearched += OnUnitSearched;
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
            Debug.LogError("Залупа");
            switch (unit.PlatoonType)
            {
                case PlatoonType.Friends:
                    ShowAbilityInfos(unit.AbilityModels);
                    _abilityApplicatorService.RememberSource(unit);
                    break;
                
                case PlatoonType.Enemies:
                    _abilityApplicatorService.Apply(unit);
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