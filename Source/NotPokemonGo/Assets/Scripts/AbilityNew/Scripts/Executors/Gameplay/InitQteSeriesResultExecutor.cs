using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Gameplay;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts.Executors.Gameplay
{
    public sealed class InitQteSeriesResultExecutor : AbilityStepExecutor<InitQteSeriesResultStep>
    {
        public override UniTask Execute(
            InitQteSeriesResultStep step,
            AbilityExecutionRuntime runtime,
            CancellationToken ct)
        {
            runtime.State.AbilityBlackboard.Set(
                BlackboardKey.QteSeriesResult,
                new QteSeriesResult());

            return UniTask.CompletedTask;
        }
    }
}