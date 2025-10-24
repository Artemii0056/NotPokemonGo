using System;

namespace PersistentProgresses
{
  [Serializable]
  public class ProjectProgress
  {
    public PlayerProgress PlayerProgress;

    public ProjectProgress(PlayerProgress playerProgress)
    {
      PlayerProgress = playerProgress;
    }
  }
}