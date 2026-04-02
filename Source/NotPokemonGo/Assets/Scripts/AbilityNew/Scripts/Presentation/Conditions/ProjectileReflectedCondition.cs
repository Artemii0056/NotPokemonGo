using System;

namespace AbilityNew.Scripts.Presentation.Conditions
{
    [Serializable]
    public sealed class ProjectileReflectedCondition : PresentationCondition
    {
        public override bool Evaluate(AbilityPresentationContext context) =>
            context.ProjectileReflected;
    }
}