using System;
using Abilities;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Gameplay;
using Cysharp.Threading.Tasks;
using Effects;
using Units;
using UnityEngine;

namespace AbilityNew.Scripts.Executors.Gameplay
{
    public sealed class DamageStepExecutor : AbilityStepExecutor<DamageStep>
    {
        private readonly IEffectResolver _effectResolver;

        public DamageStepExecutor(IEffectResolver effectResolver) => 
            _effectResolver = effectResolver;

        public override UniTask Execute(DamageStep step, AbilityExecutionRuntime runtime)
        {
            Debug.Log("DamageStepExecutor");
            
            var target = ResolveTarget(step.TargetMode, runtime);
        
            _effectResolver.ApplyEffect(target, step.Effect);

            // var request = new DamageRequest(
            //     runtime.Context.Source,
            //     target,
            //     step.BasePower,
            //     step.DamageType);

            //var damageResult = runtime.Context.Services.DamageService.ApplyDamage(request);

            //runtime.State.LastDamageResult = damageResult;
            // runtime.Result.AddEvent(new DamageAppliedBattleEvent(damageResult));

            return UniTask.CompletedTask;
        }

        private Unit ResolveTarget(TargetMode selector, AbilityExecutionRuntime runtime)
        {
            return selector switch
            {
                TargetMode.Single => runtime.Context.Target,
                _ => throw new NotSupportedException($"Unsupported target selector: {selector}")
            };
        }
    }
}