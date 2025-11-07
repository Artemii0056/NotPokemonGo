using System;
using Abilities.Bennet;
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
        private HittingGround _hittingGbility;
        private BennetBaseAttack _bennetBase;

        public AbilityService(
            IAbilityProvider abilityProvider, 
            ICoroutineRunner coroutineRunner,
            IQteService qteService, 
            IBattleStateMachine battleStateMachine)
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
                    _ability.Finished +=  Continue;
                    break;
                
                case AbilityType.HittingGround:
                    _hittingGbility = new HittingGround(_coroutineRunner, _abilityProvider, source); //Оставить один кейс и до этого найти подходящую абилку
                    _hittingGbility.Play();
                    _hittingGbility.Finished +=  Continue;
                    break;
                
                case AbilityType.BaseAttack:
                    _bennetBase = new BennetBaseAttack( source, target, _abilityProvider, _coroutineRunner);
                    _bennetBase.Play();
                    _bennetBase.Finished +=  Continue;
                    break;
                
                case AbilityType.Defailt:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
            }
        }

        private void Continue()
        {
            _battleStateMachine.Enter<UpdateBattleTickState, Battlefield>(_battlefield);
            _ability.Finished -= Continue;
        }
    }
}