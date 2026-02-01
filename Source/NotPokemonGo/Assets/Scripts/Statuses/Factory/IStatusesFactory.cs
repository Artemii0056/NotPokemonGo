using System.Collections.Generic;
using Units;

namespace Statuses.Factory
{
    public interface IStatusesFactory
    {
        List<Status> Create(IEnumerable<StatusSetup> setups, Unit source, Unit target);
    }
}