using System;
using Abilities.AbilityActions.Armaments;

namespace Abilities.AbilitySteps
{
  [Serializable]
  public sealed class ArmamentStepData : AbilityStepData
  {
    public ArmamentSetup Armament;
    public TargetMode TargetMode;
  }
}