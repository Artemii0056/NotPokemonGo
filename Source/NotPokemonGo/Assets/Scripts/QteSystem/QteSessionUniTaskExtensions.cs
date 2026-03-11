using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using QteSystem.TestQte;

namespace QteSystem
{
    public static class QteSessionUniTaskExtensions
    {
        public static async UniTask<QteResult> WaitResultAsync(
            this IQteSession session,
            float durationSeconds,
            CancellationToken token)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));

            if (session.IsCompleted && session.Result.HasValue)
                return session.Result.Value;

            var tcs = new UniTaskCompletionSource<QteResult>();

            void OnCompleted(QteResult r) => tcs.TrySetResult(r);
            session.Completed += OnCompleted;

            try
            {
                var timeoutTask = UniTask.Delay(TimeSpan.FromSeconds(durationSeconds), cancellationToken: token);

                // Ключевой трюк: WhenAny на НЕ-дженерик UniTask -> всегда возвращает int
                int winIndex = await UniTask.WhenAny(tcs.Task.AsUniTask(), timeoutTask);

                if (winIndex == 0)
                    return await tcs.Task;

                return QteResult.Fail;
            }
            finally
            {
                session.Completed -= OnCompleted;
            }
        }
    }
}