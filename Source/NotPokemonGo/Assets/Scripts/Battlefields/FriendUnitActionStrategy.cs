using System;
using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Platoons;
using Services.InputServices;
using Services.RaycastServices;
using UI.Ability;
using Units;
using UnityEngine;
using VContainer;

namespace Battlefields
{
    public class FriendUnitActionStrategy : UnitActionStrategy //TODO Этот класс нужно регать
    {
        private readonly Battlefield _battlefield;
        private readonly Unit _source;
        private Unit _currentTarget;

        private AbilitiesPanel _abilitiesPanels;

        private AbilityPanelPresenter _abilityPanelPresenter;
        private IInputReader _inputReader;
        private IRaycastService _raycastService;

        private IAbilityService _abilityService;

        private ITargetSelector _targetSelector;
        
        private readonly TargetHighlighter _targetHighlighter;

        public FriendUnitActionStrategy(Battlefield battlefield, Unit source, TargetHighlighter targetHighlighter)
        {
            _source = source;
            _targetHighlighter = targetHighlighter;
            _battlefield = battlefield;
        }

        [Inject]
        public void Initialize(
            IRaycastService raycastService,
            ITargetSelector targetSelector,
            AbilityPanelPresenter abilityPanelPresenter,
            IInputReader inputReader,
            IAbilityService abilityService,
            AbilitiesPanel abilitiesPanels
        )
        {
            _raycastService = raycastService;
            _inputReader = inputReader;
            _abilityPanelPresenter = abilityPanelPresenter;
            _abilityService = abilityService;
            _abilitiesPanels = abilitiesPanels;
            _targetSelector = targetSelector;

            _abilitiesPanels.AbilityModelSelected += OnAbilitySelected;
        }

        public override void Enable()
        {
            base.Enable();
            
            if (_targetHighlighter.Target != null)
                _currentTarget = _targetHighlighter.Target;
            else
                _currentTarget = _targetSelector.GetRandomEnemyTarget();

            RememberTarget(_currentTarget);
            
            ShowAbilityInfos(_source.AbilityModels);

            _inputReader.LeftMouseButtonPressed += OnLeftMouseButtonPressed;
        }

        private void OnLeftMouseButtonPressed()
        {
            if (_raycastService.Raycast(out Unit unit))
                RememberTarget(unit);
        }

        public override void Disable()
        {
            base.Disable();

            _inputReader.LeftMouseButtonPressed -= OnLeftMouseButtonPressed;
            _abilitiesPanels.AbilityModelSelected -= OnAbilitySelected;
        }

        private void OnAbilitySelected(AbilityModel abilityModel) 
        {
            if (abilityModel.IsReady() == false)
                return;

            _abilityService.SetBattlefield(_battlefield);
            _abilityService.RunAbilityAsync(_source, _currentTarget, abilityModel); 

            _abilityPanelPresenter.Disable();

            abilityModel.DiscardCurrentTime(); //TODO Выглядит странным тут

            if (abilityModel.Cost > 0)//
                _source.ResetAgility();//

            //TODO И вот тут должна быть проверка на хилку или атаку?
        }

        private void RememberTarget(Unit target)
        {
            _currentTarget = target;

            switch (target.PlatoonType)
            {
                case PlatoonType.Heroes:
                    Debug.Log("Выбрали союзника");
                    break;

                case PlatoonType.Enemies:
                    Debug.Log("Выбрали врага");
                    _targetHighlighter.Highlight(target);
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