using System.Collections.Generic;
using System.Threading;
using Abilities.Signals;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts
{
    public sealed class SignalService : ISignalService
    {
        private readonly Dictionary<PhaseSignal, int> _bufferedSignals = new();
        private readonly Dictionary<PhaseSignal, Queue<UniTaskCompletionSource>> _waiters = new();

        public UniTask WaitAsync(PhaseSignal signal, CancellationToken ct)
        {
            if (TryConsumeBuffered(signal))
                return UniTask.CompletedTask;

            var tcs = new UniTaskCompletionSource();

            if (_waiters.TryGetValue(signal, out Queue<UniTaskCompletionSource> queue) == false)
            {
                queue = new Queue<UniTaskCompletionSource>();
                _waiters[signal] = queue;
            }

            queue.Enqueue(tcs);

            if (ct.CanBeCanceled)
            {
                ct.Register(() =>
                {
                    RemoveWaiter(signal, tcs);
                    tcs.TrySetCanceled(ct);
                });
            }

            return tcs.Task;
        }

        public void Emit(PhaseSignal signal)
        {
            if (_waiters.TryGetValue(signal, out Queue<UniTaskCompletionSource> queue))
            {
                while (queue.Count > 0)
                {
                    UniTaskCompletionSource waiter = queue.Dequeue();

                    if (waiter.TrySetResult())
                    {
                        CleanupQueueIfEmpty(signal, queue);
                        return;
                    }
                }

                CleanupQueueIfEmpty(signal, queue);
            }

            if (_bufferedSignals.TryGetValue(signal, out int count))
                _bufferedSignals[signal] = count + 1;
            else
                _bufferedSignals[signal] = 1;
        }

        public void Reset()
        {
            foreach (Queue<UniTaskCompletionSource> queue in _waiters.Values)
            {
                while (queue.Count > 0)
                {
                    UniTaskCompletionSource waiter = queue.Dequeue();
                    waiter.TrySetCanceled();
                }
            }

            _waiters.Clear();
            _bufferedSignals.Clear();
        }

        private bool TryConsumeBuffered(PhaseSignal signal)
        {
            if (_bufferedSignals.TryGetValue(signal, out int count) == false || count <= 0)
                return false;

            if (count == 1)
                _bufferedSignals.Remove(signal);
            else
                _bufferedSignals[signal] = count - 1;

            return true;
        }

        private void RemoveWaiter(PhaseSignal signal, UniTaskCompletionSource target)
        {
            if (_waiters.TryGetValue(signal, out Queue<UniTaskCompletionSource> queue) == false)
                return;

            if (queue.Count == 0)
            {
                _waiters.Remove(signal);
                return;
            }

            int count = queue.Count;
            var rebuilt = new Queue<UniTaskCompletionSource>(count);

            for (int i = 0; i < count; i++)
            {
                UniTaskCompletionSource current = queue.Dequeue();

                if (ReferenceEquals(current, target) == false)
                    rebuilt.Enqueue(current);
            }

            if (rebuilt.Count == 0)
                _waiters.Remove(signal);
            else
                _waiters[signal] = rebuilt;
        }

        private void CleanupQueueIfEmpty(PhaseSignal signal, Queue<UniTaskCompletionSource> queue)
        {
            if (queue.Count == 0)
                _waiters.Remove(signal);
        }
    }
}