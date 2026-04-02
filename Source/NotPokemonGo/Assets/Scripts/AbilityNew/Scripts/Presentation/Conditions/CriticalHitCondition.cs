using System;

namespace AbilityNew.Scripts.Presentation.Conditions
{
    [Serializable]
    public sealed class CriticalHitCondition : PresentationCondition
    {
        public override bool Evaluate(AbilityPresentationContext context) =>
            context.IsCriticalHit;
    }
}