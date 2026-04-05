using System;
using QteSystem.Core;
using QteSystem.Gameplay;

namespace AbilityNew.Scripts.Presentation.Conditions
{
    [Serializable]
    public sealed class QteFailCondition : PresentationCondition
    {
        public override bool Evaluate(AbilityPresentationContext context) =>
            context.QteResult == QteResult.Fail;
    }
}