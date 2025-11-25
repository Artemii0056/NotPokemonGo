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
    public float TimeScale = 0.1f;
    public bool ReqiredForNextStep = false;
  }
}