using System.Collections.Generic;
using UnityEngine;

namespace TestECS.Gameplay.Features.Abilities.Configs
{
    [CreateAssetMenu(menuName = "ESC Survivors", fileName = "AbilityConfig", order = 0)]
    public class AbilityConfig : ScriptableObject
    {
        public AbilityId AbilityId;
        public List<AbilityLevel> AbilityLevels;
    }
}