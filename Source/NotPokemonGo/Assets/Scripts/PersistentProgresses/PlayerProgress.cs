using System;

namespace PersistentProgresses
{
  [Serializable]
  public class PlayerProgress
  {
    public LevelProgress LevelProgress;

    public PlayerProgress()
    {
      LevelProgress = new LevelProgress();
    }
  }
}