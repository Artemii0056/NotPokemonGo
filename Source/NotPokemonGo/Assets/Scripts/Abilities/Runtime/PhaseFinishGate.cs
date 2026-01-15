using System;
using System.Collections.Generic;

namespace Abilities.Runtime
{
    public sealed class PhaseFinishGate
    {
        private int _tokens;
        private bool _finishRequested;

        public bool CanFinish => _finishRequested && _tokens <= 0;

        public void Reset()
        {
            _tokens = 0;
            _finishRequested = false;
        }

        public IDisposable Acquire(string tag = null)
        {
            _tokens++;
            return new Token(this);
        }

        public void RequestFinish() => _finishRequested = true;

        private void Release()
        {
            _tokens = Math.Max(0, _tokens - 1);
        }

        private sealed class Token : IDisposable
        {
            private PhaseFinishGate _gate;
            public Token(PhaseFinishGate gate) => _gate = gate;
            public void Dispose()
            {
                if (_gate == null) return;
                _gate.Release();
                _gate = null;
            }
        }
    }
}