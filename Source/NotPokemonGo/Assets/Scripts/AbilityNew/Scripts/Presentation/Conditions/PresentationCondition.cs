using System;

namespace AbilityNew.Scripts.Presentation.Conditions
{
    [Serializable]
    public abstract class PresentationCondition
    {
        public abstract bool Evaluate(AbilityPresentationContext context);
    }
}