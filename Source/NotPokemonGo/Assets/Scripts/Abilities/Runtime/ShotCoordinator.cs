using System;
using System.Collections.Generic;
using Armaments.Movers;

namespace Abilities.Runtime
{
    public class ShotCoordinator
    {
         private readonly List<IArmamentMover> _movers = new();

        public int ActiveCount => _movers.Count;
        
        private Action<IArmamentMover> _onShotReached;

        public void Register(IArmamentMover mover, Action<IArmamentMover> onReached)
        {
            _onShotReached = onReached;
            
            if (mover == null) 
                throw new ArgumentException("Shot.Mover is null", nameof(mover));

            if (_movers.Contains(mover))
                throw new InvalidOperationException("ShotTracker: mover already registered.");
            
            _movers.Add(mover);

            mover.Reached  += HandleReached;
        }

        private void HandleReached(IArmamentMover mover)
        {
            if (mover == null) 
                return;

            if (_movers.Contains(mover) == false)
                return;
            
            Release(mover);
            _onShotReached?.Invoke(mover);
        }

        public void Dispose() => 
            CleanupAll();
        
        private void Release(IArmamentMover mover)
        {
            if (mover == null) 
                return;

            if (_movers.Contains(mover) == false)
                return;
            
            _movers.Remove(mover);

            mover.Reached  -= HandleReached;
        }

        public void CleanupAll()
        {
            foreach (var mover in _movers.ToArray())
                Release(mover);

            _movers.Clear();
        }
    }
}