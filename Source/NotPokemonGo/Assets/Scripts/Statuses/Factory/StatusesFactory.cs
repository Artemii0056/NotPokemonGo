using System.Collections.Generic;
using System.Linq;
using Effects;
using Units;

namespace Statuses.Factory
{
    public class StatusesFactory : IStatusesFactory
    {
        private readonly IStatusFactory _statusFactory;
        private readonly IEffectResolver _effectResolver;

        public StatusesFactory(IStatusFactory statusFactory, IEffectResolver effectResolver)
        {
            _statusFactory = statusFactory;
            _effectResolver = effectResolver;
        }

        public List<Status> Create(IEnumerable<StatusSetup> setups, Unit source, Unit target) =>
            setups.Select(s => _statusFactory.Create(s, source, target, _effectResolver)).ToList();
    }
}