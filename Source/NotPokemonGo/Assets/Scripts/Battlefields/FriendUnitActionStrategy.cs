using System;
using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Animations;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using InputServices;
using Services.StaticDataServices;
using UI.Ability;
using Units;
using UnityEngine;
using VContainer;

namespace Battlefields
{
    public class FriendUnitActionStrategy : UnitActionStrategy
    {
        private readonly Battlefield _battlefield;
        private readonly Unit _source;

        private IRaycaster _raycaster;
        private ISourceProvider _sourceProvider;
        private IAbilityProvider _abilityProvider;
        private ITargetSelector _targetSelector;
        private AbilityPanelPresenter _abilityPanelPresenter;
        private IBattleStateMachine _battleStateMachine;
        private AnimationProcessingService _animationProcessingService;

        public FriendUnitActionStrategy(Battlefield battlefield, Unit source)
        {
            _source = source;
            _battlefield = battlefield;
        }

        [Inject]
        public void Initialize(
            IRaycaster raycaster,
            ISourceProvider sourceProvider,
            IAbilityProvider abilityProvider,
            ITargetSelector targetSelector,
            IBattleStateMachine battleStateMachine,
            AbilityPanelPresenter abilityPanelPresenter
        )
        {
            _battleStateMachine = battleStateMachine;
            _abilityProvider = abilityProvider;
            _raycaster = raycaster;
            _sourceProvider = sourceProvider;
            _targetSelector = targetSelector;
            _abilityPanelPresenter = abilityPanelPresenter;
            _animationProcessingService = new AnimationProcessingService();
        }

        public override void Enable()
        {
            base.Enable();
            ShowAbilityInfos(_source.AbilityModels);
            _sourceProvider.Remember(_source);

            _raycaster.UnitSearched += OnUnitSearched;
            _source.AnimationActionEnded += OnAnimationActionEnded;
        }

        public override void Disable()
        {
            base.Disable();
            _raycaster.UnitSearched -= OnUnitSearched;
            _source.AnimationActionEnded -= OnAnimationActionEnded;
            _sourceProvider.Discard();
        }

        private void OnUnitSearched(Unit unit)
        {
            if (_abilityProvider.AbilityModel == null)
                return;

            if (_source == unit)
                return;

            switch (unit.PlatoonType)
            {
                case PlatoonType.Friends:
                    Debug.Log("Выбрали союзника");
                    break;

                case PlatoonType.Enemies: //Вот по ходу атсюдава дернуть
                    _animationProcessingService.PlayAnimation(_sourceProvider.Source, _abilityProvider.AbilityModel);
                    _targetSelector.Remember(unit, _abilityProvider.AbilityModel.TargetMode); // запоминаем цель
                    _abilityPanelPresenter.Disable();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            _abilityProvider.AbilityModel.DiscardCurrentTime();

            if (_abilityProvider.AbilityModel.Cost > 0)
                _source.ResetAgility();

            // UnitActionState - отнять выносливость
            // UnitActionState - сбросить кулдаун абилки
        }

        private void ShowAbilityInfos(List<AbilityModel> abilityModels)
        {
            _abilityPanelPresenter.Enable();
            _abilityPanelPresenter.FillAbilityView(abilityModels);
        }

        private void OnAnimationActionEnded()
        {
            _battleStateMachine.Enter<UpdateBattleTickState, Battlefield>(_battlefield);
        }
    }
}