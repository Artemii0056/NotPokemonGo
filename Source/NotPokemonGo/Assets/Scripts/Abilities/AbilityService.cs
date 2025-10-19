using System;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Services;
using Services.QTEServices;
using Units;
using UnityEngine;

namespace Abilities
{
    public class AbilityService : IAbilityService
    {
        private ICoroutineRunner _coroutineRunner;
        private IAbilityProvider _abilityProvider;
        private IBattleStateMachine _battleStateMachine;
        private IQteService _qteService;

        private Battlefield _battlefield;

        public event Action Finished;

        private EngineeringSeriesAbility _ability;

        public AbilityService(IAbilityProvider abilityProvider, ICoroutineRunner coroutineRunner,
            IQteService qteService, IBattleStateMachine battleStateMachine)
        {
            _abilityProvider = abilityProvider;
            _coroutineRunner = coroutineRunner;
            _qteService = qteService;
            _battleStateMachine = battleStateMachine;
        }

        public void Handle(Unit source, Unit target, Battlefield battlefield)
        {
            _battlefield = battlefield;
            
            var abilityType = _abilityProvider.AbilityModel.AbilityType;

            switch (abilityType)
            {
                case AbilityType.FireBall:
                    break;
                case AbilityType.FrostBall:
                    break;
                case AbilityType.PoisonBall:
                    break;
                case AbilityType.AlcoholBall:
                    break;
                case AbilityType.CastSpell:
                    break;
                case AbilityType.DoubleAttack:
                    break;
                case AbilityType.BaseAbility:
                    break;
                case AbilityType.EngineeringSeries:
                    _ability = new EngineeringSeriesAbility(source, target, _abilityProvider, _coroutineRunner, _qteService);
                    _ability.Play();
                    _ability.Finished +=  Continie;
                    break;
                case AbilityType.Defailt:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
            }
        }

        private void Continie()
        {
            Debug.Log(_battleStateMachine == null);
            
            _battleStateMachine.Enter<UpdateBattleTickState, Battlefield>(_battlefield);
            _ability.Finished -= Continie;
        }
    }
}