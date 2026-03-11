using System;
using System.Collections.Generic;
using Armaments.Movers;
using UnityEngine;

namespace Abilities.Runtime
{
    public sealed class ShotCoordinator : IDisposable
    {
        private readonly Dictionary<IArmamentMover, Action<IArmamentMover>> _active =
            new();

        public int ActiveCount => _active.Count;

        public void Register(IArmamentMover mover, Action<IArmamentMover> onReached)
        {
            if (mover == null)
                throw new ArgumentNullException(nameof(mover));

            if (_active.ContainsKey(mover))
                throw new InvalidOperationException("Mover already registered.");

            _active[mover] = onReached;

            mover.Reached += HandleReached;
        }

        private void HandleReached(IArmamentMover mover)
        {
            Debug.Log($"Reached: {_active.Count} before");

            if (!_active.TryGetValue(mover, out var callback))
                return;

            Release(mover);

            Debug.Log($"Reached: {_active.Count} after");

            callback?.Invoke(mover);
        }

        private void Release(IArmamentMover mover)
        {
            mover.Reached -= HandleReached;
            _active.Remove(mover);
        }

        public void CleanupAll()
        {
            foreach (var kv in _active)
                kv.Key.Reached -= HandleReached;

            _active.Clear();
        }

        public void Dispose() => 
            CleanupAll();
    }
}