using System;
using System.Collections;
using System.Collections.Generic;
using Cameras;
using Cinemachine;
using DG.Tweening;
using Infrastructure;
using QTESystem;
using Services;
using Services.QTEServices;
using Units;
using Units.AnimationControllers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Abilities.Bennet
{
    public class EngineeringSeriesAbility
    {
        private readonly Unit _source;
        private readonly Unit _target;
        private readonly UnitAnimatorController _animatorController;
        private readonly UnitAnimatorTrigger _animatorTrigger;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IQteService _qteService;
        private readonly List<AbilityPart> _parts;
        private readonly Vector3 _startPosition;
        private readonly CinemachineBrain _cinemachineBrain;

        private Coroutine _currentRoutine;
        private bool _lastQteSuccess;
        
        private bool _animationPlaying;

        public event Action Finished;

        public EngineeringSeriesAbility(
            Unit source,
            Unit target,
            IAbilityProvider abilityProvider,
            ICoroutineRunner coroutineRunner,
            IQteService qteService)
        {
            _source = source;
            _target = target;
            _coroutineRunner = coroutineRunner;
            _qteService = qteService;

            _animatorController = source.UnitAnimatorController;
            _animatorTrigger = source.AnimatorTrigger;
            _parts = abilityProvider.AbilityModel.Parts;
            _startPosition = source.transform.position;
            _cinemachineBrain = Object.FindObjectOfType<CinemachineBrain>(); //TODO Вот эту херню исправить 
            //Исправить и добавить тайм сервис и с ним связанную логику.
        }

        public void Play()
        {
            _currentRoutine = _coroutineRunner.StartCoroutine(ExecuteAllParts());
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
                    if (!_lastQteSuccess && phase.QteType == QteType.SliderForward)
                    {
                        Debug.Log($"Фаза {phaseIndex} пропущена из-за неудачного QTE");
                        continue;
                    }

                    yield return ExecutePhase(phase);
                }
            }

            FinishAbility();
        }

        private IEnumerator ExecutePhase(AbilityPhase phase)
        {
            _animatorTrigger.SetPhase(phase);
            _animatorController.Play(phase.AnimationCashName);

            HandleCamera(phase.CameraActionType);

            if (phase.QteType != QteType.Unknown)
                yield return RunQtePhase(phase.QteType);

            switch (phase.PhaseType)
            {
                case PhaseType.IsMovementPhase:
                    yield return MoveUnit(_source, CalculateTargetPosition(_source.transform.position, _target.transform.position));
                    break;

                case PhaseType.IsReturnPhase:
                    yield return MoveUnit(_source, _startPosition);
                    break;

                default:
                    yield return WaitForAnimation();
                    break;
            }
        }

        private IEnumerator RunQtePhase(QteType qteType)
        {
            bool completed = false;
            bool result = false;

            void OnCompleted(bool success)
            {
                completed = true;
                result = success;
                _qteService.Completed -= OnCompleted;
            }

            _qteService.Completed += OnCompleted;
            SetTimeScaleForQte(qteType);
            _qteService.Start(qteType);

            yield return new WaitUntil(() => completed);

            Time.timeScale = 1f;
            _lastQteSuccess = result;

            Debug.Log($"QTE {qteType} завершено. Успех: {_lastQteSuccess}");
        }

        private void SetTimeScaleForQte(QteType type)
        {
            Time.timeScale = type switch
            {
                QteType.TapToButton => 0.1f,
                QteType.SliderBack => 0.05f,
                QteType.SliderForward => 0.1f,
                _ => 1f
            };
        }

        private void HandleCamera(CameraActionType actionType)
        {
            switch (actionType)
            {
                case CameraActionType.FocusOnSource:
                    _source.virtualCamera.enabled = true;
                    _coroutineRunner.StartCoroutine(WaitForBlendEnd());
                    break;

                case CameraActionType.MoveBack:
                    _source.virtualCamera.enabled = false;
                    break;
            }
        }

        private IEnumerator WaitForBlendEnd()
        {
            yield return new WaitForEndOfFrame();
            
            while (_cinemachineBrain.ActiveBlend != null)
                yield return null;
            
            FinishAnimation();
        }

        private IEnumerator WaitForAnimation()
        {
            _animationPlaying = true; 
            
            void OnFinished() => FinishAnimation();

            _animatorController.Finished += OnFinished;
            yield return new WaitWhile(() => _animationPlaying);
            _animatorController.Finished -= OnFinished;
        }

        private IEnumerator MoveUnit(Unit unit, Vector3 target)
        {
            float liftDelay = 0.6f;
            int jumpPower = 2;
            var duration = _animatorController.GetAnimationLength();

            yield return new WaitForSeconds(liftDelay);

            float moveDuration = duration - liftDelay;
            unit.transform.DOKill();

            Tween jumpTween = unit.transform
                .DOJump(target, jumpPower, 1, moveDuration)
                .SetEase(Ease.InQuad);

            yield return jumpTween.WaitForCompletion();
        }

        private Vector3 CalculateTargetPosition(Vector3 start, Vector3 target)
        {
            float stopDistance = 1.5f;
            Vector3 direction = (target - start).normalized;
            return target - direction * stopDistance;
        }

        private void FinishAbility()
        {
            _animatorController.Play(Constants.BaseAnimations.Idle);
            Finished?.Invoke();
        }
        
        private void FinishAnimation() => 
            _animationPlaying = false;
    }
}