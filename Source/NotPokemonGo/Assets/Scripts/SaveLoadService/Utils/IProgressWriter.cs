using PersistentProgresses;

namespace SaveLoadService
{
  public interface IProgressWriter : IProgressReader
  {
    void WriteProgress(ProjectProgress projectProgress);
  }
}