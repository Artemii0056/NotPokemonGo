using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Bennet;
using Abilities.Configs;
using Abilities.MV;
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

        private UnitAnimatorTrigger _animatorTrigger;
        private UnitAnimatorController _animatorController;

        private bool _animationPlaying;

        private readonly List<AbilityPart> _parts;
        private Unit _source;
        private Unit _target;

        private Coroutine _currentRoutine;
        private Coroutine _attackRoutine;
        private IQteService _qteService;

        public PortalFireballSummoner(
            ICoroutineRunner coroutineRunner,
            AbilityModel abilityModel, 
            IQteService qteService)
        {
            _coroutineRunner = coroutineRunner;
            _qteService = qteService;
            Interruptibility = abilityModel.Interruptibility;
            _parts = abilityModel.Parts;
        }

        public Interruptibility Interruptibility { get; }

        public event Action<IAbilityHandler> Finished;

        public void Play(Unit source, Unit target)
        {
            _target = target;
            _source = source;

            _animatorTrigger = source.AnimatorTrigger;
            _animatorController = source.UnitAnimatorController;

            _currentRoutine = _coroutineRunner.StartCoroutine(ExecuteAllParts());
        }

        public void Stop()
        {
            _coroutineRunner.StopCoroutine(_currentRoutine);
        }

        private IEnumerator ExecuteAllParts()
        {
            for (int partIndex = 0; partIndex < _parts.Count; partIndex++)
            {
                var part = _parts[partIndex];

                for (int phaseIndex = 0; phaseIndex < part.AbilityPhases.Count; phaseIndex++)
                {
                    var phase = part.AbilityPhases[phaseIndex];

                    yield return ExecutePhase(phase);
                }
            }

            FinishAbility();
        }

        private IEnumerator ExecutePhase(AbilityPhase phase)
        {
            (QteButtonView, QtePhasePresenter) valueTuple = (null, null);
            
            _animatorController.Play(phase.AnimationCashName);
            _animatorTrigger.SetPhase(phase);
            _animatorTrigger.SetTarget(_target);

            if (phase.QteType != QteType.Unknown)
                valueTuple = _qteService.StartSimple(phase.QteType, _source);

            switch (phase.PhaseType)
            {
                default:
                    yield return WaitForAnimation();
                    break;
            }

            yield return new WaitForSeconds(3f);
            
            if (phase.QteType != QteType.Unknown)
            {
                Object.Destroy(valueTuple.Item1.gameObject);
                valueTuple.Item2.Disable();
            }
            
            //yield return new WaitForSeconds(1.5f);
        }
        
        //QteConfig qteConfig = _staticDataService.GetQteConfig(qteType); //TODO тут не забыть подрубить
        
        // private (QteButtonView, QtePhasePresenter) RunQtePhase(QteType qteType)
        // {
        //    (QteButtonView, QtePhasePresenter) value = _qteService.StartSimple(qteType, _source);
        //
        //    return value;
        // }

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

        private void FinishAnimation() =>
            _animationPlaying = false;
    }
}