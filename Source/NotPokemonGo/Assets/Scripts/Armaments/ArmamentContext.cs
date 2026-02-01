using Units;

namespace Armaments
{
    public sealed class ArmamentContext
    {
        public Unit Source { get; }
        public Unit Target { get; }
        public ArmamentSetup Setup { get; }
        public ArmamentFlyingType FlyingType { get; }

        public ArmamentContext? Parent { get; }

        public ArmamentContext(
            Unit source,
            Unit target,
            ArmamentSetup setup,
            ArmamentFlyingType flyingType,
            ArmamentContext parent = null)
        {
            Source = source;
            Target = target;
            Setup = setup;
            FlyingType = flyingType;
            Parent = parent;
        }

        public ArmamentContext CreateReflected(Unit newTarget)
        {
            return new ArmamentContext(
                source: Target,
                target: newTarget,
                setup: Setup,
                flyingType: ArmamentFlyingType.Direct,
                parent: this);
        }
    }
}