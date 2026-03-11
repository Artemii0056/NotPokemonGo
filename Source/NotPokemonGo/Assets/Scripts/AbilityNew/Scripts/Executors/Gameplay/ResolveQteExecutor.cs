using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Gameplay;
using Cysharp.Threading.Tasks;
using QteSystem;
using QteSystem.TestQte;

namespace AbilityNew.Scripts.Executors.Gameplay
{
    public class ResolveQteExecutor : AbilityStepExecutor<ResolveQteStep>
    {
        private readonly StepExecutorRegistry _registry;

        public ResolveQteExecutor(StepExecutorRegistry registry) => 
            _registry = registry;

        private UniTask<QteResult> WaitResult(IQteSession session)
        {
            var tcs = new UniTaskCompletionSource<QteResult>();

            session.Completed += result =>
            {
                tcs.TrySetResult(result);
            };

            return tcs.Task;
        }

        public override async UniTask Execute(ResolveQteStep step, AbilityExecutionRuntime runtime)
        {
            AbilityExecutionState state = runtime.State;
            
            if (state.ActiveQte == null)
                return;

            QteResult result = await WaitResult(state.ActiveQte);

            if (result == QteResult.Fail && step.OnFail != null)
                await _registry.Execute(step.OnFail, runtime);

            if (result == QteResult.Normal && step.OnNormal != null)
                await _registry.Execute(step.OnNormal, runtime);

            if (result == QteResult.Perfect && step.OnPerfect != null)
                await _registry.Execute(step.OnPerfect, runtime);

            state.ActiveQte.Dispose();
            state.ActiveQte = null;
        }
    }
}