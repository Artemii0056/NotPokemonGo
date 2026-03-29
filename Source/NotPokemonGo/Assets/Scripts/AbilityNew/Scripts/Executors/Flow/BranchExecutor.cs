using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Flow;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts.Executors.Flow
{
    public class BranchExecutor : AbilityStepExecutor<BranchStep>
    {
        private readonly StepExecutorRegistry _registry;

        public BranchExecutor(StepExecutorRegistry registry)
        {
            _registry = registry;
        }

        public override async UniTask Execute(BranchStep step, AbilityExecutionRuntime runtime, CancellationToken ct)
        {
            if (step == null)
                return;

            ct.ThrowIfCancellationRequested();

            if (runtime.State.IsInterrupted || runtime.State.IsCancelled)
                return;

            if (step.Cases != null)
            {
                foreach (var branchCase in step.Cases)
                {
                    ct.ThrowIfCancellationRequested();

                    if (runtime.State.IsInterrupted || runtime.State.IsCancelled)
                        return;

                    if (branchCase == null)
                        continue;

                    if (branchCase.Condition == null || branchCase.Step == null)
                        continue;

                    if (branchCase.Condition.Evaluate(runtime))
                    {
                        await _registry.Execute(branchCase.Step, runtime);
                        return;
                    }
                }
            }

            if (step.ElseStep != null)
                await _registry.Execute(step.ElseStep, runtime);
        }
    }
}