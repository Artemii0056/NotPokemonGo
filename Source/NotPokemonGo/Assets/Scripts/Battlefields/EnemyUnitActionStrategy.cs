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
        private IAbilityService _abilityService;

        public EnemyUnitActionStrategy(Battlefield battlefield, Unit source)
        {
            _source = source;
            _battlefield = battlefield;
        }

        [Inject]
        public void Initialize(
            ISourceProvider sourceProvider,
            IAbilityService abilityService
        )
        {
            _sourceProvider = sourceProvider;
            _abilityService = abilityService;
        }

        public override void Enable()
        {
            Debug.Log($"EnemyUnitActionStrategy.Enable source={_source.name}");
            
            base.Enable();
            Attack(_battlefield.HeroesPlatoon.AliveUnits);
        }

        public override void Disable()
        {
            base.Disable();
            _sourceProvider.Discard(); //Todo Сбрасываться должен в стейтмашине 
        }

        private void Attack(List<Unit> targets) 
        {
            foreach (AbilityModel abilityModel in _source.AbilitySO)
            {
                Debug.Log("Attack in Enemy");
                
                // if (abilityModel.IsReady())
                // {
                    Unit randomTarget = GetRandomTarget(targets);
                    
                    _abilityService.SetBattlefield(_battlefield);
                   // _abilityService.Handle(_source, randomTarget, abilityModel); 
                    //_abilityService.RunAbilityAsync(_source, randomTarget, abilityModel); 
                    
                    Debug.Log($"Enemy Attack START source={_source.name}");
                    Debug.Log($"Enemy target={randomTarget.name}");
                    Debug.Log("Enemy before RunAbilityAsync");
                    _abilityService.RunAbilityAsync(_source, randomTarget, abilityModel);
                    Debug.Log("EnemyUnitActionStrategy: after RunAbilityAsync");
                    
                    abilityModel.DiscardCurrentTime();

                    if (abilityModel.Cost > 0) 
                        _source.ResetAgility();

                    break;
                //}
            }
        }

        private Unit GetRandomTarget(List<Unit> targets) =>
            targets[Random.Range(0, targets.Count)];
    }
}