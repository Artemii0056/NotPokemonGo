namespace TestECS.Gameplay.Features.Abilities.Factory
{
    public interface IAbilityFactory
    {
        GameEntity CreateVegetableBoltAbility(int level);
        GameEntity CreateRadialBoltAbility(int level);
        GameEntity CreateBouncingBoltAbility(int level);
    }
}