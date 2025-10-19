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

        private ISourceProvider _sourceProvider;
        private IAbilityProvider _abilityProvider;
        private ITargetSelector _targetSelector;
        private AbilityPanelPresenter _abilityPanelPresenter;
        private IBattleStateMachine _battleStateMachine;
        private IInputReader _inputReader;
        private IRaycastService _raycastService;
        
        private IAbilityService _abilityService;

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
            IInputReader inputReader,
            IAbilityService abilityService
            )
        {
            _raycastService = raycastService;
            _inputReader = inputReader;
            _battleStateMachine = battleStateMachine;
            _abilityProvider = abilityProvider;
            _sourceProvider = sourceProvider;
            _targetSelector = targetSelector;
            _abilityPanelPresenter = abilityPanelPresenter;
            _abilityService = abilityService;
        }

        public override void Enable()
        {
            base.Enable();
            ShowAbilityInfos(_source.AbilityModels);
            _sourceProvider.Remember(_source);

            _source.Step.ActionEnded += OnAnimationActionEnded;
            _inputReader.LeftMouseButtonPressed += OnLeftMouseButtonPressed;
        }

        private void OnLeftMouseButtonPressed()
        {
            if (_raycastService.Raycast(out Unit unit)) 
                OnUnitSearched(unit);
        }

        public override void Disable()
        {
            base.Disable();

            _source.Step.ActionEnded -= OnAnimationActionEnded;
            _inputReader.LeftMouseButtonPressed -= OnLeftMouseButtonPressed;
           // _sourceProvider.Discard();
            
            Debug.Log("Disable Friend Unit Action");
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

                case PlatoonType.Enemies: 
                    //АбилитиСервис (управлятор всеми абилками) ->
                    //Имеет дикшарь/свичКейс со всеми возможными абилками ->
                    //выбирает по типу абилку(передает продюсера и таргета) ->
                    //Говорит абилке(MultyAttack) плей(внутри что-то похожее на HandlePhase из UnitStep) ->
                    //Multyattack полностью следит за завершением абилки и событие о завершении
                    _abilityService.Handle(_source, unit, _battlefield);
                    
                    //_source.Step.SetAbilityModel(_abilityProvider.AbilityModel, _source, unit);
                    _targetSelector.Remember(unit); 
                    _abilityPanelPresenter.Disable();
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
           // _battleStateMachine.Enter<CheckBattleEndState, Battlefield>(_battlefield);
        }
    }
}