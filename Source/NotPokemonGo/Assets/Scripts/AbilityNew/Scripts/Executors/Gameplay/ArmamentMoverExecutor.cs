using System;
using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Gameplay;
using Armaments.Movers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AbilityNew.Scripts.Executors.Gameplay
{
    public class ArmamentMoverExecutor : AbilityStepExecutor<ArmamentMoverStep>
    {
        public override UniTask Execute(ArmamentMoverStep step, AbilityExecutionRuntime runtime, CancellationToken ct)
        {
            var state = runtime.State;
            
            if (state.Movers.Count == 0)
                throw new NullReferenceException();
            
            IArmamentMover mover = state.Movers[0];
            state.Movers.Remove(mover);
            
            runtime.State.AbilityBlackboard.Set(BlackboardKey.CurrentArmament, mover.Armament);
            
            mover.Move();
            
            runtime.State.AbilityBlackboard.Set(BlackboardKey.ProjectileFlightTime, mover.Duration);
            Debug.Log($"[ArmamentMover] Flight started. Duration = {mover.Duration}");
            
            return UniTask.CompletedTask;
        }
    }
}