using System;

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

        public IDisposable Acquire()
        {
            _tokens++;
            return new Token(this);
        }

        public void RequestFinish() => 
            _finishRequested = true;

        private void Release()
        {
            if (_tokens > 0)
                _tokens--;
        }

        private sealed class Token : IDisposable
        {
            private PhaseFinishGate _gate;

            public Token(PhaseFinishGate gate) => 
                _gate = gate;

            public void Dispose()
            {
                if (_gate == null) 
                    return;
                
                _gate.Release();
                _gate = null;
            }
        }
    }
}