namespace Characters
{
    public class CharacterStep
    {
        public CharacterStep(float maxValue)
        {
            MaxValue = maxValue;
            CurrentValue = 0f;
        }

        public float CurrentValue { get; private  set; }
        public float MaxValue { get; private set; }

        public bool IsReadyToAct => CurrentValue >= MaxValue;

        public void IncreaseCurrentValue(float value)
        {
            if (value > 0)
                CurrentValue += value;
        }

        public void DecreaseCurrentValue(float value)
        {
            if (value > 0)
                CurrentValue -= value;
            
            if (CurrentValue < 0)
                ResetCurrentValue();
        }

        public void ChangeMaxValue(float value)
        {
            if (value > 0)
                MaxValue = value;
        }

        public void ResetCurrentValue() =>
            CurrentValue = 0;
    }
}