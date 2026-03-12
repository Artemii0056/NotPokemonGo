using System;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.Configs;
using AbilityNew.Scripts.Steps.Flow;
using AbilityNew.Scripts.Steps.Gameplay;
using AbilityNew.Scripts.Steps.Presentation;

namespace AbilityNew.Scripts.Validation
{
    public sealed class AbilityValidator : IAbilityValidator
    {
        public AbilityValidationResult Validate(AbilitySO ability, StepExecutorRegistry registry)
        {
            var result = new AbilityValidationResult();

            if (ability == null)
            {
                result.AddError("Ability is null.");
                return result;
            }

            if (registry == null)
            {
                result.AddError($"Ability '{ability.name}': StepExecutorRegistry is null.");
                return result;
            }

            if (ability.Steps == null)
            {
                result.AddError($"Ability '{ability.name}': Steps collection is null.");
                return result;
            }

            if (ability.Steps.Count == 0)
            {
                result.AddError($"Ability '{ability.name}': Steps collection is empty.");
                return result;
            }

            for (int i = 0; i < ability.Steps.Count; i++)
            {
                ValidateStep(
                    step: ability.Steps[i],
                    registry: registry,
                    result: result,
                    path: $"{ability.name}/Steps[{i}]");
            }

            return result;
        }

        private void ValidateStep(
            AbilityStepSO step,
            StepExecutorRegistry registry,
            AbilityValidationResult result,
            string path)
        {
            if (step == null)
            {
                result.AddError($"{path}: Step is null.");
                return;
            }

            if (registry.HasExecutor(step.GetType()) == false)
            {
                result.AddError($"{path}: Executor for step type '{step.GetType().Name}' is not registered.");
            }

            switch (step)
            {
                case SequenceStep sequenceStep:
                    ValidateSequenceStep(sequenceStep, registry, result, path);
                    break;

                case ParallelStep parallelStep:
                    ValidateParallelStep(parallelStep, registry, result, path);
                    break;

                case RepeatStep repeatStep:
                    ValidateRepeatStep(repeatStep, registry, result, path);
                    break;

                case ResolveQteStep resolveQteStep:
                    ValidateResolveQteStep(resolveQteStep, registry, result, path);
                    break;

                case DamageStep damageStep:
                    ValidateDamageStep(damageStep, result, path);
                    break;

                case HealStep healStep:
                    ValidateHealStep(healStep, result, path);
                    break;

                case ApplyStatusStep applyStatusStep:
                    ValidateApplyStatusStep(applyStatusStep, result, path);
                    break;

                case RemoveStatusStep removeStatusStep:
                    ValidateRemoveStatusStep(removeStatusStep, result, path);
                    break;

                case ApplyShieldStep applyShieldStep:
                    ValidateApplyShieldStep(applyShieldStep, result, path);
                    break;

                case WaitSignalStep waitSignalStep:
                    ValidateWaitSignalStep(waitSignalStep, result, path);
                    break;

                case PlayAnimationStep playAnimationStep:
                    ValidatePlayAnimationStep(playAnimationStep, result, path);
                    break;
            }
        }

        private void ValidateSequenceStep(
            SequenceStep step,
            StepExecutorRegistry registry,
            AbilityValidationResult result,
            string path)
        {
            if (step.Steps == null)
            {
                result.AddError($"{path}: SequenceStep.Steps is null.");
                return;
            }

            if (step.Steps.Count == 0)
            {
                result.AddError($"{path}: SequenceStep.Steps is empty.");
                return;
            }

            for (int i = 0; i < step.Steps.Count; i++)
            {
                ValidateStep(step.Steps[i], registry, result, $"{path}/Sequence[{i}]");
            }
        }

        private void ValidateParallelStep(
            ParallelStep step,
            StepExecutorRegistry registry,
            AbilityValidationResult result,
            string path)
        {
            if (step.Steps == null)
            {
                result.AddError($"{path}: ParallelStep.Steps is null.");
                return;
            }

            if (step.Steps.Count == 0)
            {
                result.AddError($"{path}: ParallelStep.Steps is empty.");
                return;
            }

            for (int i = 0; i < step.Steps.Count; i++)
            {
                ValidateStep(step.Steps[i], registry, result, $"{path}/Parallel[{i}]");
            }
        }

        private void ValidateRepeatStep(
            RepeatStep step,
            StepExecutorRegistry registry,
            AbilityValidationResult result,
            string path)
        {
            if (step.Count <= 0)
            {
                result.AddError($"{path}: RepeatStep.Count must be greater than zero.");
            }

            if (step.Step == null)
            {
                result.AddError($"{path}: RepeatStep.Step is null.");
                return;
            }

            ValidateStep(step.Step, registry, result, $"{path}/Repeat");
        }

        private void ValidateResolveQteStep(
            ResolveQteStep step,
            StepExecutorRegistry registry,
            AbilityValidationResult result,
            string path)
        {
            bool hasAnyBranch =
                step.OnFail != null ||
                step.OnNormal != null ||
                step.OnPerfect != null;

            if (hasAnyBranch == false)
            {
                result.AddError($"{path}: ResolveQteStep has no branches configured.");
                return;
            }

            if (step.OnFail != null)
                ValidateStep(step.OnFail, registry, result, $"{path}/OnFail");

            if (step.OnNormal != null)
                ValidateStep(step.OnNormal, registry, result, $"{path}/OnNormal");

            if (step.OnPerfect != null)
                ValidateStep(step.OnPerfect, registry, result, $"{path}/OnPerfect");
        }

        private void ValidateDamageStep(
            DamageStep step,
            AbilityValidationResult result,
            string path)
        {
            if (step.Effect.Value <= 0)
            {
                result.AddError($"{path}: DamageStep.Effect.Value must be greater than zero.");
            }
        }

        private void ValidateHealStep(
            HealStep step,
            AbilityValidationResult result,
            string path)
        {
            if (step.Effect.Value <= 0)
            {
                result.AddError($"{path}: HealStep.Effect.Value must be greater than zero.");
            }
        }

        private void ValidateApplyStatusStep(
            ApplyStatusStep step,
            AbilityValidationResult result,
            string path)
        {
            if (step.Status == null)
            {
                result.AddError($"{path}: ApplyStatusStep.Status is null.");
            }
        }

        private void ValidateRemoveStatusStep(
            RemoveStatusStep step,
            AbilityValidationResult result,
            string path)
        {
            // if (!Enum.IsDefined(step.StatusType))
            // {
            //     result.AddError($"{path}: RemoveStatusStep.StatusType is invalid.");
            // }
        }

        private void ValidateApplyShieldStep(
            ApplyShieldStep step,
            AbilityValidationResult result,
            string path)
        {
            if (step.Value <= 0)
            {
                result.AddError($"{path}: ApplyShieldStep.Value must be greater than zero.");
            }

            // if (step.DurationTurns < 0)
            // {
            //     result.AddError($"{path}: ApplyShieldStep.DurationTurns cannot be negative.");
            // }
        }

        private void ValidateWaitSignalStep(
            WaitSignalStep step,
            AbilityValidationResult result,
            string path)
        {
            if (string.IsNullOrWhiteSpace(step.Signal.ToString()))
            {
                result.AddError($"{path}: WaitSignalStep.Signal is empty.");
            }
        }

        private void ValidatePlayAnimationStep(
            PlayAnimationStep step,
            AbilityValidationResult result,
            string path)
        {
            if (step.Animation == null)
            {
                result.AddError($"{path}: PlayAnimationStep.Animation is null.");
            }
        }
    }
}