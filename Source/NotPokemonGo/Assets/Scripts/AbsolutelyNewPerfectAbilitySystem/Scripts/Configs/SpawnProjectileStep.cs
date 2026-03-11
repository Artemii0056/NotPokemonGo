using System;
using Armaments;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts.Configs
{
    [Serializable]
    public class SpawnProjectileStep : AbilityStepSO
    {
        public ArmamentSetup ArmamentSetup; //Префаб передать
        public float Delay;
    }
}