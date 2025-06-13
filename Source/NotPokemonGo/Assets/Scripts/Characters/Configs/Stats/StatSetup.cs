namespace Characters.Configs.Stats
{
    public class StatSetup
    {
        public StatType StatType { get; private set; }
        public float BaseValue { get; private set; }
        public float CurrentValue { get; private set; }

        public void Modify(float value) => 
            CurrentValue += value;

        public void Set(float value) => 
            CurrentValue = value;

        public void Reset() => 
            CurrentValue = BaseValue;
    }
}