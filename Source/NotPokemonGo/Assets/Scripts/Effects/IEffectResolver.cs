namespace Units
{
    public interface IEffectResolver
    {
        float CalculateFinalValue(Unit target, EffectInfo effectInfo);
    }
}