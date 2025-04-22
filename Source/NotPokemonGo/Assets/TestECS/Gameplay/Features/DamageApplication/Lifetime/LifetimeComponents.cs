using Entitas;

namespace TestECS.Gameplay.Features.DamageApplication.Lifetime
{
    public class LifetimeComponents
    {
        [Game] public class CurrentHealthPoint : IComponent { public float Value; }
        [Game] public class MaxHealthPoint : IComponent { public float Value; }
        [Game] public class Dead : IComponent {  }
        [Game] public class ProcessingDeath : IComponent {  }
    }
}