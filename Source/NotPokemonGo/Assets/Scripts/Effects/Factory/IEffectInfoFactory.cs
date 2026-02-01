using System.Collections.Generic;

namespace Effects.Factory
{
    public interface IEffectInfoFactory
    {
        List<EffectInfo> Create(List<EffectSetup> effects);
    }
}