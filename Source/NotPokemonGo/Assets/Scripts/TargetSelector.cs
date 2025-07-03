using System;
using System.Collections.Generic;
using Abilities;
using Platoons;
using Units;

public class TargetSelector : ITargetSelector
{
    private Unit _target;

    private List<Platoon> _platoons = new();

    public void SetPlatoons(Platoon platoon, Platoon platoon2)
    {
        _platoons.Add(platoon);
        _platoons.Add(platoon2);
    }

    public void Remember(Unit unit, TargetMode abilityModelTargetMode) => 
        _target = unit;

    public List<Unit> GetTargets(TargetMode abilityModelTargetMode)
    {
        Platoon targetPlatoon;

        if (_platoons[0].PlatoonType == _target.PlatoonType)
            targetPlatoon = _platoons[0];
        else
            targetPlatoon = _platoons[1];
        
        List<Unit> targets = new();
        
        switch (abilityModelTargetMode)
        {
            case TargetMode.Single:
                targets.Add(_target);
               break;
            
            case TargetMode.Several:
               //Какая то логика выбора через индекс 
                break;
            
            case TargetMode.All:
                targets.AddRange(targetPlatoon.Units);
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityModelTargetMode), abilityModelTargetMode, null);
        }
        
        return targets;
    }
}