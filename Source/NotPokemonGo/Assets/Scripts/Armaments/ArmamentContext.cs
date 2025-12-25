using Units;

namespace Armaments
{
    public class ArmamentContext
    {
        public readonly Unit Source;
        public readonly Unit Target;
        public readonly ArmamentSetup Setup;
        public readonly ArmamentFlyingType Flying;

        public ArmamentContext(Unit source, Unit target, ArmamentSetup setup, ArmamentFlyingType flying)
        {
            Source = source;
            Target = target;
            Setup = setup;
            Flying = flying;
        }
    }
}