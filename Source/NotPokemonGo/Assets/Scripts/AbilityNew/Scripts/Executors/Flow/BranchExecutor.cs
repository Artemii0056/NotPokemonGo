using System;
using Abilities.Flow;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts.Executors.Flow
{
    public class BranchExecutor : AbilityStepExecutor<BranchStep>
    {
        public override UniTask Execute(BranchStep step, AbilityExecutionRuntime runtime)
        {
            throw new NotImplementedException();
        }
    }
}