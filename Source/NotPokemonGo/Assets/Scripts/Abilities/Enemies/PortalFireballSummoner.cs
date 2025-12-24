using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Bennet;
using Abilities.Configs;
using Abilities.MV;
using Armaments;
using QTESystem;
using Services;
using UI.QTE;
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
        private readonly IArmamentApplicator _armamentApplicator;

        private readonly List<AbilityPart> _parts;

        private UnitAnimatorTrigger _animatorTrigger;
        private UnitAnimatorController _animatorController;

        private bool _animationPlaying;

        private Unit _target;

        private Coroutine _currentRoutine;
        private Coroutine _attackRoutine;

        private AbilityPhase _currentPhase;
        private Coroutine _abilityCoroutine;

        public PortalFireballSummoner(
            ICoroutineRunner coroutineRunner,
            AbilityModel abilityModel,
            IQteService qteService, 
            IArmamentApplicator armamentApplicator)
        {
            _coroutineRunner = coroutineRunner;
            _qteService = qteService;
            _armamentApplicator = armamentApplicator;
            Interruptibility = abilityModel.Interruptibility;
            _parts = abilityModel.Parts;
        }

        public Interruptibility Interruptibility { get; }

        public event Action<IAbilityHandler> Finished;

        public void Play(Unit source, Unit target)
        {
            _target = target;

            _animatorTrigger = source.AnimatorTrigger;
            _animatorController = source.UnitAnimatorController;
            _animatorController.Attack1Started += OnAttackStarted;

            _currentRoutine = _coroutineRunner.StartCoroutine(ExecuteAllParts());
        }

        public void Stop() =>
            _coroutineRunner.StopCoroutine(_currentRoutine);

        private IEnumerator ExecuteAllParts()
        {
            foreach (var part in _parts)
            {
                foreach (var phase in part.AbilityPhases)
                    yield return ExecutePhase(phase);
            }

            FinishAbility();
        }

        private IEnumerator ExecutePhase(AbilityPhase phase)
        {
            _currentPhase = phase;

            _animatorController.Play(phase.AnimationCashName);
            _animatorTrigger.SetPhase(phase);
            _animatorTrigger.SetTarget(_target);

            switch (phase.PhaseType)
            {
                default:
                    yield return WaitForAnimation();
                    break;
            }
        }

        private void FinishAbility()
        {
            _animatorTrigger.ClearParticles();
            _animatorController.Play(Constants.BaseAnimations.Idle);
            Finished?.Invoke(this);
        }

        private IEnumerator WaitForAnimation()
        {
            _animationPlaying = true;

            void OnFinished() =>
                FinishAnimation();

            _animatorController.Finished += OnFinished;
            yield return new WaitWhile(() => _animationPlaying);
            _animatorController.Finished -= OnFinished;
        }

        private void OnAttackStarted()
        {
            
            // Debug.Log("OnAttackStarted");
            //
            // if (_coroutine != null)
            //     _coroutineRunner.StopCoroutine(_currentRoutine);
            //
            // _coroutine = _coroutineRunner.StartCoroutine(ExecuteAttack()); //Трабла в этой корутине
        }

        private IEnumerator ExecuteAttack() //попробовать проще и без корутины? Получить от мувера событие, когда начался полет? 
        {
            yield return new WaitForSeconds(0.25f); //TODO вот тут попробовать получить данные о задержке у мувера.
            (QteButtonView, QtePhasePresenter) valueTuple = _qteService.PlaySimple(_currentPhase.QteType, _target);
            //Дождаться, пока фаербол долетит? 

           // yield return WaitForAnimation(); //задержка нужна для анимации

           yield return new WaitUntil(() => _animationPlaying);
           
            if (_currentPhase.QteType != QteType.Unknown)
            {
                Object.Destroy(valueTuple.Item1.gameObject);
                valueTuple.Item2.Disable();
            }
        }

        private void FinishAnimation() =>
            _animationPlaying = false;
    }
}