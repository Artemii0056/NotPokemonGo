using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem;
using AbsolutelyNewPerfectAbilitySystem.Configs;
using AbsolutelyNewPerfectAbilitySystem.Steps;
using Cysharp.Threading.Tasks;

public class RepeatExecutor : IAbilityStepExecutor
{
    private readonly StepExecutorRegistry _registry;

    public RepeatExecutor(StepExecutorRegistry registry) => 
        _registry = registry;

    public async UniTask Execute(AbilityStepSO step, AbilityContext ctx)
    {
        var repeat = (RepeatStep)step;

        for (int i = 0; i < repeat.Count; i++) 
            await _registry.Execute(repeat.Step, ctx);
    }
}