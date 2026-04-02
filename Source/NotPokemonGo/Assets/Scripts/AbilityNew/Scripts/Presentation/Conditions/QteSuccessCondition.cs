using System;

namespace AbilityNew.Scripts.Presentation.Conditions
{
    [Serializable]
    public sealed class QteSuccessCondition : PresentationCondition
    {
        public override bool Evaluate(AbilityPresentationContext context) =>
            context.Ability; //TODO Подправить потом
    }
}