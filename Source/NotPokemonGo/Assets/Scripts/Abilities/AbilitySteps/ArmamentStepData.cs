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
    public bool WhenQteSuccess;
  }
}