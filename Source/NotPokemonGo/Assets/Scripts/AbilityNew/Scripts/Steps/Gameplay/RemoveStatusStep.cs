using System;
using Abilities;
using AbilityNew.Scripts.Configs;
using Statuses;

namespace AbilityNew.Scripts.Steps.Gameplay
{
    [Serializable]
    public class RemoveStatusStep : AbilityStepSO
    {
        public StatusType StatusType;
        public TargetMode TargetMode;
    }
}