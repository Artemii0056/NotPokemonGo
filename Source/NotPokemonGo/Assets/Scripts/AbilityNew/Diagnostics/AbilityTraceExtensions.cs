using System;
using AbilityNew.AbilityDefinition;
using UnityEngine;

namespace AbilityNew.Diagnostics
{
    public static class AbilityTraceExtensions
    {
        public static void TraceStepEnter(
            this AbilityExecutionRuntime runtime,
            string stepName,
            string executorName,
            string message = "")
        {
            runtime.TraceWriter?.Write(new AbilityTraceRecord
            {
                ExecutionId = runtime.ExecutionId,
                AbilityName = runtime.Ability?.name ?? "UnknownAbility",
                SourceName = runtime.Context?.Source?.name ?? "UnknownSource",
                TargetName = runtime.Context?.Target?.name ?? "NoTarget",
                StepName = stepName,
                ExecutorName = executorName,
                Phase = AbilityTracePhase.Enter,
                Source = AbilityTraceSource.Executor,
                Message = message,
                TimestampUtcTicks = DateTime.UtcNow.Ticks,
                Frame = Time.frameCount
            });
        }

        public static void TraceStepExit(
            this AbilityExecutionRuntime runtime,
            string stepName,
            string executorName,
            string message = "")
        {
            runtime.TraceWriter?.Write(new AbilityTraceRecord
            {
                ExecutionId = runtime.ExecutionId,
                AbilityName = runtime.Ability?.name ?? "UnknownAbility",
                SourceName = runtime.Context?.Source?.name ?? "UnknownSource",
                TargetName = runtime.Context?.Target?.name ?? "NoTarget",
                StepName = stepName,
                ExecutorName = executorName,
                Phase = AbilityTracePhase.Exit,
                Source = AbilityTraceSource.Executor,
                Message = message,
                TimestampUtcTicks = DateTime.UtcNow.Ticks,
                Frame = Time.frameCount
            });
        }

        public static void TraceInfo(
            this AbilityExecutionRuntime runtime,
            AbilityTraceSource source,
            string stepName,
            string executorName,
            string message)
        {
            runtime.TraceWriter?.Write(new AbilityTraceRecord
            {
                ExecutionId = runtime.ExecutionId,
                AbilityName = runtime.Ability?.name ?? "UnknownAbility",
                SourceName = runtime.Context?.Source?.name ?? "UnknownSource",
                TargetName = runtime.Context?.Target?.name ?? "NoTarget",
                StepName = stepName,
                ExecutorName = executorName,
                Phase = AbilityTracePhase.Info,
                Source = source,
                Message = message,
                TimestampUtcTicks = DateTime.UtcNow.Ticks,
                Frame = Time.frameCount
            });
        }

        public static void TraceError(
            this AbilityExecutionRuntime runtime,
            string stepName,
            string executorName,
            Exception exception)
        {
            runtime.TraceWriter?.Write(new AbilityTraceRecord
            {
                ExecutionId = runtime.ExecutionId,
                AbilityName = runtime.Ability?.name ?? "UnknownAbility",
                SourceName = runtime.Context?.Source?.name ?? "UnknownSource",
                TargetName = runtime.Context?.Target?.name ?? "NoTarget",
                StepName = stepName,
                ExecutorName = executorName,
                Phase = AbilityTracePhase.Error,
                Source = AbilityTraceSource.Executor,
                Message = exception.ToString(),
                TimestampUtcTicks = DateTime.UtcNow.Ticks,
                Frame = Time.frameCount
            });
        }
    }
}