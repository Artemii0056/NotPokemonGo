using System;
using AbilityNew.Scripts.Configs;
using UnityEngine;

namespace AbilityNew.Scripts.Steps.Gameplay
{
    [Serializable]
    public class MoveStep : AbilityStepSO
    {
        public AnimationClip Animation;
        public float Speed;
    }
}