using System;
using Abilities;
using AbilityNew.Scripts.Configs;
using Statuses;

namespace AbilityNew.Scripts.Steps.Gameplay
{
    [Serializable]
    public class ApplyStatusStep : AbilityStepSO
    {
        public StatusSetup Status;
        public TargetMode TargetMode;
    }
}