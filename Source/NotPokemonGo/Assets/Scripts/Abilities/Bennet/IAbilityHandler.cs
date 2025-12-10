using System;
using Abilities.Configs;
using Units;

namespace Abilities.Bennet
{
    public interface IAbilityHandler
    {
        event Action<IAbilityHandler> Finished;
        void Play(Unit source, Unit target);
        void Stop();
        Interruptibility Interruptibility { get;  }
    }
}