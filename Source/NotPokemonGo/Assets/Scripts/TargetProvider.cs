using Abilities;
using Units;

public class TargetProvider : ITargetProvider
{
    public Unit Unit { get; private set; }
        
    public void Remember(Unit unit, TargetMode abilityModelTargetMode)
    {
        Unit = unit; //TODO И тут за счет таргет мода получить все цели. 
    }
}