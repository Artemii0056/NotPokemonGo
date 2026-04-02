using System;

namespace AbilityNew.Scripts.Presentation.Conditions
{
    [Serializable]
    public sealed class AlwaysPresentationCondition : PresentationCondition
    {
        public override bool Evaluate(AbilityPresentationContext context) => true;
    }
}