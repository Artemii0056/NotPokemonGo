using System;
using System.Collections.Generic;
using Abilities.MV;
using Animations;
using Infrastructure.StateMachines.States.Interfaces;
using InputServices;
using UI.Ability;
using Units;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class UnitActionState : IPayloadedState<Unit>
    {
        private readonly IRaycaster _raycaster;
        private readonly ISourceProvider _sourceProvider;
        private readonly ITargetSelector _targetSelector;
        private readonly IAbilityProvider _abilityProvider;
        private readonly AbilityPanelPresenter _abilityPanelPresenter;

        public UnitActionState(
            IRaycaster raycaster,
            ISourceProvider sourceProvider,
            ITargetSelector targetSelector,
            IAbilityProvider abilityProvider,
            AbilityPanelPresenter abilityPanelPresenter)
        {
            _raycaster = raycaster;
            _sourceProvider = sourceProvider;
            _targetSelector = targetSelector;
            _abilityProvider = abilityProvider;
            _abilityPanelPresenter = abilityPanelPresenter;
        }
        
        public void Enter(Unit unit)
        {
            _raycaster.UnitSearched += OnUnitSearched;
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
                    _targetSelector.Remember(unit, _abilityProvider.AbilityModel.TargetMode); // запоминаем цель
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void ShowAbilityInfos(List<AbilityModel> abilityModels)
        {
            _abilityPanelPresenter.Enable();
            _abilityPanelPresenter.FillAbilityView(abilityModels);
        }
    }    
}
