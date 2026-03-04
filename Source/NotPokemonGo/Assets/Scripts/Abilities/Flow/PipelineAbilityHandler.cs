using System;
using System.Threading;
using Abilities.Core;
using Abilities.Bennet;
using Abilities.MV;
using Cysharp.Threading.Tasks;
using Units;

namespace Abilities.Flow
{
    public sealed class PipelineAbilityHandler : IAbilityHandler
    {
        private readonly AbilityPipelineExecutor _executor;
        private readonly AbilityExecutionContext _ctx;

        private AbilityRunScope _scope;

        public PipelineAbilityHandler(AbilityPipelineExecutor executor, AbilityExecutionContext ctx)
        {
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        public event Action<IAbilityHandler> Finished;

        public AbilityModel CurrentAbility { get; private set; }

        public void BindAbility(AbilityModel model) => CurrentAbility = model;

        public void Play(Unit source, Unit target)
        {
            _scope = new AbilityRunScope();
            _ctx.Scope = _scope;
            RunAsync(_scope.Token).Forget();
        }

        public void Stop()
        {
            try { _scope?.Cancel(); } catch { }
            _ctx.Dispose();
        }

        private async UniTaskVoid RunAsync(CancellationToken token)
        {
            try
            {
                await _executor.Execute(_ctx, token);
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
            finally
            {
                try { Finished?.Invoke(this); } catch { }
                _ctx.Dispose();
            }
        }
    }
}
