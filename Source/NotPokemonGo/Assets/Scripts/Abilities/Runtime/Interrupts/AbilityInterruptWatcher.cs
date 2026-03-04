using System;
using System.Collections.Generic;
using Statuses;
using Units;

namespace Abilities.Runtime.Interrupts
{
    /// <summary>
    /// Observes Unit.StatusAdded and raises Interrupted when an interrupting status is applied.
    /// This keeps interrupt logic out of ability steps/policies.
    /// </summary>
    public sealed class AbilityInterruptWatcher : IDisposable
    {
        private readonly Unit _unit;
        private readonly HashSet<StatusType> _interruptStatuses;

        public event Action Interrupted;

        public AbilityInterruptWatcher(Unit unit, IEnumerable<StatusType> interruptStatuses)
        {
            _unit = unit ?? throw new ArgumentNullException(nameof(unit));
            _interruptStatuses = new HashSet<StatusType>(interruptStatuses ?? Array.Empty<StatusType>());

            _unit.StatusAdded += OnStatusAdded;
        }

        private void OnStatusAdded(Status status)
        {
            if (status?.Setup == null)
                return;

            if (_interruptStatuses.Contains(status.Setup.Type))
                Interrupted?.Invoke();
        }

        public void Dispose()
        {
            _unit.StatusAdded -= OnStatusAdded;
        }
    }
}
