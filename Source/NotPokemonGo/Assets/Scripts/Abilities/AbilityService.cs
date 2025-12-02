using System;
using System.Collections.Generic;
using Abilities.Bennet;
using Abilities.Enemies;
using Abilities.MV;
using Battlefields;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using QTESystem;
using Services;
using Units;
using UnityEngine;

namespace Abilities
{
    public class AbilityService : IAbilityService
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IBattleStateMachine _battleStateMachine;
        private readonly ISourceProvider _sourceProvider;
        private readonly IQteService _qteService;
        private readonly ITargetSelector _targetSelector;

        private Battlefield _battlefield;

        private List<IAbilityHandler> _activeAbilityHandlers;

        private Counterattack _counterattack;

        private IAbilityHandler _abilityHandler;
        
        public event Action Finished;

        public AbilityService(
            ICoroutineRunner coroutineRunner,
            IQteService qteService,
            IBattleStateMachine battleStateMachine,
            ITargetSelector targetSelector,
            ISourceProvider sourceProvider)
        {
            _coroutineRunner = coroutineRunner;
            _qteService = qteService;
            _battleStateMachine = battleStateMachine;
            _targetSelector = targetSelector;
            _sourceProvider = sourceProvider;
            _activeAbilityHandlers = new List<IAbilityHandler>();
        }

        public void SetBattlefield(Battlefield battlefield)
        {
            _battlefield = battlefield;
        }

        public void Handle(Unit source, Unit target, AbilityModel abilityModel) 
        {
            AbilityType abilityType = abilityModel.AbilityType;

            switch (abilityType)
            {
                case AbilityType.FireBall:
                    break;

                case AbilityType.FrostBall:
                    _abilityHandler = new BaseEnemyAttack(_coroutineRunner, abilityModel);
                    _abilityHandler.Play(source, target);
                    _activeAbilityHandlers.Add(_abilityHandler);
                    _abilityHandler.Finished += Continue;
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
                    _abilityHandler =
                        new EngineeringSeriesAbility(abilityModel, _coroutineRunner, _qteService);
                    _abilityHandler.Play(source, target);
                    _activeAbilityHandlers.Add(_abilityHandler);
                    _abilityHandler.Finished += Continue;
                    break;

                case AbilityType.StrikeFromAbove:
                    _abilityHandler = new StrikeFromAbove(_coroutineRunner, abilityModel);
                    _abilityHandler.Play(source, target);
                    _activeAbilityHandlers.Add(_abilityHandler);
                    _abilityHandler.Finished += Continue;
                    break;

                case AbilityType.BaseAttack:
                    _abilityHandler = new BennetBaseAttack(abilityModel, _coroutineRunner);
                    _abilityHandler.Play(source, target);
                    _activeAbilityHandlers.Add(_abilityHandler);
                    _abilityHandler.Finished += Continue;
                    break;

                case AbilityType.Default:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
            }
        }

        public void HandleCounterAttack(Unit source, Unit target, AbilityModel abilityModel)
        {
            _abilityHandler.Stop();
            _abilityHandler.Finished -= Continue;
            
            _abilityHandler = new Counterattack(_coroutineRunner, abilityModel);
            _activeAbilityHandlers.Add(_abilityHandler);

            _abilityHandler.Play(source, target); 
            _abilityHandler.Finished += Continue;
        }

        private void Continue(IAbilityHandler handler)
        {
            _activeAbilityHandlers.Remove(handler);
            handler.Finished -= Continue;

            // if (_activeAbilityHandlers.Count > 0)
            // {
            //     foreach (var abilityHandler in _activeAbilityHandlers) //Не, бред какой то 
            //         abilityHandler.Finished -= Continue;
            // }

            _battleStateMachine.Enter<CheckBattleEndState, Battlefield>(_battlefield);
        }
    }
}