using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.MV;
using Infrastructure;
using Services;
using Units;
using Units.AnimationControllers;
using UnityEngine;

namespace Abilities.Bennet
{
    public class StrikeFromAbove : IAbilityHandler
    {
        private readonly List<AbilityPart> _parts;
        private readonly ICoroutineRunner _coroutineRunner;

        private Coroutine _currentRoutine;
        private UnitAnimatorController _animatorController;
        private UnitAnimatorTrigger _animatorTrigger;

        private bool _animationPlaying;

        public event Action<IAbilityHandler> Finished;

        public StrikeFromAbove(
            ICoroutineRunner currentRoutine,
            AbilityModel abilityModel)
        {
            _coroutineRunner = currentRoutine;

            _parts = abilityModel.Parts;
        }

        public void Play(Unit source, Unit target)
        {
            _animatorController = source.UnitAnimatorController;
            _animatorTrigger = source.AnimatorTrigger;

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
            _animatorTrigger.SetPhase(phase);
            _animatorController.Play(phase.AnimationCashName);

            switch (phase.PhaseType)
            {
                default:
                    yield return WaitForAnimation();
                    break;
            }
        }

        private IEnumerator WaitForAnimation()
        {
            _animationPlaying = true;

            void OnFinished() => FinishAnimation();

            _animatorController.Finished += OnFinished;
            yield return new WaitWhile(() => _animationPlaying);
            _animatorController.Finished -= OnFinished;
        }

        private void FinishAnimation() =>
            _animationPlaying = false;

        private void FinishAbility()
        {
            _animatorController.Play(Constants.BaseAnimations.Idle);
            Finished?.Invoke(this);
        }
    }
}