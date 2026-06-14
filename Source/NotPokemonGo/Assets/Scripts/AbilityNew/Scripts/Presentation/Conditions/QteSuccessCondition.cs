using System;
using QteSystem.Core;

namespace AbilityNew.Scripts.Presentation.Conditions
{
    [Serializable]
    public sealed class QteSuccessCondition : PresentationCondition
    {
        public override bool Evaluate(AbilityPresentationContext context) =>
            context.QteResult is QteResult.Normal or QteResult.Perfect;
    }
}