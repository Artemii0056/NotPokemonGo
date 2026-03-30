using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Debugger;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AbilityNew.Scripts.Executors.Debugger
{
    public class DebugExecutor : AbilityStepExecutor<DebugStep>
    {
        public override UniTask Execute(DebugStep step, AbilityExecutionRuntime runtime, CancellationToken ct)
        {
            Debug.Log(step.Text);
            return UniTask.CompletedTask;
        }
    }
}