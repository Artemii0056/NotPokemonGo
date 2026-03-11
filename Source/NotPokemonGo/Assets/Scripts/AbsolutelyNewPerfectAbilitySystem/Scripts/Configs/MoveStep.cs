using System;
using UnityEngine;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts.Configs
{
    [Serializable]
    public class MoveStep : AbilityStepSO
    {
        public AnimationClip Animation;
        public float Speed;
        public float Duration;
    }
}