using Units;

namespace Armaments
{
    public class ArmamentContext
    {
        public readonly Unit Source;
        public readonly Unit Target;
        public readonly ArmamentSetup Setup;

        public ArmamentContext(Unit source, Unit target, ArmamentSetup setup)
        {
            Source = source;
            Target = target;
            Setup = setup;
        }
    }
}