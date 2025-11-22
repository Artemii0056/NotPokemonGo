using System.Collections.Generic;
using Abilities.AbilitySteps;
using UnityEngine;

namespace Abilities.MV
{
    public class AbilityModel
    {
        private Dictionary<AbilityStatType, AbilityStatSetup> _stats;

        public AbilityModel(AbilityConfig config)
        {
            AbilityType = config.AbilityType;
            
            Steps = config.Steps;

            _stats = new Dictionary<AbilityStatType, AbilityStatSetup>();

            foreach (AbilityStatSetup abilityStatSetup in config.AbilityStatSetup) 
                _stats[abilityStatSetup.StatsType] = abilityStatSetup;
        }
        
        public AbilityType AbilityType { get; private set; }
        public TargetMode TargetMode { get; private set; }
        
        public List<AbilityStepData> Steps { get; private set; }
        
        public float Cost => _stats[AbilityStatType.Cost].Value; 

        public bool IsReady()
        {
            if (_stats.ContainsKey(AbilityStatType.CurrentTime) == false)
            {
                Debug.LogError("No stats have been assigned " + AbilityType);
                return false;
            }
            
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