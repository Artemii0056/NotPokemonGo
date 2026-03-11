using Units;

namespace Abilities.Flow.Steps
{
    public interface IShieldApplier
    {
        void ApplyShield(Unit unit);
    }

    public interface IShieldStatusReader
    {
        bool HasShield(Unit unit);
    }
}
