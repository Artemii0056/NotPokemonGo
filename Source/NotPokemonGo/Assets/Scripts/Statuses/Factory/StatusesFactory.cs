using System.Collections.Generic;
using System.Linq;
using Units;

namespace Statuses.Factory
{
    public class StatusesFactory : IStatusesFactory
    {
        private readonly IStatusFactory _statusFactory;

        public StatusesFactory(IStatusFactory statusFactory)
        {
            _statusFactory = statusFactory;
        }

        public List<Status> Create(IEnumerable<StatusSetup> setups, Unit target) =>
            setups.Select(s => _statusFactory.Create(s, target)).ToList();
    }
}