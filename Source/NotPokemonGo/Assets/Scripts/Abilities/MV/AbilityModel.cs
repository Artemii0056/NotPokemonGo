using System.Collections.Generic;
using System.Linq;
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
            
            CastamentSetup = config.CastamentSetup;
            ArmamentSetup = config.ArmamentSetup;
            
            Phases = config.Phases;

            _stats = new Dictionary<AbilityStatType, AbilityStatSetup>();
            foreach (AbilityStatSetup abilityStatSetup in config.AbilityStatSetup) 
                _stats[abilityStatSetup.StatsType] = abilityStatSetup;
        }

        private Dictionary<AbilityStatType, AbilityStatSetup> _stats;

        public AbilityType AbilityType { get; private set; }
        public TargetMode TargetMode { get; private set; }
        
        public List<AbilityPhase> Phases { get; private set; }
        
        public ArmamentSetup ArmamentSetup { get; private set; }
        public CastamentSetup CastamentSetup { get; private set; }
        
        public bool HasArmament => ArmamentSetup.HasSetupData;
        public bool HasCastament => CastamentSetup.HasSetupData;
        public float Cost => _stats[AbilityStatType.Cost].Value; 

        public bool IsReady()
        {
            float currentTime = _stats[AbilityStatType.CurrentTime].Value;
            float cooldown = _stats[AbilityStatType.Cooldown].Value;

            if (currentTime >= cooldown)
            {
                _stats[AbilityStatType.CurrentTime].Value = cooldown;
                return true;
            }

            return false;
        }
        
        public void DiscardCurrentTime() => 
            _stats[AbilityStatType.CurrentTime].Value = 0;

        public void Tick()
        {
            if (IsReady())
                return;

            _stats[AbilityStatType.CurrentTime].Value += _stats[AbilityStatType.CooldownSpeed].Value;
        }
    }
}