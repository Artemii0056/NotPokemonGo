using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Gameplay;
using Cysharp.Threading.Tasks;
using QteSystem;

namespace AbilityNew.Scripts.Executors.Gameplay
{
    public class StartQteExecutor : AbilityStepExecutor<StartQteStep>
    {
        private readonly IQteService _qteService;

        public StartQteExecutor(IQteService qteService) =>
            _qteService = qteService;

        public override UniTask Execute(StartQteStep step, AbilityExecutionRuntime runtime)
        {
            var state = runtime.State;

            if (state.ActiveQte != null)
            {
                state.ActiveQte.Dispose();
                state.ActiveQte = null;
            }

            state.LastQteResult = null;

            state.ActiveQte = _qteService.StartSession(step.Type,
                runtime.Context.Target,
                step.Duration);

            return UniTask.CompletedTask;
        }
    }
}