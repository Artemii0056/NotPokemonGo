using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Flow;
using Cysharp.Threading.Tasks;
using Units;

namespace AbilityNew.Scripts.Executors.Flow
{
    public class PrepareArmamentSpawnPointsExecutor 
        : AbilityStepExecutor<PrepareArmamentSpawnPointsStep>
    {
        public override UniTask Execute(
            PrepareArmamentSpawnPointsStep step,
            AbilityExecutionRuntime runtime,
            CancellationToken ct)
        {
            var state = runtime.State;
            var context = runtime.Context;

            Unit unit = context.Source;
            
            if (unit == null)
                return UniTask.CompletedTask;

            if (step.ClearBeforeInit)
                state.FreeArmamentSpawnPoints.Clear();

            foreach (var point in unit.AbilitiesPositions)
            {
                if (point != null)
                    state.FreeArmamentSpawnPoints.Enqueue(point);
            }

            return UniTask.CompletedTask;
        }
    }
}