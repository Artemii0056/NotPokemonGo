using System.Collections.Generic;
using System.Linq;

namespace Effects.Factory
{
    public class EffectInfoFactory : IEffectInfoFactory
    {
        public List<EffectInfo> Create(List<EffectSetup> effects) =>
            effects.Select(s => new EffectInfo(s.Value, s.TargetType, s.Type, s.DamageType)).ToList();
    }
}