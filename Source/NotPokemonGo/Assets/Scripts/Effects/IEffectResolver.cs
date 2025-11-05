using System;
using Units;

namespace Effects
{
    public interface IEffectResolver
    {
        void ApplyEffect(Unit target, EffectInfo effect);
        event Action<EffectType, Unit, float> EffectOnTargetCompleted;
    }
}