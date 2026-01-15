using System;

namespace Services.AbilityServices
{
    public sealed class PhaseGate
    {
        private int _pending;

        public bool IsOpen => _pending <= 0;

        public IDisposable Acquire()
        {
            _pending++;
            return new Releaser(this);
        }

        private void Release()
        {
            _pending--;
            if (_pending < 0) _pending = 0;
        }

        private sealed class Releaser : IDisposable
        {
            private PhaseGate _gate;
            public Releaser(PhaseGate gate) => _gate = gate;

            public void Dispose()
            {
                if (_gate == null) return;
                _gate.Release();
                _gate = null;
            }
        }
    }
}