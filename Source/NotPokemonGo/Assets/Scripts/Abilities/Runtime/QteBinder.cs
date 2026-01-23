using System.Collections.Generic;
using QTESystem;
using QTESystem.TestQTE;
using Units;

namespace Abilities.Runtime
{
    public enum QteTimeoutPolicy
    {
        TreatAsFail,
        TreatAsNormal,
        Ignore
    }

    public sealed class QteBinder
    {
        private sealed class Entry
        {
            public IQteSession Session;
            public QteTimeoutPolicy TimeoutPolicy;
        }

        private readonly IQteService _qteService;
        private readonly Dictionary<Shot, Entry> _active = new();

        public int ActiveCount => _active.Count;

        public QteBinder(IQteService qteService) => 
            _qteService = qteService;

        public void BindOnLaunch(Shot shot, Unit qteTarget, QteTimeoutPolicy timeoutPolicy)
        {
            if (shot == null) 
                return;
            
            if (!shot.RequiresQte) 
                return;

            if (_active.ContainsKey(shot))
                return;

            var session = _qteService.StartSession(shot.Phase.QteType, qteTarget, shot.Mover.Duration);

            session.Completed += r => shot.QteResult = r;

            _active[shot] = new Entry
            {
                Session = session,
                TimeoutPolicy = timeoutPolicy
            };
        }

        public void UnbindOnReach(Shot shot)
        {
            if (shot == null)
                return;

            if (!_active.TryGetValue(shot, out var entry))
                return;

            if (shot.QteResult.HasValue == false)
            {
                switch (entry.TimeoutPolicy)
                {
                    case QteTimeoutPolicy.TreatAsFail:
                        shot.QteResult = QteResult.Fail;
                        break;
                    case QteTimeoutPolicy.TreatAsNormal:
                        shot.QteResult = QteResult.Normal;
                        break;
                    case QteTimeoutPolicy.Ignore:
                        break;
                }
            }

            entry.Session.Dispose();

            _active.Remove(shot);
        }

        public void CleanupAll()
        {
            foreach (var kv in _active)
                kv.Value.Session.Dispose();

            _active.Clear();
        }
    }
}
