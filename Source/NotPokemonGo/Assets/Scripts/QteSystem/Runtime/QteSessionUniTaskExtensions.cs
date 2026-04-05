using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using QteSystem.Core;

namespace QteSystem.Runtime
{
    public static class QteSessionUniTaskExtensions
    {
        public static async UniTask<QteResult> WaitResultAsync(
            this IQteSession session,
            float durationSeconds,
            CancellationToken token)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));

            if (session.IsCompleted && session.Result.HasValue)
                return session.Result.Value;

            var tcs = new UniTaskCompletionSource<QteResult>();

            void OnCompleted(QteResult result)
            {
                tcs.TrySetResult(result);
            }

            session.Completed += OnCompleted;

            try
            {
                int winIndex = await UniTask.WhenAny(
                    tcs.Task.AsUniTask(),
                    UniTask.Delay(TimeSpan.FromSeconds(durationSeconds), cancellationToken: token));

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