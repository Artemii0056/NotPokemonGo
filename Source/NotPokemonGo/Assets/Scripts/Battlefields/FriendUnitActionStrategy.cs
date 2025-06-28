using System;
using System.Collections.Generic;
using Abilities.MV;
using Animations;
using InputServices;
using UI.Ability;
using Units;
using VContainer;

namespace Battlefields
{
    public class FriendUnitActionStrategy : UnitActionStrategy
    {
        private readonly Battlefield _battlefield;

        private IRaycaster _raycaster;
        private ISourceProvider _sourceProvider;
        private IAbilityProvider _abilityProvider;
        private ITargetSelector _targetSelector;
        private AbilityPanelPresenter _abilityPanelPresenter;

        public FriendUnitActionStrategy(Battlefield battlefield)
        {
            _battlefield = battlefield;
        }

        [Inject]
        public void Initialize(
            IRaycaster raycaster,
            ISourceProvider sourceProvider,
            IAbilityProvider abilityProvider,
            ITargetSelector targetSelector,
            AbilityPanelPresenter abilityPanelPresenter)
        {
            _abilityProvider = abilityProvider;
            _raycaster = raycaster;
            _sourceProvider = sourceProvider;
            _targetSelector = targetSelector;
            _abilityPanelPresenter = abilityPanelPresenter;
        }
        
        public override void Enable()
        {
            base.Enable();
            _raycaster.UnitSearched += OnUnitSearched;
        }

        public override void Disable()
        {
            base.Disable();
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