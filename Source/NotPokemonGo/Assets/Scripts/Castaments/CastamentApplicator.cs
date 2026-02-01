using System.Collections.Generic;
using Armaments;
using Effects;
using Effects.Factory;
using Statuses;
using Statuses.Factory;
using Units;

namespace Castaments
{
    public class CastamentApplicator : ICastamentApplicator
    {
        private readonly IEffectInfoFactory _effectInfoFactory;
        private readonly IStatusesFactory   _statusesFactory;
        private readonly IEffectsApplier _effectsApplier;

        public CastamentApplicator( 
            IEffectInfoFactory effectInfoFactory,
            IStatusesFactory statusesFactory, 
            IEffectsApplier effectsApplier)
        {
            _effectInfoFactory = effectInfoFactory;
            _statusesFactory = statusesFactory;
            _effectsApplier = effectsApplier;
        }

        public void Apply(CastamentSetup setup, Unit source, params Unit[] targets)
        {
            foreach (var target in targets)
            {
                List<EffectInfo> effects = _effectInfoFactory.Create(setup.EffectsSetup);
                List<Status> statuses = _statusesFactory.Create(setup.Statuses, source, target);

                _effectsApplier.ApplyEffectsOnTarget(source, target, statuses, effects);
            }
        }
    }
}