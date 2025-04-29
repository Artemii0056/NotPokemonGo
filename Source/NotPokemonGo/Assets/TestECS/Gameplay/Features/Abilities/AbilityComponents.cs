using Entitas;

namespace TestECS.Gameplay.Features.Abilities
{
    public class AbilityComponents
    {
        [Game] public class AbilityIdComponent : IComponent {public AbilityId Value;}
        [Game] public class VegetableBoltAbility : IComponent {}
        [Game] public class RadialBoltAbility : IComponent {}
        [Game] public class BouncingBoltAbility : IComponent {}
        [Game] public class NeedTarget : IComponent {}
    }
}