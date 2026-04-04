using System;
using System.Threading;
using AbilityNew.Diagnostics;
using AbilityNew.Scripts;

namespace AbilityNew.AbilityDefinition
{
    public sealed class AbilityExecutionRuntime
    {
        public AbilitySO Ability { get; }
        public AbilityExecutionContext Context { get; }
        public AbilityExecutionState State { get; }
        public AbilityExecutionResult Result { get; }
        public CancellationToken CancellationToken { get; }

        public string ExecutionId { get; }
        public IAbilityTraceWriter TraceWriter { get; }

        public AbilityExecutionRuntime(
            AbilitySO ability,
            AbilityExecutionContext context,
            CancellationToken cancellationToken,
            IAbilityTraceWriter traceWriter)
        {
            Ability = ability;
            Context = context;
            CancellationToken = cancellationToken;
            TraceWriter = traceWriter;

            ExecutionId = $"{DateTime.UtcNow:yyyyMMdd_HHmmss_fff}_{Guid.NewGuid().ToString("N").Substring(0, 6)}";

            State = new AbilityExecutionState();
            Result = new AbilityExecutionResult();
        }
    }
}