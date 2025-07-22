using System.Collections;
using System.Collections.Generic;
using Abilities.MV;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Services;
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
        private ITargetSelector _targetSelector;
        private ICoroutineRunner _coroutineRunner;

        public EnemyUnitActionStrategy(Battlefield battlefield, Unit source)
        {
            _source = source;
            _battlefield = battlefield;
        }

        [Inject]
        public void Initialize(
            IBattleStateMachine battleStateMachine,
            ISourceProvider sourceProvider,
            IAbilityProvider abilityProvider,
            ITargetSelector targetSelector,
            ICoroutineRunner coroutineRunner
        )
        {
            _coroutineRunner = coroutineRunner;
            _targetSelector = targetSelector;
            _abilityProvider = abilityProvider;
            _sourceProvider = sourceProvider;
            _battleStateMachine = battleStateMachine;
        }

        public override void Enable()
        {
            base.Enable();
            Attack(_battlefield.Heroes.Units);
            _source.Step.ActionEnded += OnAnimationActionEnded;
        }

        public override void Disable()
        {
            base.Disable();
            _sourceProvider.Discard();
            _source.Step.ActionEnded -= OnAnimationActionEnded;
        }

        private void Attack(List<Unit> targets)
        {
            foreach (AbilityModel abilityModel in _source.AbilityModels)
            {
                if (abilityModel.IsReady())
                {
                    _source.Step.SetAbilityModel(abilityModel, _source, GetRandomTarget(targets));
                    
                    _sourceProvider.Remember(_source);
                    _abilityProvider.Remember(abilityModel);
                    _targetSelector.Remember(GetRandomTarget(targets), _abilityProvider.AbilityModel.TargetMode);
                    
                    abilityModel.DiscardCurrentTime();

                    if (abilityModel.Cost > 0) 
                        _source.ResetAgility();

                    break;
                }
            }
        }

        private Unit GetRandomTarget(List<Unit> targets) =>
            targets[Random.Range(0, targets.Count)];

        private void OnAnimationActionEnded()
        {
            _coroutineRunner.StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(0.5f);
            _battleStateMachine.Enter<UpdateBattleTickState, Battlefield>(_battlefield);
        }
    }
}