using System;

namespace AbilityNew.Scripts.Presentation.Conditions
{
    [Serializable]
    public sealed class QteSeriesAllSuccessPresentationCondition : PresentationCondition
    {
        public override bool Evaluate(AbilityPresentationContext context) =>
            context.QteSeriesResult != null && context.QteSeriesResult.AllSuccessOrBetter;
    }
}