using System;

namespace AbilityNew.Scripts.Presentation
{
    public interface IPresentationStepExecutor
    {
        Type StepType { get; }
        void Execute(PresentationStep step, AbilityPresentationContext context);
    }
}