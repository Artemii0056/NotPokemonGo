using System;
using Units;
using UnityEngine;

namespace Abilities.AbilitySteps
{
  [Serializable]
  public sealed class PlayAnimationStepData : AbilityStepData
  {
    public AnimationClip Clip;
    public string AnimationName;
    public bool WaitForFinish = true;
  }
}