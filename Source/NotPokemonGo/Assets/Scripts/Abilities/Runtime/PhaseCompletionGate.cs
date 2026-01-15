using System;
using System.Collections.Generic;

namespace Abilities.Runtime
{
    /// <summary>
    /// Барьер завершения фазы: фаза завершена, когда выполнены ВСЕ обязательные ожидания.
    /// Работает через "версию фазы", чтобы события от прошлой фазы не открывали текущую.
    /// </summary>
    public sealed class PhaseCompletionGate
    {
        private int _phaseVersion;

        private readonly HashSet<string> _required = new();
        private readonly HashSet<string> _done = new();

        public int Version => _phaseVersion;

        public void BeginPhase()
        {
            _phaseVersion++;
            _required.Clear();
            _done.Clear();
        }

        /// <summary>Объявить обязательное ожидание. Например: "finish", "camera", "qte", "move".</summary>
        public void Require(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("key is null/empty", nameof(key));

            _required.Add(key);
        }

        /// <summary>Пометить ожидание выполненным.</summary>
        public void Done(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            // done без require допустим: не ломает ничего (удобно для "optional" кирпичиков)
            _done.Add(key);
        }

        public bool IsRequired(string key) => _required.Contains(key);

        public bool IsOpen
        {
            get
            {
                if (_required.Count == 0)
                    return true;

                foreach (var r in _required)
                    if (!_done.Contains(r))
                        return false;

                return true;
            }
        }

        public void ForceOpen()
        {
            foreach (var r in _required)
                _done.Add(r);
        }

        public void Clear()
        {
            _phaseVersion = 0;
            _required.Clear();
            _done.Clear();
        }
    }

    /// <summary>Ключи ожиданий — чтобы не плодить магические строки.</summary>
    public static class PhaseWaitKeys
    {
        public const string Finish = "finish"; // PhaseSignal.Finish
        public const string Move   = "move";   // UnitMover finished
        public const string Camera = "camera"; // Camera action finished
        public const string Qte    = "qte";    // QTE finished
    }
}
