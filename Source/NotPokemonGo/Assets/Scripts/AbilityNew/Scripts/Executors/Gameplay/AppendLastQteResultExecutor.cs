using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps;
using AbilityNew.Scripts.Steps.Gameplay;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AbilityNew.Scripts.Executors.Gameplay
{
    public sealed class AppendLastQteResultExecutor : AbilityStepExecutor<AppendLastQteResultStep>
    {
        public override UniTask Execute(
            AppendLastQteResultStep step,
            AbilityExecutionRuntime runtime,
            CancellationToken ct)
        {
            if (runtime.State.AbilityBlackboard.TryGet(
                    BlackboardKey.QteSeriesResult,
                    out QteSeriesResult series))
            {
                if (runtime.State.LastQteResult != null)
                {
                    Debug.Log($"QTE appended: {runtime.State.LastQteResult}");
                    series.Add(runtime.State.LastQteResult.Value);
                }
            }

            return UniTask.CompletedTask;
        }
    }
}