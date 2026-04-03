using System;
using AbilityNew.AbilityDefinition;

namespace AbilityNew.Scripts.Conditions
{
    [Serializable]
    public sealed class QteSeriesAllSuccessCondition : AbilityCondition
    {
        public override bool Evaluate(AbilityExecutionRuntime runtime)
        {
            return runtime.State.AbilityBlackboard.TryGet(
                       BlackboardKey.QteSeriesResult,
                       out QteSeriesResult series)
                   && series.AllSuccessOrBetter;
        }
    }
}