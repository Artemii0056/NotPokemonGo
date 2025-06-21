using System;
using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Characters;
using Infrastructure.StateMachine.States.Interfaces;
using InputServices;
using Services.SceneServices;
using UI.Ability;
using UI.Factory;
using Units;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public class BattleLoopState : IUpdateState, IPayloadedState<BattleLoopPayload>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;
        private readonly IRaycaster _raycaster;
        
        private IAbilityApplicatorService _abilityApplicatorService;
        
        private Battlefield _battlefield;
        private AbilitiesPanel _abilitiesPanel;

        public BattleLoopState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader, IUIFactory uiFactory, IRaycaster raycaster, IAbilityApplicatorService abilityApplicatorService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _raycaster = raycaster;
            _abilityApplicatorService = abilityApplicatorService;
        }

        // public void Enter(Battlefield battlefield,  AbilitiesPanel abilitiesPanel)
        // {
        //     _raycaster.UnitSearched += OnUnitSearched;
        // }

        public void Enter(BattleLoopPayload payload)
        {
            Debug.Log("Entering BattleLoopState");
            
            _battlefield = payload.Battlefield;
            _abilitiesPanel = payload.AbilitiesPanel;
            
            _raycaster.UnitSearched += OnUnitSearched;
        }

        public void Exit()
        {
            _raycaster.UnitSearched -= OnUnitSearched;
        }

        public void Update(float deltaTime)
        {
            _battlefield?.Tick(deltaTime);
        }
        
        private void OnUnitSearched(Unit unit)
        {
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
            Debug.Log("Ability Infos");
            _abilitiesPanel.SetAbilities(abilityModels);
        }
    }
}