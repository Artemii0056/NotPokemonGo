using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
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
            base.Enable();
            Attack(_battlefield.HeroesPlatoon.AliveUnits);
        }

        public override void Disable()
        {
            base.Disable();
            _sourceProvider.Discard(); //Todo Сбрасываться должен в стейтмашине 
        }

        private void Attack(List<Unit> targets) //Имеем несколько проблем. 1 - именно тут рандомится враг, хотя абилка может наносить атаку по разным таргетам.
                                                //2 - Изменяет стейт. Этого тут не должно происходить. Только если "включить стейт ходьбы врага"
                                                //3 - работает с абилити сервисом. Нужно эту логику разбить. Враг просто ходит - выбирает способность, которую применит и все, дальше уже логика способности
                                                //4 -  
        {
            foreach (AbilityModel abilityModel in _source.AbilityModels)
            {
                if (abilityModel.IsReady())
                {
                    Unit randomTarget = GetRandomTarget(targets);
                    
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
    }
}