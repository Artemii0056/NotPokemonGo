using System;
using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Configs;
using Armaments;
using Cysharp.Threading.Tasks;
using Spawners.Spawner;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts.Executors
{
    public class SpawnProjectileExecutor : IAbilityStepExecutor //сервис, отвечающий за количество? Выбирающий скорее
    {
        private readonly IArmamentSpawner _armamentSpawner;

        public SpawnProjectileExecutor(IArmamentSpawner armamentSpawner) => 
            _armamentSpawner = armamentSpawner;

        public async UniTask Execute(AbilityStepSO step, AbilityContext ctx)
        {
            var data = (SpawnProjectileStep)step;

           var mover =  _armamentSpawner.Create(
                new ArmamentContext(
                    ctx.Source, 
                    ctx.Target, 
                    data.ArmamentSetup, 
                    data.ArmamentSetup.FlyingType, 
                    ctx.Source.transform)
            );
           
            ctx.Movers.Add(mover);

            if (data.Delay > 0)
                await UniTask.Delay(TimeSpan.FromSeconds(data.Delay));
        }
    }
}