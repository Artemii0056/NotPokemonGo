using System;
using Abilities;
using AbilityNew.Scripts.Configs;
using Effects;

namespace AbilityNew.Scripts.Steps.Gameplay
{
    [Serializable]
    public class DamageStep : AbilityStepSO
    {
        public EffectInfo Effect;
        public TargetMode TargetMode;
    }
}