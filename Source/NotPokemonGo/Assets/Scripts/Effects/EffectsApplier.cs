using System.Collections.Generic;
using Statuses;
using Statuses.Services;
using Units;

namespace Effects
{
    public class EffectsApplier : IEffectsApplier 
    {
        private readonly IEffectResolver _effectResolver;
        private readonly IStatusResolver _statusResolver;

        public EffectsApplier(
            IEffectResolver effectResolver,
            IStatusResolver statusResolver)
        {
            _effectResolver = effectResolver;
            _statusResolver = statusResolver;
        }

        public void ApplyEffectsOnTarget(Unit source, Unit target, List<Status> statuses, List<EffectInfo> effects)
        {
            foreach (var status in statuses)
                _statusResolver.Resolve(status, target);

            foreach (var effectInfo in effects)
                _effectResolver.ApplyEffect(source, target, effectInfo);
        }
    }
}