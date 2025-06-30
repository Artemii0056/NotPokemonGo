using System.Collections;
using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Animations;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Services;
using UI.Ability;
using Units;
using UnityEngine;
using VContainer;

namespace Battlefields
{
    public class EnemyUnitActionStrategy : UnitActionStrategy
    {
        private readonly Battlefield _battlefield;
        private readonly Unit _source;

        private IBattleStateMachine _battleStateMachine;
        private ISourceProvider _sourceProvider;
        private IAbilityProvider _abilityProvider;
        private IAbilityApplicatorService _abilityApplicatorService;
        private ITargetSelector _targetSelector;
        private readonly IAnimationProcessingService _animationProcessingService;

        public EnemyUnitActionStrategy(Battlefield battlefield, Unit source,IAnimationProcessingService animationProcessingService )
        {
            _animationProcessingService = animationProcessingService;
            _source = source;
            _battlefield = battlefield;
        }

        [Inject]
        public void Initialize(
            IBattleStateMachine battleStateMachine,
            ISourceProvider sourceProvider,
            IAbilityProvider abilityProvider,
            IAbilityApplicatorService abilityApplicatorService,
            ITargetSelector targetSelector
            )
        {
            _targetSelector = targetSelector;
            _abilityApplicatorService = abilityApplicatorService;
            _abilityProvider = abilityProvider;
            _sourceProvider = sourceProvider;
            _battleStateMachine = battleStateMachine;
        }

        public override void Enable()
        {
            base.Enable();
            Attack(_battlefield.Platoon2.Heroes);
            _source.AnimationActionEnded += OnAnimationActionEnded;
            
        }

        public override void Disable()
        {
            base.Disable();
            _source.AnimationActionEnded -= OnAnimationActionEnded;
            _sourceProvider.Discard();
        }

        private void Attack(List<Unit> targets)
        {
            foreach (AbilityModel abilityModel in _source.AbilityModels)
            {
                if (abilityModel.IsReady)
                {
                    _sourceProvider.Remember(_source);
                    _abilityProvider.Remember(abilityModel);
                    _targetSelector.Remember(GetRandomTarget(targets), _abilityProvider.AbilityModel.TargetMode); 
                    
                    Debug.Log("EnemyUnitActionStrategy");
                    
                    _animationProcessingService.PlayAnimation(_sourceProvider.Source, _abilityProvider.AbilityModel.AbilityType);
                    break;
                }
            }

            _source.Step.ResetCurrentValue();
        }

        private Unit GetRandomTarget(List<Unit> targets) => 
            targets[Random.Range(0, targets.Count)];

        private void OnAnimationActionEnded()
        {
            _battleStateMachine.Enter<UpdateBattleTickState, Battlefield>(_battlefield);
        }
    }
}