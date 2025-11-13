using System;
using UnityEngine;

namespace Abilities.AbilitySteps
{
  [Serializable]
  public sealed class PlayAnimationStepData : AbilityStepData
  {
    public AnimationClip Clip;
    public bool WaitForFinish = true;
  }
}