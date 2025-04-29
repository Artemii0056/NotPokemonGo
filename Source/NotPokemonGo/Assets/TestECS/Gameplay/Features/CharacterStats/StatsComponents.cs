using System.Collections.Generic;
using Entitas;

namespace TestECS.Gameplay.Features.CharacterStats
{
    public class StatsComponents
    {
        [Game] public class BaseStats : IComponent {public Dictionary<Stats, float> Value;}
        [Game] public class StatModifiers : IComponent {public Dictionary<Stats, float> Value;}

        [Game] public class StatChange : IComponent {public Stats Value;}
    }
}