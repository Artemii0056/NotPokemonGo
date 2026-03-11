using System;
using AbilityNew.Scripts.Configs;
using Armaments;

namespace AbilityNew.Scripts.Steps.Gameplay
{
    [Serializable]
    public class SpawnProjectileStep : AbilityStepSO
    {
        public ArmamentSetup ArmamentSetup; //Префаб передать
        public float Delay;
    }
}