using System;
using Abilities.AbilityActions.Castaments;
using Units;

namespace Abilities.AbilitySteps
{
  [Serializable]
  public sealed class CastamentStepData : AbilityStepData
  {
    public CastamentSetup Castament;
    public TargetMode TargetMode;
    public override void Accept(IAbilityStepVisitor visitor, Unit source, Unit target)
      => visitor.Visit(this, source, target);
  }
}