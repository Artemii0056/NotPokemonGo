using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Abilities.Flow
{
    public sealed class BranchStep : IAbilityStep
    {
        private readonly Func<AbilityExecutionContext, bool> _predicate;
        private readonly IAbilityStep _trueStep;

        public BranchStep(Func<AbilityExecutionContext, bool> predicate, IAbilityStep trueStep)
        {
            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            _trueStep = trueStep ?? throw new ArgumentNullException(nameof(trueStep));
        }

        public UniTask Execute(AbilityExecutionContext ctx, CancellationToken token)
        {
            if (_predicate(ctx))
                return _trueStep.Execute(ctx, token);

            return UniTask.CompletedTask;
        }
    }
}
