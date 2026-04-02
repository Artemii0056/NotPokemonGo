using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Presentation;
using AbilityNew.Scripts.Steps.Presentation;
using Armaments;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AbilityNew.Scripts.Executors.Presentation
{
    public sealed class EmitPresentationSignalExecutor : AbilityStepExecutor<EmitPresentationSignalStep>
    {
        private readonly IAbilityPresentationService _presentationService;

        public EmitPresentationSignalExecutor(IAbilityPresentationService presentationService) => 
            _presentationService = presentationService;

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
            };

            Debug.LogWarning("EmitPresentationSignalExecutor");
            
            if (step.Signal == AbilityPresentationSignal.ProjectileLaunched ||
            step.Signal == AbilityPresentationSignal.ProjectileSpawned ||
                step.Signal == AbilityPresentationSignal.ProjectileHighlighted)
            {
                if (runtime.State.AbilityBlackboard.TryGet(BlackboardKey.CurrentArmament, out Armament armament) &&
                    armament != null)
                {
                    context.ExplicitTransform = armament.transform;
                }
            }

            _presentationService.Play(context);
            return UniTask.CompletedTask;
        }
    }
}