using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.AbilityServices
{
    public sealed class PhaseGate
    {
        private int _pending;
        private int _version;

        public bool IsOpen => _pending == 0;

        // Для быстрой диагностики:
        public int Pending => _pending;
        public int Version => _version;

#if UNITY_EDITOR
        private int _nextId;
        private readonly Dictionary<int, string> _holders = new(); // id -> stack/tag
#endif

        public IDisposable Acquire(string tag = null)
        {
            _pending++;

#if UNITY_EDITOR
            int id = ++_nextId;
            string trace = tag ?? "no-tag";
            trace += "\n" + StackTraceUtility.ExtractStackTrace();
            _holders[id] = trace;
            return new Releaser(this, _version, id);
#else
            return new Releaser(this, _version);
#endif
        }

        public void Reset()
        {
            _pending = 0;
            _version++;

#if UNITY_EDITOR
            _holders.Clear();
#endif
        }

        private void Release(int tokenVersion
#if UNITY_EDITOR
            , int id
#endif
        )
        {
            if (tokenVersion != _version)
                return;

            _pending--;
            if (_pending < 0) _pending = 0;

#if UNITY_EDITOR
            _holders.Remove(id);
#endif
        }

#if UNITY_EDITOR
        public void DumpIfStuck(string prefix)
        {
            if (IsOpen) return;

            Debug.LogError($"{prefix} Gate stuck. Pending={_pending} Version={_version}");

            foreach (var kv in _holders)
                Debug.LogError($"Gate holder #{kv.Key}:\n{kv.Value}");
        }
#endif

        private sealed class Releaser : IDisposable
        {
            private PhaseGate _gate;
            private readonly int _version;

#if UNITY_EDITOR
            private readonly int _id;
            public Releaser(PhaseGate gate, int version, int id)
            {
                _gate = gate;
                _version = version;
                _id = id;
            }
#else
            public Releaser(PhaseGate gate, int version)
            {
                _gate = gate;
                _version = version;
            }
#endif

            public void Dispose()
            {
                if (_gate == null) return;

#if UNITY_EDITOR
                _gate.Release(_version, _id);
#else
                _gate.Release(_version);
#endif
                _gate = null;
            }
        }
    }
}
