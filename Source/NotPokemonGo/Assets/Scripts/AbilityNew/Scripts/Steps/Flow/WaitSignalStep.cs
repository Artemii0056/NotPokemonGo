using System;
using Abilities.Signals;
using AbilityNew.Scripts.Configs;

namespace AbilityNew.Scripts.Steps.Flow
{
    [Serializable]
    public class WaitSignalStep : AbilityStepSO
    {
        public PhaseSignal Signal;
    }
}