using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem.Configs;
using Cysharp.Threading.Tasks;

namespace AbsolutelyNewPerfectAbilitySystem.Steps
{
    public interface IAbilityStepExecutor
    {
        UniTask Execute(AbilityStepSO step, AbilityContext ctx);
    }
}