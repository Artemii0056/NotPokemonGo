using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Abilities.Flow
{
    public sealed class AbilityPipelineExecutor
    {
        private readonly List<IAbilityStep> _steps;

        public AbilityPipelineExecutor(params IAbilityStep[] steps)
        {
            _steps = steps != null ? new List<IAbilityStep>(steps) : new List<IAbilityStep>();
        }

        public async UniTask Execute(AbilityExecutionContext ctx, CancellationToken token)
        {
            if (ctx == null) throw new ArgumentNullException(nameof(ctx));

            for (int i = 0; i < _steps.Count; i++)
            {
                token.ThrowIfCancellationRequested();
                await _steps[i].Execute(ctx, token);
            }
        }
    }
}
