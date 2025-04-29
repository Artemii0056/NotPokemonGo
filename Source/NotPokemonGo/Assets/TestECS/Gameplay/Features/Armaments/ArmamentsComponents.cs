using System.Collections.Generic;
using Entitas;
using TestECS.Gameplay.Features.Effects;
using TestECS.Gameplay.Features.Statuses;

namespace TestECS.Gameplay.Features.Armaments
{
    public class ArmamentsComponents
    {
        [Game] public class TargetLimit : IComponent {public int Value;}
        [Game] public class EffectSetups : IComponent {public List<EffectSetup> Value;}
        [Game] public class StatusSetups : IComponent {public List<StatusSetup> Value;}
        [Game] public class Armament : IComponent {}
        [Game] public class Processed : IComponent {}
        [Game] public class ReachedToTarget : IComponent {}
        [Game] public class BouncingArmament : IComponent {}
    }
}