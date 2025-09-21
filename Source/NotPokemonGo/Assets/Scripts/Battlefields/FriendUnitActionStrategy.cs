using System;
using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Services.InputServices;
using Services.RaycastServices;
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

        private IRaycastService _raycastService;
        private ISourceProvider _sourceProvider;
        private IAbilityProvider _abilityProvider;
        private ITargetSelector _targetSelector;
        private AbilityPanelPresenter _abilityPanelPresenter;
        private IBattleStateMachine _battleStateMachine;
        private IInputReader _inputReader;

        public FriendUnitActionStrategy(Battlefield battlefield, Unit source)
        {
            _source = source;
            _battlefield = battlefield;
        }

        [Inject]
        public void Initialize(
            IRaycastService raycastService,
            ISourceProvider sourceProvider,
            IAbilityProvider abilityProvider,
            ITargetSelector targetSelector,
            IBattleStateMachine battleStateMachine,
            AbilityPanelPresenter abilityPanelPresenter,
            IInputReader inputReader
            )
        {
            _inputReader = inputReader;
            _battleStateMachine = battleStateMachine;
            _abilityProvider = abilityProvider;
            _raycastService = raycastService;
            _sourceProvider = sourceProvider;
            _targetSelector = targetSelector;
            _abilityPanelPresenter = abilityPanelPresenter;
        }

        public override void Enable()
        {
            base.Enable();
            ShowAbilityInfos(_source.AbilityModels);
            _sourceProvider.Remember(_source);

            _inputReader.LeftMouseButtonPressed += LeftMouseButtonClicked;
            _source.Step.ActionEnded += OnAnimationActionEnded;
        }

        public override void Disable()
        {
            base.Disable();

            _inputReader.LeftMouseButtonPressed -= LeftMouseButtonClicked;
            _source.Step.ActionEnded -= OnAnimationActionEnded;
            _sourceProvider.Discard();
        }

        private void LeftMouseButtonClicked()
        {
            if (_raycastService.Raycast(out Unit unit) == false)
                return;

            if (_abilityProvider.AbilityModel == null)
                return;

            if (_source == unit)
                return;

            switch (unit.PlatoonType)
            {
                case PlatoonType.Friends:
                    Debug.Log("Выбрали союзника");
                    break;

                case PlatoonType.Enemies: 
                    _source.Step.SetAbilityModel(_abilityProvider.AbilityModel, _source, unit);
                    _targetSelector.Remember(unit); 
                    _abilityPanelPresenter.Disable();
                    
                    _battleStateMachine.Enter<QTEBattleState, AbilityType>(_abilityProvider.AbilityModel.AbilityType);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            _abilityProvider.AbilityModel.DiscardCurrentTime();

            if (_abilityProvider.AbilityModel.Cost > 0)
                _source.ResetAgility();
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