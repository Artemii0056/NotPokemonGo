using System;
using Stats;
using Units;

namespace Effects
{
    public interface IEffectResolver
    {
        void ApplyEffect(Unit source, Unit target, EffectInfo effect);
        void ApplyEffect(Unit target, EffectInfo effect);
        public event Action<Unit, Unit, float, EffectInfo> EffectApllayed;
    }
}