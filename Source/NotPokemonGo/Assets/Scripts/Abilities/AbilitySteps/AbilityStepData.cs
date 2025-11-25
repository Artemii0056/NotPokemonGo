using System;
using Units;

namespace Abilities.AbilitySteps
{
  [Serializable]
  public abstract class AbilityStepData
  {
    public abstract void Accept(IAbilityStepVisitor visitor, Unit source, Unit target);
  }
}