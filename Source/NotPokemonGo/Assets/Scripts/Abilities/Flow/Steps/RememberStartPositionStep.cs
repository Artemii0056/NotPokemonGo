using System.Threading;
using Cysharp.Threading.Tasks;

namespace Abilities.Flow.Steps
{
    public sealed class RememberStartPositionStep : IAbilityStep
    {
        public UniTask Execute(AbilityExecutionContext ctx, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (ctx?.Source != null)
                ctx.Source.SetStartPosition(ctx.Source.transform.position);

            return UniTask.CompletedTask;
        }
    }
}
