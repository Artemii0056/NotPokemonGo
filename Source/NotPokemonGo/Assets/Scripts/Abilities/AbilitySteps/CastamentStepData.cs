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
    public bool WhenQteSuccess;
  }
}