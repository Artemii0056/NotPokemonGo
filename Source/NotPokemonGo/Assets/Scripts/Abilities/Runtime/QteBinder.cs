using System.Collections.Generic;
using Abilities.General;
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

        public QteBinder(IQteService qteService)
        {
            _qteService = qteService;
        }

        /// <summary>
        /// Вызывается на mover.Launched (когда duration уже известен).
        /// </summary>
        public void BindOnLaunch(Shot shot, Unit qteTarget, QteTimeoutPolicy timeoutPolicy)
        {
            if (shot == null) return;
            if (!shot.RequiresQte) return;

            // не плодим вторые сессии
            if (_active.ContainsKey(shot))
                return;

            var session = _qteService.StartSession(shot.Phase.QteType, qteTarget, shot.Mover.Duration);

            // NullQteSession не стреляет Completed — это ок.
            session.Completed += r => shot.QteResult = r;

            _active[shot] = new Entry
            {
                Session = session,
                TimeoutPolicy = timeoutPolicy
            };
        }

        /// <summary>
        /// Вызывается на mover.Reached. Применяет timeout policy и гарантированно закрывает QTE.
        /// </summary>
        public void UnbindOnReach(Shot shot)
        {
            if (shot == null) return;

            if (!_active.TryGetValue(shot, out var entry))
                return;

            // если QTE не успели завершить — трактуем по политике
            if (!shot.QteResult.HasValue)
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

            // Dispose убьёт UI + отписки
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
