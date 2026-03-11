using System.Collections.Generic;
using Abilities.Signals;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts
{
    public class SignalService
    {
        private Dictionary<PhaseSignal, UniTaskCompletionSource> _signals = new();

        public UniTask Wait(PhaseSignal signal)
        {
            var tcs = new UniTaskCompletionSource();
            _signals[signal] = tcs;

            return tcs.Task;
        }

        public void Emit(PhaseSignal signal)
        {
            if (_signals.TryGetValue(signal, out var tcs))
            {
                tcs.TrySetResult();
                _signals.Remove(signal);
            }
        }
    }
}