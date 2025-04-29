using System;
using System.Collections.Generic;
using System.Linq;

namespace TestECS.Gameplay.Features.CharacterStats
{
    public enum Stats
    {
        Unknown = 0,
        Speed = 1,
        MaxHealth = 2,
        Damage = 3
    }

    public static class InitStats
    {
        public static Dictionary<Stats, float> EmptyStatDictionary()
        {
            return Enum.GetValues(typeof(Stats))
                .Cast<Stats>()
                .Except(new []{Stats.Unknown})
                .ToDictionary(stat => stat, _ => 0f);
        }
    }
}