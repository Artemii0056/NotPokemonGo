using System;
using Abilities.AbilityActions.Armaments;
using Units;

namespace Abilities.AbilitySteps
{
  [Serializable]
  public sealed class ArmamentStepData : AbilityStepData
  {
    public ArmamentSetup Armament;
    public TargetMode TargetMode;
    public override void Accept(IAbilityStepVisitor visitor, Unit source, Unit target)
      => visitor.Visit(this, source, target);
  }
}