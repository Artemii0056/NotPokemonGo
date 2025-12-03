using System.Collections.Generic;
using System.Linq;
using Effects;
using Statuses;
using Statuses.Services;
using Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Castaments
{
    public class CastamentApplicator : ICastamentApplicator
    {
        private readonly IStatusFactory _statusFactory;
        private readonly IEffectResolver _effectResolver;
        private readonly IStatusResolver _statusResolver;

        public CastamentApplicator(
            IStatusFactory statusFactory,
            IEffectResolver effectResolver,
            IStatusResolver statusResolver)
        {
            _statusFactory = statusFactory;
            _effectResolver = effectResolver;
            _statusResolver = statusResolver;
        }

        public void Apply(CastamentSetup setup, Unit source, params Unit[] targets)
        {
            foreach (var target in targets)
            {
                List<EffectInfo> effects = CreateEffects(setup.EffectsSetup);
                List<Status> statuses = CreateStatuses(setup.Statuses, source, target);

                ApplyEffectsOnTarget(source, target, statuses, effects);

                if (setup.ParticleSystem != null)
                {
                    ParticleSystem effect = Object.Instantiate(setup.ParticleSystem);
                    effect.transform.position = target.transform.position;
                    effect.Play();
                }
            }
        }


        private List<EffectInfo> CreateEffects(List<EffectSetup> effects) =>
            effects.Select(s => new EffectInfo(s.Value, s.TargetType, s.Type, s.DamageType)).ToList();

        private List<Status> CreateStatuses(IEnumerable<StatusSetup> setups, Unit source, Unit target) =>
            setups.Select(s => _statusFactory.Create(s, source, target, _effectResolver)).ToList();

        private void ApplyEffectsOnTarget(Unit source, Unit target, List<Status> statuses, List<EffectInfo> effects)
        {
            foreach (var status in statuses)
                _statusResolver.Resolve(status, target);

            foreach (var effectInfo in effects)
                _effectResolver.ApplyEffect(source, target, effectInfo);
        }
    }
}