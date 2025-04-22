using Entitas;

namespace TestECS.Gameplay.Features.Cooldowns
{
    public class CooldownsComponents
    {
        [Game] public class Cooldown : IComponent {public float Value;}
        [Game] public class CooldownLeft : IComponent {public float Value;}
        [Game] public class CooldownUp : IComponent {}
    }
}