using System;
using Abilities.Signals;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts.Configs
{
    [Serializable]
    public class WaitSignalStep : AbilityStepSO
    {
        public PhaseSignal Signal;
    }
}