
using Entitas;

namespace TestECS.Gameplay.Features.Effects
{
    [Game]public class Effect : IComponent {}
    [Game]public class ProducerId : IComponent {public int Value;}
    [Game]public class EffectTargetId : IComponent {public int Value;}
    [Game]public class EffectValue : IComponent {public float Value;}
    [Game]public class DamageEffect : IComponent {}
}