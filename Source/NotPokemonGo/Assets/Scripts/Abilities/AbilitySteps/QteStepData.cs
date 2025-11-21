using System;
using Cameras;
using QTESystem;

namespace Abilities.AbilitySteps
{
  [Serializable]
  public sealed class QteStepData : AbilityStepData
  {
    public QteType QteType;
    public CameraActionType CameraActionType;
  }
}