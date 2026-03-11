using System;
using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Configs;
using Armaments.Movers;
using Cysharp.Threading.Tasks;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts.Executors
{
    public class ArmamentMoverExecutor : IAbilityStepExecutor
    {
        public UniTask Execute(AbilityStepSO step, AbilityContext ctx)
        {
            if (ctx.Movers.Count == 0)
                throw new NullReferenceException();
            
            IArmamentMover armamentMover = ctx.Movers[0];
            ctx.Movers.Remove(armamentMover);
            
            armamentMover.Move();
            
            return UniTask.CompletedTask; //Тут ретернить в момент долета? 
        }
    }
}