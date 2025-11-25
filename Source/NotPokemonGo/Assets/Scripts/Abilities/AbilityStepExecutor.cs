using System;
using System.Collections;
using Abilities.AbilitySteps;
using Cameras;
using DG.Tweening;
using Services;
using Services.AbilityServices;
using Services.QTEServices;
using Units;
using UnityEngine;
using Cinemachine;

namespace Abilities
{
    public class AbilityStepExecutor : IAbilityStepExecutor
    {
        private readonly ICoroutineRunner _runner;
        private readonly AbilityPhaseService _phaseService;
        private readonly IQteService _qteService;
        private readonly CinemachineBrain _cinemachineBrain;

        public AbilityStepExecutor(
            ICoroutineRunner runner,
            AbilityPhaseService phaseService,
            IQteService qteService,
            CinemachineBrain cinemachineBrain)
        {
            _runner = runner;
            _phaseService = phaseService;
            _qteService = qteService;
            _cinemachineBrain = cinemachineBrain;
        }

        public IEnumerator ExecuteStep(
            AbilityStepData step,
            Unit source,
            Unit target,
            ExecutionContext ctx)
        {
            switch (step)
            {
                case ParallelStepAbilityData parallel:
                    yield return ExecuteParallel(parallel, source, target, ctx);
                    yield break;

                case CastamentStepData cast:
                    if (cast.WhenQteSuccess == false || ctx.LastQteSuccess)
                        _phaseService.OnNext(cast, source, target);
                    yield break;

                case ArmamentStepData arm:
                    if (arm.WhenQteSuccess == false || ctx.LastQteSuccess)
                        _phaseService.OnNext(arm, source, target);
                    yield break;

                case PlayAnimationStepData anim:
                    yield return PlayAnimation(anim, source, target);
                    yield break;

                case QteStepData qte:
                    yield return RunQte(qte, ctx);
                    yield break;

                case MovementStepData move:
                    yield return Move(move, source, target);
                    yield break;

                case CameraStepData camera:
                    yield return HandleCamera(camera, source);
                    yield break;

                default:
                    throw new Exception($"Unknown AbilityStepData type: {step?.GetType().Name}");
            }
        }

        // -------------------
        // ПАРАЛЛЕЛЬНЫЙ ШАГ
        // -------------------
        private IEnumerator ExecuteParallel(
            ParallelStepAbilityData parallel,
            Unit source,
            Unit target,
            ExecutionContext context)
        {
            if (parallel.Steps == null || parallel.Steps.Count == 0)
                yield break;

            int remaining = parallel.Steps.Count;

            IEnumerator RunSingle(AbilityStepData step)
            {
                // выполняем вложенный шаг
                yield return ExecuteStep(step, source, target, context);
                // после завершения шага уменьшаем счётчик
                remaining--;
            }

            // запускаем ВСЕ подшаги одновременно
            foreach (var step in parallel.Steps)
                _runner.StartCoroutine(RunSingle(step));

            // ждём, пока все подшаги закончатся
            yield return new WaitUntil(() => remaining <= 0);
        }

        // -------------------
        // АНИМАЦИЯ
        // -------------------
        private IEnumerator PlayAnimation(
            PlayAnimationStepData data,
            Unit source,
            Unit target)
        {
            var animator = source.UnitAnimatorController;
            var trigger = source.AnimatorTrigger;

            trigger.SetTarget(target);
            // если нужно — можно сделать SetPhaseFromStep(data)
            animator.Play(Animator.StringToHash(data.Clip.name));

            if (!data.WaitForFinish)
                yield break;

            bool playing = true;

            void OnFinished() => playing = false;

            animator.Finished += OnFinished;
            yield return new WaitWhile(() => playing);
            animator.Finished -= OnFinished;
        }

        // -------------------
        // QTE
        // -------------------
        private IEnumerator RunQte(QteStepData data, ExecutionContext ctx)
        {
            bool completed = false;
            bool success = false;

            void OnCompleted(bool ok)
            {
                completed = true;
                success = ok;
                _qteService.Completed -= OnCompleted;
            }

            _qteService.Completed += OnCompleted;

            float prevTimeScale = Time.timeScale;
            Time.timeScale = data.TimeScale;

            _qteService.Start(data.QteType);

            yield return new WaitUntil(() => completed);

            Time.timeScale = prevTimeScale;

            ctx.LastQteSuccess = success;
        }

        // -------------------
        // ДВИЖЕНИЕ
        // -------------------
        private IEnumerator Move(
            MovementStepData data,
            Unit source,
            Unit target)
        {
            // стартовая позиция — можно кешировать в Unit, если нужно
            Vector3 startPos = source.transform.position;

            if (data.ReturnToStart)
            {
                yield return MoveRun(source, startPos, 0f, data);
                yield break;
            }

            Vector3 targetPos = target.transform.position;
            Vector3 direction = (targetPos - startPos).normalized;
            Vector3 stopPos = targetPos - direction * data.StopDistance;

            switch (data.Mode)
            {
                case MovementStepData.MovementMode.Run:
                    yield return MoveRun(source, stopPos, data.LiftDelay, data);
                    break;

                case MovementStepData.MovementMode.Jump:
                    yield return MoveJump(source, stopPos, data.LiftDelay, data);
                    break;

                case MovementStepData.MovementMode.Teleport:
                    if (data.LiftDelay > 0f)
                        yield return new WaitForSeconds(data.LiftDelay);
                    source.transform.position = stopPos;
                    break;
            }
        }

        private IEnumerator MoveRun(
            Unit unit,
            Vector3 targetPos,
            float delay,
            MovementStepData data)
        {
            const float speed = 4f;

            if (delay > 0f)
                yield return new WaitForSeconds(delay);

            while (Vector3.Distance(unit.transform.position, targetPos) > 0.01f)
            {
                unit.transform.position = Vector3.MoveTowards(
                    unit.transform.position,
                    targetPos,
                    speed * Time.deltaTime);

                yield return null;
            }
        }

        private IEnumerator MoveJump(
            Unit unit,
            Vector3 targetPos,
            float delay,
            MovementStepData data)
        {
            if (delay > 0f)
                yield return new WaitForSeconds(delay);

            unit.transform.DOKill();

            float duration = 0.5f; // можно связать с длиной анимации, если нужно

            Tween jumpTween = unit.transform
                .DOJump(targetPos, data.JumpPower, 1, duration)
                .SetEase(Ease.InQuad);

            yield return jumpTween.WaitForCompletion();
        }

        // -------------------
        // КАМЕРА
        // -------------------
        private IEnumerator HandleCamera(CameraStepData data, Unit source)
        {
            switch (data.ActionType)
            {
                case CameraActionType.FocusOnSource:
                    source.virtualCamera.enabled = true;
                    yield return WaitForBlendEnd();
                    break;

                case CameraActionType.MoveBack:
                    source.virtualCamera.enabled = false;
                    break;
            }
        }

        private IEnumerator WaitForBlendEnd()
        {
            // дождаться начала бленда
            yield return new WaitForEndOfFrame();

            while (_cinemachineBrain != null &&
                   _cinemachineBrain.ActiveBlend != null)
            {
                yield return null;
            }
        }
    }
    
    public class ExecutionContext
    {
        public bool LastQteSuccess;
    }
}
