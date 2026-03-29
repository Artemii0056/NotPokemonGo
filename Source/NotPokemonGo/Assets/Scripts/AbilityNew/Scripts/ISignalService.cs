using System.Threading;
using Abilities.Signals;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts
{
    public interface ISignalService
    {
        UniTask WaitAsync(PhaseSignal signal, CancellationToken ct);
        void Emit(PhaseSignal signal);
        void Reset();
    }
}