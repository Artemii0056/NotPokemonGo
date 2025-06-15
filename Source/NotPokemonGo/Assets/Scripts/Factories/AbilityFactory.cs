using System.Collections.Generic;
using Abilities;
using Statuses;
using Units;

namespace Factories
{
    public class AbilityFactory
    {
        private StatusFactory _statusFactory;

        public AbilityFactory(StatusFactory statusFactory) => 
            _statusFactory = statusFactory;

        public Ability Create(AbilityConfig abilityConfig, Unit targetUnit, EffectResolver effectResolver)
        {
            List<Status> statuses = new List<Status>();
            List<EffectInfo> effectInfos = new List<EffectInfo>();

            foreach (var statusSetup in abilityConfig.Statuses)
                statuses.Add(_statusFactory.Create(statusSetup, targetUnit, effectResolver)); 

            foreach (var effectInfo in abilityConfig.EffectInfo)
                effectInfos.Add(new EffectInfo(effectInfo.Type, effectInfo.Value));
            
            return new Ability(abilityConfig, statuses, effectInfos, targetUnit);
        }
    }
}