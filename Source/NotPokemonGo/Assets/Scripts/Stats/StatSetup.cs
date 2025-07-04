using System;

namespace Stats
{
    public class StatSetup
    {
        public event Action<float, StatType> CurrentValueChanged;
        
        public StatSetup(StatConfig statConfig)
        {
            Type = statConfig.StatsType;
            BaseValue = statConfig.Value;
            CurrentValue = BaseValue;
        }
        
        public StatType Type { get; private set; }
        public float BaseValue { get; private set; }
        public float CurrentValue { get; private set; }

        public void Modify(float value)
        {
            CurrentValue += value;
            CurrentValueChanged?.Invoke(CurrentValue,  Type);
        }

        public void Set(float value)
        {
            CurrentValue = value;
            CurrentValueChanged?.Invoke(CurrentValue,  Type);
        }

        public void Reset()
        {
            CurrentValue = BaseValue;
            CurrentValueChanged?.Invoke(CurrentValue,  Type);
        }
    }
}