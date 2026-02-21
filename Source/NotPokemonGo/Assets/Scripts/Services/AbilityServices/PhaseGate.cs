using System;
using System.Collections.Generic;

namespace Services.AbilityServices
{
    public sealed class PhaseGate
    {
        private readonly Dictionary<int, string> _holders;
        private int _nextId;
        
        private int _version;

        private int _pending;

        public PhaseGate() =>
            _holders = new();

        public event Action Opened; 

        public IDisposable Acquire(string tag = null)
        {
            int id = ++_nextId;

            _pending++;
            _holders[id] = tag ?? "no-tag";

            return new Token(this, id);
        }

        public void Reset()
        {
            _version++;
            _pending = 0;
            _holders.Clear();
        }

        public void Release(int id)
        {
            if (_holders.Remove(id) == false)
                return; 
            
            _pending--;

            if (_pending == 0)
                Opened?.Invoke();
        }

        public void Dispose() =>
            _holders.Clear();
    }

    sealed class Token : IDisposable
    {
        private readonly PhaseGate _gate;
        private readonly int _id;
        private bool _released;
        
        private readonly int _version;

        public Token(PhaseGate gate, int id)
        {
            _gate = gate;
            _id = id;
        }

        public void Dispose()
        {
            if (_released)
                return;

            _released = true;
            _gate.Release(_id);
        }
    }
}