using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Bennet;
using Abilities.Configs;
using Abilities.MV;
using Abilities.Signals;
using Armaments;
using Armaments.Spawner;
using QTESystem;
using QTESystem.TestQTE;
using Services;
using Stats;
using Units;
using Units.AnimationControllers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Abilities.Enemies
{
    public class PortalFireballSummoner : IAbilityHandler
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IQteService _qteService;
        private readonly IArmamentSpawner _armamentSpawner;

        private readonly List<AbilityPart> _parts;

        private UnitAnimatorTrigger _animatorTrigger;
        private AnimatorController _animatorController;

        private bool _waitingFinish;

        private Unit _target;
        private Unit _source;

        private Coroutine _currentRoutine;

        private AbilityPhase _currentPhase;
        private Coroutine _abilityCoroutine;
        private TimingBarQte _timingBarQte;

        public PortalFireballSummoner(
            ICoroutineRunner coroutineRunner,
            AbilityModel abilityModel,
            IQteService qteService,
            IArmamentSpawner armamentSpawner)
        {
            _coroutineRunner = coroutineRunner;
            _qteService = qteService;
            _armamentSpawner = armamentSpawner;
            Interruptibility = abilityModel.Interruptibility;
            _parts = abilityModel.Parts;
        }

        public Interruptibility Interruptibility { get; }

        public event Action<IAbilityHandler> Finished;

        public void Play(Unit source, Unit target)
        {
            _source = source;
            _target = target;

           // _source.AnimatorTrigger.AbilityPhaseService.ArmamentRequested += OnArmamentRequested;

            _animatorTrigger = source.AnimatorTrigger;
            _animatorController = source.AnimatorController;

            _animatorController.Signal += OnAnimSignal;

            _currentRoutine = _coroutineRunner.StartCoroutine(ExecuteAllParts());
        }

        public void Stop()
        {
            if (_currentRoutine != null)
                _coroutineRunner.StopCoroutine(_currentRoutine);

            Cleanup();
        }

        private IEnumerator ExecuteAllParts()
        {
            foreach (var part in _parts)
            {
                foreach (var phase in part.AbilityPhases)
                    yield return ExecutePhase(phase);
            }

            FinishAbility();
        }

        private void OnArmamentRequested(AbilityPhase phase) 
        {
            // ArmamentContext context = new ArmamentContext(_source, _target, phase.ArmamentSetup, phase.ArmamentSetup.FlyingType);
            //
            // IArmamentMover mover = _armamentSpawner.Create(context);
            //
            // mover.Move();
            // mover.Reached += OnReached;
            //
            // _timingBarQte = _qteService.PlayTimingBar(_currentPhase.QteType, _target, mover.Duration); 
            // _timingBarQte.OnReached += OnQteFinished;
        }

        private void OnQteFinished(QteResult result)
        {
            switch (result)
            {
                case QteResult.Fail:
                    Debug.Log("OnFail");
                    break;

                case QteResult.Normal:
                    Debug.Log("OnNormal");
                    break;
                
                case QteResult.Perfect:
                    Debug.Log("Perfect");
                    //_target.ChangeStatValue(1, StatType.DodgeFlag); //подумать над реализацией "временных" бафов
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
        }

        private void OnReached(IArmamentMover mover) 
        {
            Debug.Log(" OnReached");
            
            mover.Reached -= OnReached;
            _timingBarQte.OnReached -= OnQteFinished;

            Object.Destroy(_timingBarQte.gameObject);
        }

        private IEnumerator ExecutePhase(AbilityPhase phase)
        {
            _currentPhase = phase;      

            _animatorController.Play(phase.AnimationCashName);
            _animatorTrigger.SetPhase(phase);
            _animatorTrigger.SetTarget(_target);

            yield return WaitForPhaseFinish();
        }

        private void FinishAbility()
        {
            //_animatorTrigger.ClearParticles();
            _animatorController.Play(Constants.BaseAnimations.Idle);
           // _source.AnimatorTrigger.AbilityPhaseService.ArmamentRequested -= OnArmamentRequested;
            Cleanup();
            Finished?.Invoke(this);
        }

        private IEnumerator WaitForPhaseFinish()
        {
            _waitingFinish = true;
            yield return new WaitWhile(() => _waitingFinish);
        }

        private void OnAnimSignal(int id)
        {
            if (PhaseSignalUtil.FromInt(id) == PhaseSignal.Finish)
                _waitingFinish = false;
        }

        private void Cleanup()
        {
            if (_animatorController != null)
                _animatorController.Signal -= OnAnimSignal;

            _waitingFinish = false;
            _currentRoutine = null;
        }
    }
}