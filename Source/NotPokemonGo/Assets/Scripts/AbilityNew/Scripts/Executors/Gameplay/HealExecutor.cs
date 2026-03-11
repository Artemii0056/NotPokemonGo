using System;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Gameplay;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts.Executors.Gameplay
{
    public class HealExecutor : AbilityStepExecutor<HealStep>
    {
        public override UniTask Execute(HealStep step, AbilityExecutionRuntime runtime)
        {
            throw new NotImplementedException();
        }
    }
}