using Services.StaticDataServices;
using Statuses;
using Statuses.Services;
using Units;

namespace Abilities.Flow.Steps
{
    public sealed class ShieldStatusApplier : IShieldApplier
    {
        private readonly IStaticDataService _staticData;
        private readonly IStatusFactory _statusFactory;
        private readonly IStatusResolver _statusResolver;

        public ShieldStatusApplier(IStaticDataService staticData, IStatusFactory statusFactory, IStatusResolver statusResolver)
        {
            _staticData = staticData;
            _statusFactory = statusFactory;
            _statusResolver = statusResolver;
        }

        public void ApplyShield(Unit unit)
        {
            if (unit == null) return;

            StatusSetup setup = _staticData.GetStatusSetup(StatusType.Bubble);
            var status = _statusFactory.Create(setup, unit);
            _statusResolver.Resolve(status, unit);
        }
    }

    public sealed class ShieldStatusReader : IShieldStatusReader
    {
        public bool HasShield(Unit unit) => unit != null && unit.HasStatus(StatusType.Bubble);
    }
}
