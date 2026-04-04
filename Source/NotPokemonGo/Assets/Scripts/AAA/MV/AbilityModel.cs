using System.Collections.Generic;
using AbilityNew.Scripts;

namespace Abilities.MV
{
    public class AbilityModel
    {
        public AbilityModel(AbilitySo config)
        {
            ConfigSO = config;
            AbilityType = config.Type;
            
            _stats = new Dictionary<AbilityStatType, AbilityStatSetup>();

            foreach (AbilityStatSetup abilityStatSetup in config.AbilityStatSetup) 
                _stats[abilityStatSetup.StatsType] = abilityStatSetup;
        }

        private Dictionary<AbilityStatType, AbilityStatSetup> _stats;

        public AbilitySo ConfigSO { get; }

        public AbilityType AbilityType { get; private set; }
        public TargetMode TargetMode { get; private set; }
        
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