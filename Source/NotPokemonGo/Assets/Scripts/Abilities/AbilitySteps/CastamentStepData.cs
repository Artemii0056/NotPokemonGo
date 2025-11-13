using System;
using Abilities.AbilityActions.Castaments;

namespace Abilities.AbilitySteps
{
  [Serializable]
  public sealed class CastamentStepData : AbilityStepData
  {
    public CastamentSetup Castament;
    public TargetMode TargetMode;
  }
}