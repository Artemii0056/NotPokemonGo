using System;
using System.Collections.Generic;
using Map;

namespace PersistentProgresses
{
  [Serializable]
  public class LevelProgress
  {
    public Dictionary<MapType, int> StartMapsCompleted = new Dictionary<MapType, int>();
  }
}