using System;
using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Gameplay;
using Armaments.Movers;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts.Executors.Gameplay
{
    public class ArmamentMoverExecutor : AbilityStepExecutor<ArmamentMoverStep>
    {
        public override UniTask Execute(ArmamentMoverStep step, AbilityExecutionRuntime runtime, CancellationToken ct)
        {
            var state = runtime.State;
            
            if (state.Movers.Count == 0)
                throw new NullReferenceException();
            
            IArmamentMover armamentMover = state.Movers[0];
            state.Movers.Remove(armamentMover);
            
            armamentMover.Move();
            
            return UniTask.CompletedTask; //Тут ретернить в момент долета? 
        }
    }
}