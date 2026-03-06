using System;
using Armaments;

namespace AbsolutelyNewPerfectAbilitySystem.Configs
{
    [Serializable]
    public class SpawnProjectileStepSO : AbilityStepSO
    {
        public ArmamentSetup ArmamentSetup;
        public float Delay;
    }
}