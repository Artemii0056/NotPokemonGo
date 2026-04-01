using System.Collections.Generic;
using Abilities.Configs;
using AbilityNew.Scripts;

namespace Abilities.MV
{
    public class AbilityModel
    {
        public AbilityModel(AbilityConfig config)
        {
            Config = config;
            AbilityType = config.AbilityType;
            
           Parts = config.Parts;
           Interruptibility = config.Interruptibility;

            _stats = new Dictionary<AbilityStatType, AbilityStatSetup>();

            foreach (AbilityStatSetup abilityStatSetup in config.AbilityStatSetup) 
                _stats[abilityStatSetup.StatsType] = abilityStatSetup;
        }
        
        public AbilityModel(AbilitySO config)
        {
            ConfigSO = config;
            AbilityType = config.Type;
            
           // Parts = config.Parts;
           // Interruptibility = config.Interruptibility;

            _stats = new Dictionary<AbilityStatType, AbilityStatSetup>();

            foreach (AbilityStatSetup abilityStatSetup in config.AbilityStatSetup) 
                _stats[abilityStatSetup.StatsType] = abilityStatSetup;
        }

        private Dictionary<AbilityStatType, AbilityStatSetup> _stats;

        public AbilityConfig Config { get; }
        public AbilitySO ConfigSO { get; }

        public AbilityType AbilityType { get; private set; }
        public TargetMode TargetMode { get; private set; }
        public Interruptibility Interruptibility { get;  }
        
        public List<AbilityPart> Parts { get; private set; }
        
        
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