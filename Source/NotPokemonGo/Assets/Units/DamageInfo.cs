namespace Units
{
    public struct DamageInfo
    {
        public DamageType Type;
        public float Value;
        public Unit Source;

        public DamageInfo(DamageType type, float value, Unit source)
        {
            Type = type;
            Value = value;
            Source = source;
        }
    }
}