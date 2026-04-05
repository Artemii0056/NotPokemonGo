using System;
using AbilityNew;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts;

namespace Abilities
{
    public sealed class AbilityExecutionSession : IDisposable
    {
        public AbilityExecutionSession(
            AbilityExecutionContext context,
            StepExecutorRegistry registry,
            IAbilityPresentationService presentationService,
            SignalService signalService,
            AnimationSignalRelay animationSignalRelay)
        {
            Context = context;
            Registry = registry;
            PresentationService = presentationService;
            SignalService = signalService;
            AnimationSignalRelay = animationSignalRelay;
        }
        
        public AbilityExecutionContext Context { get; }
        public StepExecutorRegistry Registry { get; }
        public IAbilityPresentationService PresentationService { get; }
        public SignalService SignalService { get; }
        public AnimationSignalRelay AnimationSignalRelay { get; }

        public void Dispose()
        {
            AnimationSignalRelay?.Dispose();
        }
    }
}