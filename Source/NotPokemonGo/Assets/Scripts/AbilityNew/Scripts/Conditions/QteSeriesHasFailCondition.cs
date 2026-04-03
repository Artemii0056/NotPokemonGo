using System;
using AbilityNew.AbilityDefinition;
using UnityEngine;

namespace AbilityNew.Scripts.Conditions
{
    [Serializable]
    public sealed class QteSeriesHasFailCondition : AbilityCondition
    {
        public override bool Evaluate(AbilityExecutionRuntime runtime)
        {
            return Text(runtime);
        }

        public bool Text(AbilityExecutionRuntime runtime)
        {
            var a = runtime.State.AbilityBlackboard.TryGet(
                BlackboardKey.QteSeriesResult,
                out QteSeriesResult series);
            
            Debug.Log($"Series has fail: {series.HasFail}, Count: {series.Results.Count}");

            return a && series.HasFail;
        }
    }
}