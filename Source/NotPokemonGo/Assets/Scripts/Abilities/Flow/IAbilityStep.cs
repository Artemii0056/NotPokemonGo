using Cysharp.Threading.Tasks;
using System.Threading;

namespace Abilities.Flow
{
    public interface IAbilityStep
    {
        UniTask Execute(AbilityExecutionContext ctx, CancellationToken token);
    }
}
