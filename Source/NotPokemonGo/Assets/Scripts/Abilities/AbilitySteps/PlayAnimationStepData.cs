using System;
using Units;
using UnityEngine;

namespace Abilities.AbilitySteps
{
  [Serializable]
  public sealed class PlayAnimationStepData : AbilityStepData
  {
    public AnimationClip Clip;
    public bool WaitForFinish = true;
    public override void Accept(IAbilityStepVisitor visitor, Unit source, Unit target)
      => visitor.Visit(this, source, target);
  }
}