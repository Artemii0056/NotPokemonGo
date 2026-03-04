using System;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;

namespace Abilities.Core
{
    /// <summary>
    /// Owns all runtime resources created by a single ability execution:
    /// - CancellationToken
    /// - event subscriptions / IDisposable cleanups
    /// - DOTween ownership (Kill by scope id)
    /// </summary>
    public sealed class AbilityRunScope : IDisposable
    {
        private static int _nextId;

        private readonly List<IDisposable> _disposables = new(16);
        private readonly CancellationTokenSource _cts = new();
        private bool _disposed;

        public AbilityRunScope() =>
            Id = unchecked(++_nextId);

        /// <summary>Unique scope id. Used for DOTween SetId / Kill.</summary>
        public int Id { get; }

        public CancellationToken Token => _cts.Token;

        public void Add(IDisposable disposable)
        {
            if (disposable == null)
                return;

            if (_disposed)
            {
                disposable.Dispose();
                return;
            }

            _disposables.Add(disposable);
        }

        public void Add(Action unsubscribe)
        {
            if (unsubscribe == null)
                return;

            Add(new ActionDisposable(unsubscribe));
        }

        /// <summary>
        /// Applies DOTween ownership to the scope.
        /// </summary>
        public T OwnTween<T>(T tween) where T : Tween
        {
            if (tween == null)
                return tween;

            tween.SetId(Id);
            return tween;
        }

        public void Cancel()
        {
            if (_disposed)
                return;

            _cts.Cancel();
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            try { _cts.Cancel(); } catch { /* ignore */ }

            try { DOTween.Kill(Id); } catch { /* ignore */ }

            for (int i = _disposables.Count - 1; i >= 0; i--)
            {
                try { _disposables[i]?.Dispose(); } catch { /* ignore */ }
            }

            _disposables.Clear();
            _cts.Dispose();
        }

        private sealed class ActionDisposable : IDisposable
        {
            private Action _action;
            public ActionDisposable(Action action) => _action = action;

            public void Dispose()
            {
                var a = _action;
                if (a == null)
                    return;

                _action = null;
                a.Invoke();
            }
        }
    }
}
