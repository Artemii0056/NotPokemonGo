using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Presentation;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Presentation;
using AbilityNew.Scripts.Steps.Presentation;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts.Executors.Presentation
{
    public sealed class EmitPresentationSignalExecutor : AbilityStepExecutor<EmitPresentationSignalStep>
    {
        private readonly IAbilityPresentationService _presentationService;

        public EmitPresentationSignalExecutor(IAbilityPresentationService presentationService)
        {
            _presentationService = presentationService;
        }

        public override UniTask Execute(
            EmitPresentationSignalStep step,
            AbilityExecutionRuntime runtime,
            CancellationToken ct)
        {
            var context = new AbilityPresentationContext
            {
                Caster = runtime.Context.Source,
                Target = runtime.Context.Target,
                Ability = runtime.Ability,
                Signal = step.Signal,
                SpawnType = step.SpawnType
            };

            _presentationService.Play(context);
            return UniTask.CompletedTask;
        }
    }
}