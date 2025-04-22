using UnityEngine;

namespace TestECS.Levels
{
  public class LevelDataProvider : ILevelDataProvider
  {
    public Vector3 StartPoint { get; private set; } = new Vector3(0, 0.5f, 0);

    public void SetStartPoint(Vector3 startPoint)
    {
      StartPoint = startPoint;
    }
  }
}