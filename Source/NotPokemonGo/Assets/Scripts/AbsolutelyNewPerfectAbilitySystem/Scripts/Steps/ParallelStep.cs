using System;
using System.Collections.Generic;
using AbsolutelyNewPerfectAbilitySystem.Configs;

namespace AbsolutelyNewPerfectAbilitySystem.Steps
{
    [Serializable]
    public class ParallelStep : AbilityStepSO
    {
        public List<AbilityStepSO> Steps;
    }
}