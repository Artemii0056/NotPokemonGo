using System;

namespace AbilityNew.Scripts.Presentation
{
    public abstract class PresentationStepExecutor<TStep> : IPresentationStepExecutor
        where TStep : PresentationStep
    {
        public Type StepType => typeof(TStep);

        public void Execute(PresentationStep step, AbilityPresentationContext context)
        {
            Execute((TStep)step, context);
        }

        protected abstract void Execute(TStep step, AbilityPresentationContext context);
    }
}