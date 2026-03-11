using System;
using UnityEngine;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts.Configs
{
    [Serializable]
    public class MoveBackStep : AbilityStepSO
    {
        public AnimationClip Animation;
        public float Speed;
        public float Duration;
    }
}