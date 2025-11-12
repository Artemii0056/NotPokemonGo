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
    public class HittingGround: IAbilityHandler
    {
        private Coroutine _currentRoutine;
        private ICoroutineRunner _coroutineRunner;
        private readonly List<AbilityPart> _parts;
        private readonly UnitAnimatorController _animatorController;
        private readonly UnitAnimatorTrigger _animatorTrigger;
        private bool _animationPlaying;

        public event Action<IAbilityHandler> Finished;

        public HittingGround(ICoroutineRunner currentRoutine,
            AbilityModel abilityModel,
            Unit source)
        {
            _coroutineRunner = currentRoutine;
            _animatorController = source.UnitAnimatorController;
            _animatorTrigger = source.AnimatorTrigger;
            _parts = abilityModel.Parts;
        }

        public void Play()
        {
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

                    // 🔸 пример: пропустить фазы после неудачного QTE
                    // if (!_lastQteSuccess && phase.QteType == QteType.SliderForward)
                    // {
                    //     Debug.Log($"Фаза {phaseIndex} пропущена из-за неудачного QTE");
                    //     continue;
                    // }

                    yield return ExecutePhase(phase);
                }
            }

            FinishAbility();
        }

        private IEnumerator ExecutePhase(AbilityPhase phase)
        {
            _animatorTrigger.SetPhase(phase);
            _animatorController.Play(phase.AnimationCashName);

            // HandleCamera(phase.CameraActionType);
            //
            // if (phase.QteType != QteType.Unknown)
            //     yield return RunQtePhase(phase.QteType);

            switch (phase.PhaseType)
            {
                // case PhaseType.IsMovementPhase:
                //     yield return MoveUnit(_source, CalculateTargetPosition(_source.transform.position, _target.transform.position));
                //     break;
                //
                // case PhaseType.IsReturnPhase:
                //     yield return MoveUnit(_source, _startPosition);
                //     break;

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