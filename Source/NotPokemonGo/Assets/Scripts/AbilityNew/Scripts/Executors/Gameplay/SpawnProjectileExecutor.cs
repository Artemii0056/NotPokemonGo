using System;
using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Gameplay;
using Armaments;
using Cysharp.Threading.Tasks;
using Spawners.Spawner;

namespace AbilityNew.Scripts.Executors.Gameplay
{
    public class
        SpawnProjectileExecutor : AbilityStepExecutor<SpawnProjectileStep> //сервис, отвечающий за количество? Выбирающий скорее
    {
        private readonly IArmamentSpawner _armamentSpawner;

        public SpawnProjectileExecutor(IArmamentSpawner armamentSpawner) =>
            _armamentSpawner = armamentSpawner;

        public override async UniTask Execute(SpawnProjectileStep step, AbilityExecutionRuntime runtime, CancellationToken ct)
        {
            var context = runtime.Context;

            var mover = _armamentSpawner.Create(
                new ArmamentContext(
                    context.Source,
                    context.Target,
                    step.ArmamentSetup,
                    step.ArmamentSetup.FlyingType,
                    context.Source.transform)
            );

            runtime.State.Movers.Add(mover);

            if (step.Delay > 0)
                await UniTask.Delay(TimeSpan.FromSeconds(step.Delay));
        }
    }
}