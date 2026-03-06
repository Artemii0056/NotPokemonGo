using System.Collections.Generic;
using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem;
using AbsolutelyNewPerfectAbilitySystem.Configs;
using AbsolutelyNewPerfectAbilitySystem.Steps;
using Cysharp.Threading.Tasks;

public class ParallelExecutor : IAbilityStepExecutor
{
    private StepExecutorRegistry _registry;

    public async UniTask Execute(AbilityStepSO step, AbilityContext ctx)
    {
        var parallel = (ParallelStep)step;

        var tasks = new List<UniTask>();

        foreach (var child in parallel.Steps) 
            tasks.Add(_registry.Execute(child, ctx));

        await UniTask.WhenAll(tasks);
    }
}