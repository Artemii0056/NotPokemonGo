using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Configs;
using Cysharp.Threading.Tasks;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts.Executors
{
    public interface IAbilityStepExecutor
    {
        UniTask Execute(AbilityStepSO step, AbilityContext ctx);
    }
}