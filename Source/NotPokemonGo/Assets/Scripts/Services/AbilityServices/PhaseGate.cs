using System;

namespace Services.AbilityServices
{
    public sealed class PhaseGate
    {
        private int _pending;
        private int _version;

        public bool IsOpen => _pending == 0;

        public IDisposable Acquire()
        {
            _pending++;
            return new Releaser(this, _version);
        }

        public void Reset()
        {
            _pending = 0;
            _version++; 
        }

        private void Release(int tokenVersion)
        {
            if (tokenVersion != _version)
                return;

            _pending--;
            if (_pending < 0) _pending = 0;
        }

        private sealed class Releaser : IDisposable
        {
            private PhaseGate _gate;
            private readonly int _version;

            public Releaser(PhaseGate gate, int version)
            {
                _gate = gate;
                _version = version;
            }

            public void Dispose()
            {
                if (_gate == null) 
                    return;
                
                _gate.Release(_version);
                _gate = null;
            }
        }
    }
}