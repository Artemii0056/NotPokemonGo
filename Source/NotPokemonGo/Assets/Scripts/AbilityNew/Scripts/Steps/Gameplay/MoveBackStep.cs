using System;
using AbilityNew.Scripts.Configs;
using UnityEngine;

namespace AbilityNew.Scripts.Steps.Gameplay
{
    [Serializable]
    public class MoveBackStep : AbilityStepSO
    {
        public AnimationClip Animation;
        public float Speed;
        public float Duration;
    }
}