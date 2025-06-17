using System.Collections.Generic;
using Infrastructure.MVP;
using Stats;
using Statuses;

namespace Units
{
    public class UnitModel : Model
    {
        private Dictionary<StatType, StatSetup> _stats;
        private List<Status> _imposedStatuses;

        public UnitModel(List<StatConfig> statConfig)
        {
            _stats = new Dictionary<StatType, StatSetup>();
            _imposedStatuses = new List<Status>();
            
            foreach (var statSetup in statConfig)
            {
                _stats.Add(statSetup.StatsType, new StatSetup(statSetup));
            }
        }

        public void ChangeStat(StatType statType, float value)
        {
            if (_stats.ContainsKey(statType) == false)
                throw new KeyNotFoundException($"No stat found for type {statType}");
            
            _stats[statType].Modify(value);
        }
        
        public void AddStatus(Status status)
        {
            _imposedStatuses.Add(status);
        }

        public void RemoveStatus(Status status)
        {
            _imposedStatuses.Remove(status);
        }
    }
}