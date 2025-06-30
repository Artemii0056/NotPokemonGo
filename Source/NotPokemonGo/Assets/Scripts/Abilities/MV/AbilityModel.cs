using System.Collections.Generic;
using Abilities.AbilityActions.Armaments;
using Abilities.AbilityActions.Castaments;

namespace Abilities.MV
{
    public class AbilityModel
    {
        public AbilityModel(AbilityConfig config)
        {
            TargetMode = config.TargetMode;
            AbilityType = config.AbilityType;
            
            Cost = config.Cost;
            Cooldown = config.Cooldown;

            Phases = config.Phases;
        }

        public AbilityType AbilityType { get; private set; }
        public TargetMode TargetMode { get; private set; }
        
        public List<AbilityPhase> Phases { get; private set; }
        
        public float Cost { get; private set; }
        public float CurrentTime { get; private set; } = 0;
        public float Cooldown { get; private set; }
        public bool IsReady => CurrentTime >= Cooldown;
        
        public void UpdateTime(float deltaTime) =>
            CurrentTime += deltaTime;

        public void DiscardCurrentTime() =>
            CurrentTime = 0;
    }
}