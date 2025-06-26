using Abilities;
using Units;

public interface ITargetProvider
{
    Unit Unit { get; }
    void Remember(Unit unit, TargetMode abilityModelTargetMode);
}