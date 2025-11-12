using System;

namespace Abilities.Bennet
{
    public interface IAbilityHandler
    {
        event Action<IAbilityHandler> Finished;
        void Play();
        void Stop();
    }
}