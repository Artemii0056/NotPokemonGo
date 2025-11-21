using PersistentProgresses;

namespace SaveLoadService
{
  public interface IProgressReader
  {
    void ReadProgress(ProjectProgress projectProgress);
  }
}