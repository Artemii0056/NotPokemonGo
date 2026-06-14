using System.Collections.Generic;
using Units;

namespace AbilityNew.AbilityDefinition
{
    public sealed class AbilityExecutionContext
    {
        public AbilityExecutionContext(
            Unit source,
            Unit target,
            IReadOnlyList<Unit> allTargets)
        {
            Source = source;
            Target = target;
            AllTargets = allTargets;
        }
        
        public Unit Source { get; }
        public Unit Target { get; }
        public IReadOnlyList<Unit> AllTargets { get; }
    }
}