using System;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities.Configs
{
    [Serializable]
    public class AbilityPart
    {
        [field: SerializeField] public List<AbilityPhase> AbilityPhases { get; private set; }
    }
}