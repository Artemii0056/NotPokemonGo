using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Abilities.Flow
{
    public sealed class ParallelStep : IAbilityStep
    {
        private readonly IAbilityStep[] _steps;

        public ParallelStep(params IAbilityStep[] steps) => _steps = steps ?? Array.Empty<IAbilityStep>();

        public UniTask Execute(AbilityExecutionContext ctx, CancellationToken token)
        {
            var tasks = _steps.Select(s => s.Execute(ctx, token));
            return UniTask.WhenAll(tasks);
        }
    }
}
