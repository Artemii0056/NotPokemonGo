using System;
using Stats;
using Units;

namespace Effects
{
    public interface IEffectResolver
    {
        void ApplyEffect(Unit source, Unit target, EffectInfo effect);
        event Action<EffectResolver.EffectDataPayload> EffectApplied;
    }
}