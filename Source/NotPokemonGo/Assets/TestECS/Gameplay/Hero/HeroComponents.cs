using Entitas;
using TestECS.Gameplay.Hero.Code;

namespace TestECS.Gameplay.Hero
{
    [Game] public class Hero : IComponent { };
    [Game] public class HeroAnimatorComponent : IComponent { public HeroAnimator Value;};
}