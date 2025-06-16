using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace Characters
{
    public class Character : MonoBehaviour
    {
        private Dictionary<StatType, StatSetup> _stats = new Dictionary<StatType, StatSetup>();
        public CharacterStep Step { get; private set; }
        public bool IsAlive { get; private set; }
        public string Name { get; set; }

        public void Initialize(List<StatConfig> statConfig)
        {
            Step = new CharacterStep(5);
            
            IsAlive = true;
            foreach (var statSetup in statConfig)
            {
                _stats.Add(statSetup.StatsType, new StatSetup(statSetup));
            }
        }

        public float GetStat(StatType statType)
        {
            return _stats[statType].CurrentValue;
        }

        public void ChangeValue(StatType statType, float value)
        {
            _stats[statType].Modify(value);
        }

        public void UseAbility()
        {
            Step.ResetCurrentValue();
        }

        public void UpdateStepCurrentValue(float deltaTime)
        {
            Step.IncreaseCurrentValue(deltaTime * GetStat(StatType.Agility));
        }
    }
}