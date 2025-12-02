using System;
using System.Collections.Generic;
using Abilities;
using Units;

namespace Platoons
{
    public class TargetSelector : ITargetSelector
    {
        private List<Platoon> _platoons = new();

        public void SetPlatoons(Platoon platoon, Platoon platoon2)
        {
            _platoons.Add(platoon);
            _platoons.Add(platoon2);
        }

        public List<Unit> GetTargets(TargetMode abilityModelTargetMode, Unit target) //Сюда передать и таргет сразу 
        {
            Platoon targetPlatoon;
        
            if (_platoons[0].Type == target.PlatoonType)
                targetPlatoon = _platoons[0];
            else
                targetPlatoon = _platoons[1];
        
            List<Unit> targets = new();
        
            switch (abilityModelTargetMode)
            {
                case TargetMode.Single:
                    targets.Add(target);
                    break;
            
                case TargetMode.Several:
                    //Какая то логика выбора через индекс 
                    break;
            
                case TargetMode.All:
                    targets.AddRange(targetPlatoon.AliveUnits);
                    break;
            
                default:
                    throw new ArgumentOutOfRangeException(nameof(abilityModelTargetMode), abilityModelTargetMode, null);
            }
        
            return targets;
        }
    }
}