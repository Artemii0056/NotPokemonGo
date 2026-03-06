using System;
using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem.Configs;
using AbsolutelyNewPerfectAbilitySystem.Steps;
using Armaments;
using Cysharp.Threading.Tasks;
using Spawners.Spawner;

public class SpawnProjectileExecutor : IAbilityStepExecutor
{
    private readonly IArmamentSpawner _armamentSpawner;

    public SpawnProjectileExecutor(IArmamentSpawner armamentSpawner) => 
        _armamentSpawner = armamentSpawner;

    public async UniTask Execute(AbilityStepSO step, AbilityContext ctx)
    {
        var data = (SpawnProjectileStepSO)step;

        _armamentSpawner.Create(
            new ArmamentContext(
                ctx.Source, 
                ctx.Target, 
                data.ArmamentSetup, 
                data.ArmamentSetup.FlyingType, 
                ctx.Source.transform)
            );

        if (data.Delay > 0)
            await UniTask.Delay(TimeSpan.FromSeconds(data.Delay));
    }
}