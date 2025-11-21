using System.Collections.Generic;

namespace SaveLoadService
{
  public interface ISaveLoadService
  {
    List<IProgressReader> ProgressReaders { get; }
    bool HasSavedProgress { get; }
    void SaveProgress();
    void LoadProgress();
    void DeleteSaves();
  }
}