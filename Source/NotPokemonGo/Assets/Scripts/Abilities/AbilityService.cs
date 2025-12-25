using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Bennet;
using Abilities.Enemies;
using Abilities.MV;
using Armaments;
using Armaments.Spawner;
using Battlefields;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using QTESystem;
using Services;
using UnityEngine;
using Unit = Units.Unit;

namespace Abilities
{
    public class AbilityService : IAbilityService
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IBattleStateMachine _battleStateMachine;
        private readonly IQteService _qteService;
        private readonly IEffectsApplier _effectsApplier;
        private readonly IArmamentViewFactory _viewFactory;
        private readonly IArmamentLifecycle _armamentLifecycle;
        private readonly IArmamentSpawner _armamentSpawner;

        private Battlefield _battlefield;

        private List<IAbilityHandler> _activeAbilityHandlers;

        private Counterattack _counterattack;

        private IAbilityHandler _abilityHandler;

        public event Action Finished;

        public AbilityService(
            ICoroutineRunner coroutineRunner,
            IQteService qteService,
            IBattleStateMachine battleStateMachine,
            IEffectsApplier effectsApplier, 
            IArmamentViewFactory viewFactory, IArmamentLifecycle armamentLifecycle, IArmamentSpawner armamentSpawner)
        {
            _coroutineRunner = coroutineRunner;
            _qteService = qteService;
            _battleStateMachine = battleStateMachine;
            _effectsApplier = effectsApplier;
            _viewFactory = viewFactory;
            _armamentLifecycle = armamentLifecycle;
            _armamentSpawner = armamentSpawner;
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
                    var a = new PortalFireballSummoner(_coroutineRunner, abilityModel, _qteService, _armamentSpawner);
                    a.Play(source, target);
                    //_activeAbilityHandlers.Add(_abilityHandler);
                    a.Finished += Continue;
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

                case AbilityType.CounterAttack:
                    break;

                case AbilityType.DroneBaseAttack:
                    _abilityHandler = new DroneBaseAttack(abilityModel, _coroutineRunner);
                    _abilityHandler.Play(source, target);
                    _activeAbilityHandlers.Add(_abilityHandler);
                    _abilityHandler.Finished += Continue;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
            }

            source.RememberAbility(_abilityHandler);
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
            _coroutineRunner.StartCoroutine(Continue2(handler));
        }

        private IEnumerator Continue2(IAbilityHandler handler)
        {
            _activeAbilityHandlers.Remove(handler);
            handler.Finished -= Continue;

            yield return new WaitForSeconds(0.5f);

            _battleStateMachine.Enter<CheckBattleEndState, Battlefield>(_battlefield);
        }
    }
}