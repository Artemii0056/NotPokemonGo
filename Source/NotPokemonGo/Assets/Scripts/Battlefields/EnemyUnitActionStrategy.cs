using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Units;
using UnityEngine;
using VContainer;

namespace Battlefields
{
    public class EnemyUnitActionStrategy : UnitActionStrategy
    {
        private readonly Battlefield _battlefield;
        private readonly Unit _source;

        private ISourceProvider _sourceProvider;
        private ITargetSelector _targetSelector;
        private IAbilityService _abilityService;

        public EnemyUnitActionStrategy(Battlefield battlefield, Unit source)
        {
            _source = source;
            _battlefield = battlefield;
        }

        [Inject]
        public void Initialize(
            ISourceProvider sourceProvider,
            ITargetSelector targetSelector,
            IAbilityService abilityService
        )
        {
            _targetSelector = targetSelector;
            _sourceProvider = sourceProvider;
            _abilityService = abilityService;
        }

        public override void Enable()
        {
            base.Enable();
            Attack(_battlefield.HeroesPlatoon.AliveUnits);
        }

        public override void Disable()
        {
            base.Disable();
            _sourceProvider.Discard(); //Уберу дискард если чт
        }

        private void Attack(List<Unit> targets)
        {
            foreach (AbilityModel abilityModel in _source.AbilityModels)
            {
                if (abilityModel.IsReady())
                {
                    Unit randomTarget = GetRandomTarget(targets);
                    
                    //_abilityProvider.Remember(abilityModel); //Для врагов нужно сделать отдельный AbilityService 
                    _targetSelector.Remember(randomTarget);
                    _sourceProvider.Remember(_source);
                    _abilityService.SetBattlefield(_battlefield);
                    _abilityService.Handle(_source, randomTarget, abilityModel); 
                    
                    abilityModel.DiscardCurrentTime();

                    if (abilityModel.Cost > 0) 
                        _source.ResetAgility();

                    break;
                }
            }
        }

        private Unit GetRandomTarget(List<Unit> targets) =>
            targets[Random.Range(0, targets.Count)];

        // private IEnumerator Delay()
        // {
        //     yield return new WaitForSeconds(0.5f);
        //     _battleStateMachine.Enter<CheckBattleEndState, Battlefield>(_battlefield);
        // }
    }
}