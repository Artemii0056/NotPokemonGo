using System;
using AbilityNew.Scripts.Configs;

namespace AbilityNew.Scripts.Steps.Flow
{
    [Serializable]
    public class SetBlackboardBoolStep : AbilityStepSO
    {
        public BlackboardKey Key;
        public bool Value;
    }
}