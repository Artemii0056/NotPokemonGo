using System.Threading;
using Cysharp.Threading.Tasks;
using QteSystem;
using QteSystem.TestQte;

namespace Abilities.Flow
{
    public sealed class QteStep : IAbilityStep
    {
        private readonly QteType _type;
        private readonly float _duration;

        public QteStep(QteType type, float duration)
        {
            _type = type;
            _duration = duration;
        }

        public async UniTask Execute(AbilityExecutionContext ctx, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            IQteSession session = null;

            try
            {
                session = ctx.QteService.StartSession(_type, ctx.Target, _duration);

                QteResult result = await session.WaitResultAsync(_duration, token);

                ctx.QteResult = result;
            }
            finally
            {
                session?.Dispose();
            }
        }
    }
}