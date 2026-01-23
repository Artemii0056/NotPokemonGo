using System;
using System.Collections.Generic;
using Armaments;

namespace Abilities.Runtime
{
    public sealed class ShotTracker : IDisposable
    {
        private sealed class Entry
        {
            public Shot Shot;
            public Action<Shot> OnLaunched;
            public Action<Shot> OnReached;
        }

        private readonly Dictionary<IArmamentMover, Entry> _entries = new();

        public int ActiveCount => _entries.Count;

        public void Register(Shot shot, Action<Shot> onLaunched, Action<Shot> onReached)
        {
            if (shot == null) 
                throw new ArgumentNullException(nameof(shot));
            
            if (shot.Mover == null) 
                throw new ArgumentException("Shot.Mover is null", nameof(shot));

            var mover = shot.Mover;

            if (_entries.ContainsKey(mover))
                throw new InvalidOperationException("ShotTracker: mover already registered.");

            _entries[mover] = new Entry
            {
                Shot = shot,
                OnLaunched = onLaunched,
                OnReached = onReached
            };

            mover.Launched += HandleLaunched;
            mover.Reached  += HandleReached;
        }

        private void HandleLaunched(IArmamentMover mover)
        {
            if (mover == null) 
                return;

            if (_entries.TryGetValue(mover, out var entry))
                entry.OnLaunched?.Invoke(entry.Shot);
        }

        private void HandleReached(IArmamentMover mover)
        {
            if (mover == null) 
                return;

            if (!_entries.TryGetValue(mover, out var entry))
                return;

            Release(mover);

            entry.OnReached?.Invoke(entry.Shot);
        }

        public void Release(Shot shot)
        {
            if (shot?.Mover == null) 
                return;
            
            Release(shot.Mover);
        }

        public void Release(IArmamentMover mover)
        {
            if (mover == null) 
                return;

            if (!_entries.Remove(mover))
                return;

            mover.Launched -= HandleLaunched;
            mover.Reached  -= HandleReached;
        }

        public void CleanupAll()
        {
            foreach (var mover in new List<IArmamentMover>(_entries.Keys))
                Release(mover);

            _entries.Clear();
        }

        public void Dispose() => CleanupAll();
    }
}
