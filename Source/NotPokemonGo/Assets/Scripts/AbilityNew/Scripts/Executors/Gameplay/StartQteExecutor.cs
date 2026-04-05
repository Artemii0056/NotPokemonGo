using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Gameplay;
using Cysharp.Threading.Tasks;
using QteSystem;
using QteSystem.Core;

namespace AbilityNew.Scripts.Executors.Gameplay
{
    public sealed class StartQteExecutor : AbilityStepExecutor<StartQteStep>
    {
        private readonly IQteService _qteService;

        public StartQteExecutor(IQteService qteService)
        {
            _qteService = qteService;
        }

        public override UniTask Execute(
            StartQteStep step,
            AbilityExecutionRuntime runtime,
            CancellationToken ct)
        {
            var state = runtime.State;

            if (state.ActiveQte != null)
            {
                state.ActiveQte.Dispose();
                state.ActiveQte = null;
            }

            state.LastQteResult = null;

            var request = new QteRequest(
                step.Type,
                runtime.Context.Target,
                step.Duration,
                step.OutcomeMode);

            state.ActiveQte = _qteService.StartSession(request);

            return UniTask.CompletedTask;
        }
    }
}