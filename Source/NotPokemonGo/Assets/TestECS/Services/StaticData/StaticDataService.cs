using System;
using System.Collections.Generic;
using System.Linq;
using TestECS.Gameplay.Features.Abilities;
using TestECS.Gameplay.Features.Abilities.Configs;
using TestECS.Gameplay.Features.Abilities.Factory;
using UnityEngine;

namespace TestECS.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<AbilityId, AbilityConfig> _abilityByConfigs;

        public StaticDataService()
        {
            LoadAbilities();
        }

        // public void LoadAll()
        // {
        //     LoadAbilities();
        // }

        public AbilityConfig GetAbilityById(AbilityId abilityId)
        {
            if (_abilityByConfigs.TryGetValue(abilityId, out AbilityConfig abilityConfig))
                return abilityConfig;

            throw new Exception($"Ability with id {abilityId} not found");
        }

        public AbilityLevel GetAbilityLevel(AbilityId abilityId, int level)
        {
            AbilityConfig config = GetAbilityById(abilityId);

            if (level > config.AbilityLevels.Count) 
                level = config.AbilityLevels.Count;
            
            return config.AbilityLevels[level - 1];
        }

        private void LoadAbilities()
        {
           _abilityByConfigs = Resources
               .LoadAll<AbilityConfig>("Abilities") 
               .ToDictionary(x => x.AbilityId, x => x);
        }
    }
}