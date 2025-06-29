using System;
using Abilities.AbilityActions.Armaments;
using Abilities.AbilityActions.Castaments;

namespace Abilities
{
    [Serializable]
    public class AbilityPhase
    {
        public ArmamentSetup ArmamentSetup;
        public CastamentSetup CastamentSetup;
    }
}