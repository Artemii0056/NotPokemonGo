using System;
using Cameras;
using QTESystem;
using Units;

namespace Abilities.AbilitySteps
{
  [Serializable]
  public sealed class QteStepData : AbilityStepData
  {
    public QteType QteType;
    public CameraActionType CameraActionType;
    
    public override void Accept(IAbilityStepVisitor visitor, Unit source, Unit target)
      => visitor.Visit(this, source, target);
  }
}