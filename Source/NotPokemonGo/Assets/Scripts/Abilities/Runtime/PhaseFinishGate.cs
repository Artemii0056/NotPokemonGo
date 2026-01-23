using System;

namespace Abilities.Runtime
{
    public sealed class PhaseFinishGate
    {
        private int _counter;
        private Action _tryCompleteFinish;

        public void Reset(Action tryCompleteFinish)
        {
            _counter = 0;
            _tryCompleteFinish = tryCompleteFinish;
        }

        public IDisposable Acquire()
        {
            _counter++;
            return new Releaser(this);
        }

        public bool IsOpen => _counter == 0;

        private void Release()
        {
            _counter--;

            if (_counter < 0)
                _counter = 0;

            if (_counter == 0)
                _tryCompleteFinish?.Invoke();
        }

        private sealed class Releaser : IDisposable
        {
            private PhaseFinishGate _gate;

            public Releaser(PhaseFinishGate gate) => 
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