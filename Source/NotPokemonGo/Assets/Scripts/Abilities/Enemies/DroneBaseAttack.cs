using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Bennet;
using Abilities.Configs;
using Abilities.MV;
using Services;
using Units;
using Units.AnimationControllers;
using UnityEngine;

namespace Abilities.Enemies
{
    public class DroneBaseAttack : IAbilityHandler
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly List<AbilityPart> _parts;
        
        private UnitAnimatorTrigger _animatorTrigger;
        private UnitAnimatorController _animatorController;
        private Coroutine _currentRoutine;
        private bool _animationPlaying;
        
        private Unit _target;
        private Unit _source;

        public DroneBaseAttack(AbilityModel abilityModel, ICoroutineRunner coroutineRunner)
        {
            _parts = abilityModel.Parts;
            _coroutineRunner = coroutineRunner;
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

        private void FinishAnimation() =>
            _animationPlaying = false;
    }
}