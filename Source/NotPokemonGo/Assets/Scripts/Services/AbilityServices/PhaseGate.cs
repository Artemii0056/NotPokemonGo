using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Services.AbilityServices
{
    public sealed class PhaseGate : IDisposable
    {
        private readonly Dictionary<int, string> _holders = new();

        private int _nextId;
        private int _version;
        private int _pending;

        /// <summary>
        /// DOTween owner id for all tweens created within this phase.
        /// 0 means "no owner".
        /// </summary>
        public int OwnerId { get; private set; }

        public event Action Opened;

        public bool IsOpened => _pending == 0;

        public IDisposable Acquire(string tag = null)
        {
            int id = ++_nextId;

            _pending++;
            _holders[id] = tag ?? "no-tag";

            return new Token(this, id, _version);
        }

        public void Reset() => Reset(0);

        public void Reset(int ownerId)
        {
            _version++;
            _pending = 0;
            _holders.Clear();
            OwnerId = ownerId;
        }

        private void Release(int id, int tokenVersion)
        {
            if (tokenVersion != _version)
                return;

            if (_holders.Remove(id) == false)
                return;

            _pending--;

            if (_pending == 0)
                Opened?.Invoke();
        }

        public string GetDebugSnapshot(int maxEntries = 32)
        {
            var sb = new StringBuilder(256);

            sb.Append("[PhaseGate] ")
              .Append("pending=").Append(_pending)
              .Append(", version=").Append(_version)
              .Append(", holders=").Append(_holders.Count);

            if (_holders.Count == 0)
                return sb.ToString();

            sb.AppendLine();

            int i = 0;
            
            foreach (var kvp in _holders)
            {
                if (i >= maxEntries)
                {
                    sb.Append("... (truncated)").AppendLine();
                    break;
                }

                sb.Append("  #").Append(kvp.Key).Append(" : ").Append(kvp.Value).AppendLine();
                i++;
            }

            return sb.ToString();
        }

        public void DumpToLog(string prefix = null, int maxEntries = 32)
        {
            string snapshot = GetDebugSnapshot(maxEntries);

            if (string.IsNullOrEmpty(prefix))
                Debug.Log(snapshot);
            else
                Debug.Log(prefix + "\n" + snapshot);
        }

        public void Dispose()
        {
            _holders.Clear();
            _pending = 0;
            _version++;
        }

        private sealed class Token : IDisposable
        {
            private readonly PhaseGate _gate;
            private readonly int _id;
            private readonly int _version;

            private bool _released;

            public Token(PhaseGate gate, int id, int version)
            {
                _gate = gate;
                _id = id;
                _version = version;
            }

            public void Dispose()
            {
                if (_released)
                    return;

                _released = true;
                _gate.Release(_id, _version);
            }
        }
    }
}
